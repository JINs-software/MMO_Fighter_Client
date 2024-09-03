
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;

static public class JNET_PROTOCOL
{
    public const byte JNET_PROTO_CODE = 119;
    public const byte JNET_PROTO_SYMM_KEY = 50;
    public const int RECV_BUFFER_LENGTH = 1024;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SIMPLE_MSG_HDR
    {
        public byte Code;
        public byte MsgLen;
        public byte MsgType;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_HDR
    {
        public byte Code;
        public UInt16 MsgLen;
        public byte RandomKey;
        public byte CheckSum;
    }

    static System.Random RandKeyMaker = new System.Random();
    static public byte GetRandomKey()
    {
        return (byte)RandKeyMaker.Next(0, 255);
    }

    static public void Encode(byte symmKey, byte randKey, UInt16 payloadLen, out byte checkSum, byte[] payload)
    {
        byte payloadSum = 0;
        for (ushort i = 0; i < payloadLen; i++)
        {
            payloadSum += payload[i];
            payloadSum = (byte)(payloadSum % 256);
        }
        byte Pb = (byte)(payloadSum ^ (randKey + 1));
        byte Eb = (byte)(Pb ^ (symmKey + 1));
        checkSum = Eb;

        for (ushort i = 1; i <= payloadLen; i++)
        {
            byte Pn = (byte)(payload[i - 1] ^ (Pb + randKey + i + 1));
            byte En = (byte)(Pn ^ (Eb + symmKey + i + 1));

            payload[i - 1] = En;

            Pb = Pn;
            Eb = En;
        }
    }

    static public bool Decode(byte symmKey, byte randKey, ushort payloadLen, byte checkSum, byte[] payload)
    {
        byte Pb = (byte)(checkSum ^ (symmKey + 1));
        byte payloadSum = (byte)(Pb ^ (randKey + 1));
        byte Eb = checkSum;
        byte Pn;
        byte Dn;
        byte payloadSumCmp = 0;

        for (ushort i = 1; i <= payloadLen; i++)
        {
            Pn = (byte)(payload[i - 1] ^ (Eb + symmKey + i + 1));
            Dn = (byte)(Pn ^ (Pb + randKey + i + 1));

            Pb = Pn;
            Eb = payload[i - 1];
            payload[i - 1] = Dn;
            payloadSumCmp += payload[i - 1];
            payloadSumCmp = (byte)(payloadSumCmp % 256);
        }

        if (payloadSum != payloadSumCmp) { return false; }
        return true;
    }
}

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

    public void Clear()
    {
        m_Index = 0;
    }

