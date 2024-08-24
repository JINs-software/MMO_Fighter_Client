using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
struct stMSG_HDR
{
    public byte Code;
    public byte MsgLen;
    public byte MsgType;
}

public class RPC : MonoBehaviour
{
    static RPC s_Instance;
    public static RPC Instance { get { Init(); return s_Instance; } }
    public static Proxy proxy = new Proxy();
    public static byte ValidCode = 136;

    private Dictionary<int, Action<byte[]>> StubMethods = new Dictionary<int, Action<byte[]>>();

    NetworkManager m_NetworkManager = new NetworkManager();
    public static NetworkManager Network { get { return Instance.m_NetworkManager; } }

    private void Start()
    {
        Init();
    }

    private static void Init()
    {
        GameObject go = GameObject.Find("@RPC");
        if(go == null)
        {
            go = new GameObject { name = "@RPC" };
            go.AddComponent<RPC>();

            DontDestroyOnLoad(go);
            s_Instance = go.GetComponent<RPC>();
        }
    }

    public bool Initiate(string serverIP, UInt16 serverPort)
    {
        return Network.Connect(serverIP, serverPort);
    }

    public void AttachStub(Stub stub)
    {
        foreach (var method in stub.methods)
        {
            StubMethods.Add(method.Key, method.Value);
        }
    }

    private void Update()
    {
        while(Network.ReceivedDataSize() >= Marshal.SizeOf<stMSG_HDR>())
        {
            stMSG_HDR hdr;
            Network.Peek<stMSG_HDR>(out hdr);
            if (Marshal.SizeOf<stMSG_HDR>() + hdr.MsgLen <= Network.ReceivedDataSize())
            {
                Network.ReceiveData<stMSG_HDR>(out hdr);
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
    }
}
