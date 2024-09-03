
using System;

public class FightGameDamage : Stub_FightGameDamage
{
    private void Start() 
    {
        base.Init();
    }

    private void OnDestroy()
    {
        base.Clear();  
    }


    protected override void DAMAGE(UInt32 attker, UInt32 target, byte targetHP) 
    {
        BattleField battleField = gameObject.GetComponent<BattleField>();
        if (battleField.Fighters.ContainsKey(target))
        {
            Fighter fighter = battleField.Fighters[target];
            fighter.TAKE_DAMAGE(targetHP);
        }
    }

}
