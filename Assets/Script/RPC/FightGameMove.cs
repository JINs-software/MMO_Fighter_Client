using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightGameMove : Stub_FightGameMove
{
    private void Start()
    {
        base.Init();
    }

    protected override void MOVE_START(uint ID, byte Dir, ushort X, ushort Y)
    {
        BattleField battleField = gameObject.GetComponent<BattleField>();
        if (battleField.Fighters.ContainsKey(ID))
        {
            Fighter fighter = battleField.Fighters[ID];
            if(fighter != battleField.Player)
            {
                fighter.X = X;
                fighter.Y = Y;
                fighter.MOVE_START(Dir);
            }
        }
    }

    protected override void MOVE_STOP(uint ID, byte Dir, ushort X, ushort Y)
    {
        BattleField battleField = gameObject.GetComponent<BattleField>();
        if (battleField.Fighters.ContainsKey(ID))
        {
            Fighter fighter = battleField.Fighters[ID];
            if (fighter != battleField.Player)
            {
                fighter.X = X;
                fighter.Y = Y;
                fighter.direction = (Direction)Dir;
                fighter.MOVE_STOP();
            }
        }
    }
}
