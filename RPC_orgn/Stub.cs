using System;
using System.Collections.Generic;
using UnityEngine;


public class Stub : MonoBehaviour
{
    public Dictionary<int, Action<byte[]>> methods = new Dictionary<int, Action<byte[]>>();

    protected Dictionary<string, byte> MessageIDs = new Dictionary<string, byte>()
    {
        {"CRT_CHARACTER", 0},
        {"CRT_OTHER_CHARACTER", 1},
        {"DEL_CHARACTER", 2},
        {"MOVE_START", 11},
        {"MOVE_STOP", 13},
        {"ATTACK1", 21},
        {"ATTACK2", 23},
        {"ATTACK3", 25},
        {"DAMAGE", 30},
        {"SYNC", 251},
        {"ECHO", 253},
    };
}

public abstract class Stub_FightGameCrtDel : Stub
{
    public void Init()
    {
        methods.Add(MessageIDs["CRT_CHARACTER"], CRT_CHARACTER);
        methods.Add(MessageIDs["CRT_OTHER_CHARACTER"], CRT_OTHER_CHARACTER);
        methods.Add(MessageIDs["DEL_CHARACTER"], DEL_CHARACTER);

        RPC.Instance.AttachStub(this);
    }
    public void CRT_CHARACTER(byte[] payload)
    {
        int offset = 0;
        UInt32 ID = BitConverter.ToUInt32(payload, offset); offset += sizeof(UInt32);
        byte Dir = payload[offset++]; 
        UInt16 X = BitConverter.ToUInt16(payload, offset); offset += sizeof(UInt16);
        UInt16 Y = BitConverter.ToUInt16(payload, offset); offset += sizeof(UInt16);
        byte HP = payload[offset++];
        CRT_CHARACTER(ID, Dir, X, Y, HP);
    }
    public void CRT_OTHER_CHARACTER(byte[] payload)
    {
        int offset = 0;
        UInt32 ID = BitConverter.ToUInt32(payload, offset); offset += sizeof(UInt32);
        byte Dir = payload[offset++]; 
        UInt16 X = BitConverter.ToUInt16(payload, offset); offset += sizeof(UInt16);
        UInt16 Y = BitConverter.ToUInt16(payload, offset); offset += sizeof(UInt16);
        byte HP = payload[offset++];
        CRT_OTHER_CHARACTER(ID, Dir, X, Y, HP);
    }
    public void DEL_CHARACTER(byte[] payload)
    {
        int offset = 0;
        UInt32 ID = BitConverter.ToUInt32(payload, offset); offset += sizeof(UInt32);
        DEL_CHARACTER(ID);
    }

    protected abstract void CRT_CHARACTER(UInt32 ID, byte Dir, UInt16 X, UInt16 Y, byte HP);
    protected abstract void CRT_OTHER_CHARACTER(UInt32 ID, byte Dir, UInt16 X, UInt16 Y, byte HP);
    protected abstract void DEL_CHARACTER(UInt32 ID);
}

public abstract class Stub_FightGameMove : Stub
{
    public void Init()
    {
        methods.Add(MessageIDs["MOVE_START"], MOVE_START);
        methods.Add(MessageIDs["MOVE_STOP"], MOVE_STOP);

        RPC.Instance.AttachStub(this);
    }

    public void MOVE_START(byte[] payload)
    {
        int offset = 0;
        UInt32 ID = BitConverter.ToUInt32(payload, offset); offset += sizeof(UInt32);
        byte Dir = payload[offset++];
        UInt16 X = BitConverter.ToUInt16(payload, offset); offset += sizeof(UInt16);
        UInt16 Y = BitConverter.ToUInt16(payload, offset); offset += sizeof(UInt16);
        MOVE_START(ID, Dir, X, Y);
    }
    public void MOVE_STOP(byte[] payload)
    {
        int offset = 0;
        UInt32 ID = BitConverter.ToUInt32(payload, offset); offset += sizeof(UInt32);
        byte Dir = payload[offset++];
        UInt16 X = BitConverter.ToUInt16(payload, offset); offset += sizeof(UInt16);
        UInt16 Y = BitConverter.ToUInt16(payload, offset); offset += sizeof(UInt16);
        MOVE_STOP(ID, Dir, X, Y);
    }

