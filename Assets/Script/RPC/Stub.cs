using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Stub : MonoBehaviour
{
    public Dictionary<int, Action<object[]>> methods = new Dictionary<int, Action<object[]>>();
}

public abstract class Stub_FightGameCrtDel_abstract : Stub
{
    public void Init()
    {
        methods.Add(0, CRT_CHARACTER);
        methods.Add(1, CRT_CHARACTER);
        methods.Add(2, CRT_CHARACTER);

        RPC.Instance.AttachStub(this);
    }

    public void CRT_CHARACTER(object[] parameters)
    {
        UInt32 ID = (UInt32)parameters[0];
        UInt16 Dir = (UInt16)parameters[1];
        UInt16 X = (UInt16)parameters[2];
        UInt16 Y = (UInt16)parameters[3];
        byte HP = (byte)parameters[4];
        CRT_CHARACTER(ID, Dir, X, Y, HP);
    }
    public void CRT_OTHER_CHARACTER(object[] parameters)
    {
        UInt32 ID = (UInt32)parameters[0];
        UInt16 Dir = (UInt16)parameters[1];
        UInt16 X = (UInt16)parameters[2];
        UInt16 Y = (UInt16)parameters[3];
        byte HP = (byte)parameters[4];
        CRT_OTHER_CHARACTER(ID, Dir, X, Y, HP);
    }
    public void DEL_CHARACTER(object[] parameters)
    {
        UInt32 ID = (UInt32)parameters[0];
        DEL_CHARACTER(ID);
    }

    protected abstract void CRT_CHARACTER(UInt32 ID, UInt16 Dir, UInt16 X, UInt16 Y, byte HP);
    protected abstract void CRT_OTHER_CHARACTER(UInt32 ID, UInt16 Dir, UInt16 X, UInt16 Y, byte HP);
    protected abstract void DEL_CHARACTER(UInt32 ID);
}