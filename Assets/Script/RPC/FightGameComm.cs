
using System;

public class FightGameComm : Stub_FightGameComm
{
    private void Start() 
    {
        base.Init();
    }

    private void OnDestroy()
    {
        base.Clear();  
    }


    protected override void SYNC(UInt32 ID, UInt16 X, UInt16 Y) 
    {
        BattleField battleField = gameObject.GetComponent<BattleField>();
        if (battleField.Fighters.ContainsKey(ID))
        {
            Fighter fighter = battleField.Fighters[ID];
            fighter.X = X;
            fighter.Y = Y;
        }
    }

    protected override void ECHO(UInt32 Time) 
    {
        //throw new NotImplementedException("ECHO");
    }

}
