using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stub_FightGameCrtDel : Stub_FightGameCrtDel_abstract
{
    private void Start()
    {
        base.Init();
    }

    protected override void CRT_CHARACTER(UInt32 ID, UInt16 Dir, UInt16 X, UInt16 Y, byte HP)
    {
        throw new NotImplementedException("CRT_CHARACTER");
    }
    protected override void CRT_OTHER_CHARACTER(UInt32 ID, UInt16 Dir, UInt16 X, UInt16 Y, byte HP)
    {
        throw new NotImplementedException("CRT_OTHER_CHARACTER");
    }
    protected override void DEL_CHARACTER(UInt32 ID)
    {
        throw new NotImplementedException("DEL_CHARACTER");
    }
}