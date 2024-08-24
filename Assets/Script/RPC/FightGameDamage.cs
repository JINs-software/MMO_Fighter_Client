using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightGameDamage : Stub_FightGameDamage
{
    private void Start()
    {
        base.Init();
    }

    protected override void DAMAGE(uint attacker, uint target, byte targetHP)
    {
        BattleField battleField = gameObject.GetComponent<BattleField>();
        if (battleField.Fighters.ContainsKey(target))
        {
            Fighter fighter = battleField.Fighters[target];
            fighter.TAKE_DAMAGE(targetHP);
        }
    }
}
