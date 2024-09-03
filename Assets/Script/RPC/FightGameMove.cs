
using System;

public class FightGameMove : Stub_FightGameMove
{
    private void Start() 
    {
        base.Init();
    }

    private void OnDestroy()
    {
        base.Clear();  
    }


    protected override void MOVE_START(UInt32 ID, byte Dir, UInt16 X, UInt16 Y) 
    {
        BattleField battleField = gameObject.GetComponent<BattleField>();
        if (battleField.Fighters.ContainsKey(ID))
        {
            Fighter fighter = battleField.Fighters[ID];
            if (fighter != battleField.Player)
            {
                fighter.X = X;
                fighter.Y = Y;
                fighter.MOVE_START(Dir);
            }
        }
    }

    protected override void MOVE_STOP(UInt32 ID, byte Dir, UInt16 X, UInt16 Y) 
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
