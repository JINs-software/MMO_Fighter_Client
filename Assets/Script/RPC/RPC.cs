#define SIMPLE_PACKET
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class RPC : MonoBehaviour
{
    static RPC s_Instance;
    public static RPC Instance { get { Init(); return s_Instance; } }
    public static Proxy proxy = new Proxy();
    public static byte ValidCode = 137;
    public static bool EnDecodeFlag = false;
    private Dictionary<UInt16, Action<byte[]>> StubMethods = new Dictionary<UInt16, Action<byte[]>>();

    NetworkManager m_NetworkManager = new NetworkManager();
    public static NetworkManager Network { get { return Instance.m_NetworkManager; } }

    private string ServerIP;
    private UInt16 ServerPort;

    private void Start()
    {
        Init();
    }

    private static void Init()
    {
        GameObject go = GameObject.Find("@RPC");
        if (go == null)
        {
            go = new GameObject { name = "@RPC" };
            go.AddComponent<RPC>();

            DontDestroyOnLoad(go);
            s_Instance = go.GetComponent<RPC>();
        }
    }

    public bool Initiate(string serverIP, UInt16 serverPort)
    {
        ServerIP = serverIP;
        ServerPort = serverPort;   

        if (!Network.Connected)
        {
            return Network.Connect(serverIP, serverPort);
        }

        return true;
    }

    public void AttachStub(Stub stub)
    {
        foreach (var method in stub.methods)
        {
            if (StubMethods.ContainsKey(method.Key))
            {
                StubMethods.Remove(method.Key);
            }
            StubMethods.Add(method.Key, method.Value);
        }
    }

    public void DetachStub(Stub stub)
    {
        foreach(var method in stub.methods)
        {
            if (StubMethods.ContainsKey(method.Key))
            {
                StubMethods.Remove(method.Key);
            }
        }
    }

    public NetworkManager AllocNewClientSession()
    {
        NetworkManager newSession = new NetworkManager();
        if(newSession != null)
        {
            if(newSession.Connect(ServerIP, ServerPort))
            {
                return newSession;
            }
        }

        return null;
    }

    private void Update()
    {
#if SIMPLE_PACKET
        while (Network.ReceivedDataSize() >= Marshal.SizeOf<JNET_PROTOCOL.SIMPLE_MSG_HDR>())
        {
            JNET_PROTOCOL.SIMPLE_MSG_HDR hdr;
            Network.Peek<JNET_PROTOCOL.SIMPLE_MSG_HDR>(out hdr);
            if (Marshal.SizeOf<JNET_PROTOCOL.SIMPLE_MSG_HDR>() + hdr.MsgLen <= Network.ReceivedDataSize())
            {
                Network.ReceiveData<JNET_PROTOCOL.SIMPLE_MSG_HDR>(out hdr);
                byte[] payload = Network.ReceiveBytes(hdr.MsgLen);
                if (StubMethods.ContainsKey(hdr.MsgType))
                {
                    StubMethods[hdr.MsgType].Invoke(payload);
                }
            }
            else
            {
                break;
            }
        }
#else
        while (Network.ReceivedDataSize() >= Marshal.SizeOf<JNET_PROTOCOL.MSG_HDR>())
        {
            byte[] payload;
            if (Network.ReceivePacketBytes(out payload, EnDecodeFlag))
            {
                UInt16 msgType = BitConverter.ToUInt16(payload, 0);
                if (StubMethods.ContainsKey(msgType))
                {
                    StubMethods[msgType].Invoke(new ArraySegment<byte>(payload, sizeof(UInt16), payload.Length - sizeof(UInt16)).ToArray());
                }
            }
            else
            {
                break;
            }
        }
#endif
    }
}