    public bool Peek(byte[] dest, int length, int offset = 0)
    {
        if (length + offset > m_Index)
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

    private NetBuffer m_RecvBuffer = new NetBuffer(JNET_PROTOCOL.RECV_BUFFER_LENGTH);

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

    public bool IsValidServerIpAddress(string ipAddress)
    {
        IPAddress ip;
        return IPAddress.TryParse(ipAddress, out ip);
    }

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
        m_RecvBuffer.Clear();
        byte[] buffer = new byte[m_TcpClient.Available];
        m_Stream.Read(buffer, 0, buffer.Length);
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

        if (m_RecvBuffer.BufferedSize < dataSize)
        {
            int resSize = dataSize - m_RecvBuffer.BufferedSize;
            if (m_RecvBuffer.FreeSize < resSize)
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

    public bool ReceivePacketBytes(out byte[] payloadBytes, bool decoding = true)
    {
        payloadBytes = default(byte[]);

        JNET_PROTOCOL.MSG_HDR hdr;
        Peek<JNET_PROTOCOL.MSG_HDR>(out hdr);
        if (Marshal.SizeOf<JNET_PROTOCOL.MSG_HDR>() + hdr.MsgLen > ReceivedDataSize())
        {
            return false;
        }

        ReceiveData<JNET_PROTOCOL.MSG_HDR>(out hdr);
        payloadBytes = ReceiveBytes(hdr.MsgLen);
        if (decoding)
        {
            if (!JNET_PROTOCOL.Decode(JNET_PROTOCOL.JNET_PROTO_SYMM_KEY, hdr.RandomKey, hdr.MsgLen, hdr.CheckSum, payloadBytes))
            {
                return false;
            }
        }

        return true;
    }

    public bool ReceivePacket<T>(out T payload, bool decoding = true)
    {
        payload = default(T);
        if (ReceivedDataSize() < Marshal.SizeOf<JNET_PROTOCOL.MSG_HDR>() + Marshal.SizeOf<T>())
        {
            return false;
        }

        JNET_PROTOCOL.MSG_HDR hdr;
        ReceiveData<JNET_PROTOCOL.MSG_HDR>(out hdr);
        byte[] payloadBytes = ReceiveBytes(Marshal.SizeOf<T>());
        if (payloadBytes == null)
        {
            return false;
        }

        if (decoding)
        {
            if (!JNET_PROTOCOL.Decode(JNET_PROTOCOL.JNET_PROTO_SYMM_KEY, hdr.RandomKey, hdr.MsgLen, hdr.CheckSum, payloadBytes))
            {
                return false;
            }
        }

        payload = BytesToMessage<T>(payloadBytes);
        return true;
    }

    public bool ReceiveSimplePacket<T>(out byte msgType, out T payload)
    {
        msgType = default(byte);
        payload = default(T);
        if (ReceivedDataSize() < Marshal.SizeOf<JNET_PROTOCOL.SIMPLE_MSG_HDR>() + Marshal.SizeOf<T>())
        {
            return false;
        }

        JNET_PROTOCOL.SIMPLE_MSG_HDR hdr;
        ReceiveData<JNET_PROTOCOL.SIMPLE_MSG_HDR>(out hdr);
        byte[] payloadBytes = ReceiveBytes(Marshal.SizeOf<T>());
        if (payloadBytes == null)
        {
            return false;
        }

        msgType = hdr.MsgType;
        payload = BytesToMessage<T>(payloadBytes);
        return true;
    }

    public bool ReceiveData<T>(out T data)
    {
        byte[] receivedBytes = ReceiveBytes(Marshal.SizeOf<T>());
        if (receivedBytes == null)
        {
            data = default(T);
            return false;
        }

        data = BytesToMessage<T>(receivedBytes);
        return true;
    }

    public byte[] ReceiveBytes(int length)
    {
        try
        {
            byte[] bytes = new byte[length];
            if (ReceivedDataSize() < length)
            {
                return null;
            }

            if (m_RecvBuffer.BufferedSize >= length)
            {
                m_RecvBuffer.Read(bytes, length);
            }
            else
            {
                int buffedSize = m_RecvBuffer.BufferedSize;
                m_RecvBuffer.Read(bytes, buffedSize);

                int bytesRead = m_Stream.Read(bytes, buffedSize, length - buffedSize);
                if (bytesRead < length - buffedSize)
                {
                    throw new IOException("스트림에서 예상한 만큼의 데이터를 읽지 못함");
                }
            }
            return bytes;
        }
        catch (IOException ex)
        {
            // 네트워크 오류로 인해 발생하는 IOException 처리
            Console.WriteLine($"Network Error : {ex.Message}");

            // [추가적인 재시도 로직이나 네트워크 복구 로직]
            // ... 
        }
        catch (ObjectDisposedException ex)
        {
            // 스트림이 이미 닫혔을 때 발생하는 예외 처리
            Console.WriteLine($"Stream Closed : {ex.Message}");
        }
        catch (SocketException ex)
        {
            // 소켓 관련 오류 처리
            Console.WriteLine($"Socekt Error : {ex.Message}");
        }
        catch (Exception ex)
        {
            // 기타
            Console.WriteLine($"알 수 없는 오류 발생: {ex.Message}");
        }

        return null;
    }

    public void SendPacketBytes(byte[] payloadBytes, bool encoding = true)
    {
        JNET_PROTOCOL.MSG_HDR hdr = new JNET_PROTOCOL.MSG_HDR();
        hdr.Code = JNET_PROTOCOL.JNET_PROTO_CODE;
        hdr.MsgLen = (UInt16)payloadBytes.Length;
        hdr.RandomKey = JNET_PROTOCOL.GetRandomKey();

        if (encoding)
        {
            JNET_PROTOCOL.Encode(JNET_PROTOCOL.JNET_PROTO_SYMM_KEY, hdr.RandomKey, hdr.MsgLen, out hdr.CheckSum, payloadBytes);
        }

        byte[] hdrAndPayload = new byte[Marshal.SizeOf<JNET_PROTOCOL.MSG_HDR>() + payloadBytes.Length];

        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf<JNET_PROTOCOL.MSG_HDR>());
        try
        {
            Marshal.StructureToPtr(hdr, ptr, false);
            Marshal.Copy(ptr, hdrAndPayload, 0, Marshal.SizeOf<JNET_PROTOCOL.MSG_HDR>());
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
        Buffer.BlockCopy(payloadBytes, 0, hdrAndPayload, Marshal.SizeOf<JNET_PROTOCOL.MSG_HDR>(), payloadBytes.Length);

        SendBytes(hdrAndPayload);
    }

    public void SendPacket<T>(T payload, bool encoding = true)
    {
        byte[] bytes = MessageToBytes(payload);

        JNET_PROTOCOL.MSG_HDR hdr = new JNET_PROTOCOL.MSG_HDR();
        hdr.Code = JNET_PROTOCOL.JNET_PROTO_CODE;
        hdr.MsgLen = (UInt16)Marshal.SizeOf<T>();
        hdr.RandomKey = JNET_PROTOCOL.GetRandomKey();

        if (encoding)
        {
            JNET_PROTOCOL.Encode(JNET_PROTOCOL.JNET_PROTO_SYMM_KEY, hdr.RandomKey, hdr.MsgLen, out hdr.CheckSum, bytes);
        }

        byte[] hdrAndPayload = new byte[Marshal.SizeOf<JNET_PROTOCOL.MSG_HDR>() + Marshal.SizeOf<T>()];

        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf<JNET_PROTOCOL.MSG_HDR>());
        try
        {
            Marshal.StructureToPtr(hdr, ptr, false);
            Marshal.Copy(ptr, hdrAndPayload, 0, Marshal.SizeOf<JNET_PROTOCOL.MSG_HDR>());
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
        Buffer.BlockCopy(bytes, 0, hdrAndPayload, Marshal.SizeOf<JNET_PROTOCOL.MSG_HDR>(), bytes.Length);

        SendBytes(hdrAndPayload);
    }

    public void SendSimplePacket<T>(byte packetType, T payload)
    {
        JNET_PROTOCOL.SIMPLE_MSG_HDR hdr = new JNET_PROTOCOL.SIMPLE_MSG_HDR();
        hdr.Code = JNET_PROTOCOL.JNET_PROTO_CODE;
        hdr.MsgLen = (byte)Marshal.SizeOf<T>();
        hdr.MsgType = packetType;

        byte[] hdrAndPayload = new byte[Marshal.SizeOf<JNET_PROTOCOL.SIMPLE_MSG_HDR>() + Marshal.SizeOf<T>()];

        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf<JNET_PROTOCOL.MSG_HDR>());
        try
        {
            Marshal.StructureToPtr(hdr, ptr, false);
            Marshal.Copy(ptr, hdrAndPayload, 0, Marshal.SizeOf<JNET_PROTOCOL.MSG_HDR>());
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
        Buffer.BlockCopy(hdrAndPayload, 0, hdrAndPayload, Marshal.SizeOf<JNET_PROTOCOL.MSG_HDR>(), hdrAndPayload.Length);

        SendBytes(hdrAndPayload);
    }

    public void SendData<T>(T data)
    {
        byte[] bytes = MessageToBytes<T>(data);
        SendBytes(bytes);
    }

    public void SendBytes(byte[] data)
    {
        try
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "전송할 데이터가 null입니다.");
            }

            m_Stream.Write(data, 0, data.Length);
        }
        catch (IOException ex)
        {
            // 네트워크 오류로 인해 발생하는 IOException 처리
            Console.WriteLine($"Network Error : {ex.Message}");

            // [재시도 로직이나 네트워크 복구 로직]
            // ...
        }
        catch (ObjectDisposedException ex)
        {
            // 스트림이 이미 닫혔을 때 발생하는 예외 처리
            Console.WriteLine($"Stream Closed : {ex.Message}");
        }
        catch (SocketException ex)
        {
            // 소켓 관련 오류 처리
            Console.WriteLine($"Socket Error: {ex.Message}");

            // [소켓 오류에 따른 추가 처리 로직]
            // .. 
        }
        catch (Exception ex)
        {
            // 기타
            Console.WriteLine($"Exception : {ex.Message}");
        }
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
            Console.WriteLine($"Exception : {ex.Message}");
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
            Console.WriteLine($"Exception : {ex.Message}");
            Debugger.Break();
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }

        return str;
    }
}