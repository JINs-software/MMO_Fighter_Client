using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightGameComm : Stub_FightGameComm
{
    private void Start()
    {
        base.Init();    
    }

    protected override void ECHO(uint Time)
    {
        //throw new System.NotImplementedException();
    }

    protected override void SYNC(uint ID, ushort X, ushort Y)
    {
        //throw new System.NotImplementedException();
        BattleField battleField = gameObject.GetComponent<BattleField>();
        if (battleField.Fighters.ContainsKey(ID))
        {
            Fighter fighter = battleField.Fighters[ID];
            Debug.Log("client position: " + new Vector3(fighter.X, fighter.Y, 0) + ", server position" + new Vector3(X, Y, 0));
            //fighter.gameObject.transform.position = new Vector3(X, Y, 0);
            fighter.X = X;
            fighter.Y = Y;  
        }
    }
}
