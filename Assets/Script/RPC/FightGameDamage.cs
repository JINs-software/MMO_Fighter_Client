
using System;

public class FightGameDamage : Stub_FightGameDamage
{
    private void Start() 
    {
        base.Init();
    }


    protected override void DAMAGE(UInt32 attker, UInt32 target, Byte targetHP) 
    {
        BattleField battleField = gameObject.GetComponent<BattleField>();
        if (battleField.Fighters.ContainsKey(target))
        {
            Fighter fighter = battleField.Fighters[target];
            fighter.TAKE_DAMAGE(targetHP);
        }
    }

}
