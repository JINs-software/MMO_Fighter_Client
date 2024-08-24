using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

public class NetBuffer
{
    private byte[] m_Buffer;
    private int m_Index;

    public int BufferedSize { get { return m_Index; } }
    public int FreeSize { get { return m_Buffer.Length - m_Index; } }

    public NetBuffer(int buffSize)
    {
        m_Buffer = new byte[buffSize];
    }

    public bool Peek(byte[] dest, int length, int offset = 0)
    {
        if(length + offset > m_Index)
        {
            return false;
        }

        Array.Copy(m_Buffer, offset, dest, 0, length);
        return true;
    }

    public bool Write(byte[] source, int length, int offset = 0)
    {
        if (m_Index + length > m_Buffer.Length)
        {
            return false;
        }

        Array.Copy(source, offset, m_Buffer, m_Index, length);
        m_Index += length;
        return true;
    }
    public bool WriteFront(byte[] source, int length, int offset = 0)
    {
        if (m_Index + length > m_Buffer.Length)
        {
            return false;
        }

        if (m_Index == 0)
        {
            Write(source, length, offset);
        }
        else
        {
            byte[] newBuffer = new byte[m_Buffer.Length];

            Array.Copy(source, 0, newBuffer, 0, length);
            Array.Copy(m_Buffer, 0, newBuffer, length, BufferedSize);
            m_Index = BufferedSize + length;
            m_Buffer = newBuffer;
        }

        return true;
    }

    public bool Read(byte[] dest, int length)
    {
        if (m_Index < length)
        {
            return false;
        }

        Array.Copy(m_Buffer, dest, length);

        if (m_Index == length)
        {
            m_Index = 0;
        }
        else
        {
            byte[] newBuffer = new byte[m_Buffer.Length];
            Array.Copy(m_Buffer, length, newBuffer, 0, BufferedSize - length);
            m_Index = BufferedSize - length;
            m_Buffer = newBuffer;
        }
        return true;
    }
}

public class NetworkManager
{
    private TcpClient m_TcpClient = null;
    private NetworkStream m_Stream = null;

    private NetBuffer m_RecvBuffer = new NetBuffer(1024);

    private System.Random m_RandKeyMaker = new System.Random();

    public NetworkManager(IPEndPoint ipEndPoint = null)
    {
        if (ipEndPoint == null)
        {
            m_TcpClient = new TcpClient();
        }
        else
        {
            m_TcpClient = new TcpClient(ipEndPoint);
            m_Stream = m_TcpClient.GetStream();
        }
    }

    public bool Connected { get { return m_TcpClient.Connected; } }

    public bool Connect(string serverIP = "127.0.0.1", int port = 7777)
    {
        if (!Connected)
        {
            try
            {
                m_TcpClient.Connect(IPAddress.Parse(serverIP), port);
                m_Stream = m_TcpClient.GetStream();
            }
            catch
            {
                return false;
            }
        }

        return true;
    }

    public void Disconnect()
    {
        if (Connected)
        {
            m_TcpClient.Close();
        }
    }

    public void ClearRecvBuffer()
    {
        while (m_Stream.DataAvailable)
        {
            byte[] buffer = new byte[1024];
            m_Stream.Read(buffer, 0, buffer.Length);
        }
    }

    public void SendBytes(byte[] data)
    {
        m_Stream.Write(data);
    }

    public bool ReceiveDataAvailable()
    {
        return m_Stream.DataAvailable;
    }

    public int ReceivedDataSize()
    {
        return m_RecvBuffer.BufferedSize + m_TcpClient.Available;
    }

    public bool Peek<T>(out T data)
    {
        int dataSize = Marshal.SizeOf(typeof(T));
        if (m_RecvBuffer.BufferedSize + m_TcpClient.Available < dataSize)
        {
            data = default(T);
            return false;
        }

        if(m_RecvBuffer.BufferedSize < dataSize)
        {
            int resSize = dataSize - m_RecvBuffer.BufferedSize; 
            if(m_RecvBuffer.FreeSize < resSize)
            {
                data = default(T);
                return false;
            }

            byte[] buffer = new byte[resSize];  
            m_Stream.Read(buffer, 0, buffer.Length);
            m_RecvBuffer.Write(buffer, resSize, 0); 
        }

        byte[] bytes = new byte[dataSize];
        m_RecvBuffer.Peek(bytes, bytes.Length, 0);
        data = BytesToMessage<T>(bytes);

        return true;
    }

    public byte[] ReceiveBytes(int length)
    {
        byte[] bytes = new byte[length];
        if (m_RecvBuffer.BufferedSize + m_TcpClient.Available < length)
        {
            return null;
        }

        if(m_RecvBuffer.BufferedSize >= length)
        {
            m_RecvBuffer.Read(bytes, length);
        }
        else
        {
            int buffedSize = m_RecvBuffer.BufferedSize;
            m_RecvBuffer.Read(bytes, buffedSize);
            m_Stream.Read(bytes, buffedSize, length - buffedSize);
        }
        return bytes;
    }

    public bool ReceiveData<T>(out T data)
    {
        data = default(T);

        if (ReceivedDataSize() < Marshal.SizeOf<T>())
        {
            return false;
        }

        byte[] receivedBytes = new byte[Marshal.SizeOf<T>()];
        if(m_RecvBuffer.BufferedSize >= receivedBytes.Length)
        {
            m_RecvBuffer.Read(receivedBytes, receivedBytes.Length);
        }
        else
        {
            int bufferedSize = m_RecvBuffer.BufferedSize;
            m_RecvBuffer.Read(receivedBytes, bufferedSize);
            m_Stream.Read(receivedBytes, bufferedSize, receivedBytes.Length - bufferedSize);
        }

        data = BytesToMessage<T>(receivedBytes);    
        return true;
    }

    private byte[] MessageToBytes<T>(T str)
    {
        int size = Marshal.SizeOf(str);
        byte[] arr = new byte[size];
        IntPtr ptr = Marshal.AllocHGlobal(size);
        try
        {
            Marshal.StructureToPtr(str, ptr, false);
            Marshal.Copy(ptr, arr, 0, size);
        }
        catch (Exception ex)
        {
            Debugger.Break();
        }
        finally
        {
            // 할당받은 네이티브 메모리 해제
            Marshal.FreeHGlobal(ptr);
        }

        return arr;
    }

    public void MessageToBytes<T>(T str, byte[] dest)
    {
        int size = Marshal.SizeOf(str);

        IntPtr ptr = Marshal.AllocHGlobal(size);
        try
        {
            Marshal.StructureToPtr(str, ptr, false);
            Marshal.Copy(ptr, dest, 0, size);
        }
        catch (Exception ex)
        {
            Debugger.Break();
        }
        finally
        {
            // 할당받은 네이티브 메모리 해제
            Marshal.FreeHGlobal(ptr);
        }
    }

    public T BytesToMessage<T>(byte[] bytes)
    {
        T str = default(T);
        int size = Marshal.SizeOf<T>();
        IntPtr ptr = Marshal.AllocHGlobal(size);

        try
        {
            Marshal.Copy(bytes, 0, ptr, size);
            str = Marshal.PtrToStructure<T>(ptr);
        }
        catch (Exception ex)
        {
            Debugger.Break();
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }

        return str;
    }
}