    protected abstract void MOVE_START(UInt32 ID, byte Dir, UInt16 X, UInt16 Y);
    protected abstract void MOVE_STOP(UInt32 ID, byte Dir, UInt16 X, UInt16 Y);
}

public abstract class Stub_FightGameAttack : Stub
{
    public void Init()
    {
        methods.Add(MessageIDs["ATTACK1"], ATTACK1);
        methods.Add(MessageIDs["ATTACK2"], ATTACK2);
        methods.Add(MessageIDs["ATTACK3"], ATTACK3);

        RPC.Instance.AttachStub(this);
    }

    public void ATTACK1(byte[] payload)
    {
        int offset = 0;
        UInt32 ID = BitConverter.ToUInt32(payload, offset); offset += sizeof(UInt32);
        byte Dir = payload[offset++];
        UInt16 X = BitConverter.ToUInt16(payload, offset); offset += sizeof(UInt16);
        UInt16 Y = BitConverter.ToUInt16(payload, offset); offset += sizeof(UInt16);
        ATTACK1(ID, Dir, X, Y);
    }
    public void ATTACK2(byte[] payload)
    {
        int offset = 0;
        UInt32 ID = BitConverter.ToUInt32(payload, offset); offset += sizeof(UInt32);
        byte Dir = payload[offset++];
        UInt16 X = BitConverter.ToUInt16(payload, offset); offset += sizeof(UInt16);
        UInt16 Y = BitConverter.ToUInt16(payload, offset); offset += sizeof(UInt16);
        ATTACK2(ID, Dir, X, Y);
    }

    public void ATTACK3(byte[] payload)
    {
        int offset = 0;
        UInt32 ID = BitConverter.ToUInt32(payload, offset); offset += sizeof(UInt32);
        byte Dir = payload[offset++];
        UInt16 X = BitConverter.ToUInt16(payload, offset); offset += sizeof(UInt16);
        UInt16 Y = BitConverter.ToUInt16(payload, offset); offset += sizeof(UInt16);
        ATTACK3(ID, Dir, X, Y);
    }

    protected abstract void ATTACK1(UInt32 ID, byte Dir, UInt16 X, UInt16 Y);
    protected abstract void ATTACK2(UInt32 ID, byte Dir, UInt16 X, UInt16 Y);
    protected abstract void ATTACK3(UInt32 ID, byte Dir, UInt16 X, UInt16 Y);
}

public abstract class Stub_FightGameDamage : Stub
{
    public void Init()
    {
        methods.Add(MessageIDs["DAMAGE"], DAMAGE);

        RPC.Instance.AttachStub(this);
    }

    public void DAMAGE(byte[] payload)
    {
        int offset = 0;
        UInt32 Attacker = BitConverter.ToUInt32(payload, offset); offset += sizeof(UInt32);
        UInt32 Target = BitConverter.ToUInt32(payload, offset); offset += sizeof(UInt32);
        byte TargetHP = payload[offset++];
        DAMAGE(Attacker, Target, TargetHP);
    }

    protected abstract void DAMAGE(uint attacker, uint target, byte targetHP);
}

public abstract class Stub_FightGameComm : Stub
{
    public void Init()
    {
        methods.Add(MessageIDs["SYNC"], SYNC);
        methods.Add(MessageIDs["ECHO"], ECHO);

        RPC.Instance.AttachStub(this);
    }

    public void SYNC(byte[] payload)
    {
        int offset = 0;
        UInt32 ID = BitConverter.ToUInt32(payload, offset); offset += sizeof(UInt32);
        UInt16 X = BitConverter.ToUInt16(payload, offset); offset += sizeof(UInt16);
        UInt16 Y = BitConverter.ToUInt16(payload, offset); offset += sizeof(UInt16);
        SYNC(ID, X, Y);
    }
    public void ECHO(byte[] payload)
    {
        int offset = 0;
        UInt32 Time = BitConverter.ToUInt32(payload, offset);
        ECHO(Time);
    }

    protected abstract void SYNC(UInt32 ID, UInt16 X, UInt16 Y);
    protected abstract void ECHO(UInt32 Time);  
}