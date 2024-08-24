using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
struct stMSG_HDR
{
    public byte byCode;
    public byte bySize;
    public byte byType;
}

public class RPC : MonoBehaviour
{
    static RPC s_Instance;
    public static RPC Instance { get { Init(); return s_Instance; } }
    
    private Dictionary<int, Action<object[]>> StubMethods = new Dictionary<int, Action<object[]>>();

    private void Start()
    {
        Init();

        // 임시
        Manager.Network.Connect("127.0.0.1", 12121);
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

    public void AttachStub(Stub stub)
    {
        foreach (var method in stub.methods)
        {
            StubMethods.Add(method.Key, method.Value);
        }
    }

    private void Update()
    {
        while (Manager.Network.ReceiveDataAvailable())
        {
            stMSG_HDR hdr;
            if(Manager.Network.Peek<stMSG_HDR>(out hdr))
            {
                if(Marshal.SizeOf<stMSG_HDR>() + hdr.bySize <= Manager.Network.ReceivedDataSize())
                {
                    Manager.Network.ReceivePacket<stMSG_HDR>(out hdr);
                    byte[] payload = Manager.Network.ReceivePacket(hdr.bySize);

                    System.Reflection.MethodInfo methodInfo = StubMethods[hdr.byType].Method;
                    System.Reflection.ParameterInfo[] paramInfos = methodInfo.GetParameters();
                    object[] arguments = new object[paramInfos.Length];
                    int offset = 0;
                    for (int i = 0; i < paramInfos.Length; i++)
                    {
                        System.Reflection.ParameterInfo param = paramInfos[i];
                        Type type = param.ParameterType;
                        if (type == typeof(byte))
                        {
                            arguments[i] = payload[offset];
                        }
                        else if (type == typeof(short))
                        {
                            arguments[i] = BitConverter.ToInt16(payload, offset);
                            offset += 2;
                        }
                        else if (type == typeof(ushort))
                        {
                            arguments[i] = BitConverter.ToUInt16(payload, offset);
                            offset += 2;
                        }
                        else if (type == typeof(int))
                        {
                            arguments[i] = BitConverter.ToInt32(payload, offset);
                            offset += 4;
                        }
                        else if (type == typeof(uint))
                        {
                            arguments[i] = BitConverter.ToUInt32(payload, offset);
                            offset += 4;
                        }
                        else
                        {
                            // 현재 디버깅 중인 상태에서 코드가 중단되고,
                            // 디버거가 활성화되지 않았다면 디버거를 시작할 수 있는 창이 뜸
                            Debugger.Break();
                        }
                    }
                    StubMethods[hdr.byType].Invoke(arguments);
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }
    }
}
