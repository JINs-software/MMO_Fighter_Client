
using System;

public class FightGameAttack : Stub_FightGameAttack
{
    private void Start() 
    {
        base.Init();
    }

    private void OnDestroy()
    {
        base.Clear();  
    }


    protected override void ATTACK1(UInt32 ID, byte Dir, UInt16 X, UInt16 Y) 
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
                fighter.ATTACK1();
            }
        }
    }

    protected override void ATTACK2(UInt32 ID, byte Dir, UInt16 X, UInt16 Y) 
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
                fighter.ATTACK2();
            }
        }
    }

    protected override void ATTACK3(UInt32 ID, byte Dir, UInt16 X, UInt16 Y) 
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
                fighter.ATTACK3();
            }
        }
    }

}
