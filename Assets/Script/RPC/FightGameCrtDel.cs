
using System;
using UnityEngine;

public class FightGameCrtDel : Stub_FightGameCrtDel
{
    private void Start() 
    {
        base.Init();
    }

    private void OnDestroy()
    {
        base.Clear();  
    }


    protected override void CRT_CHARACTER(UInt32 ID, byte Dir, UInt16 X, UInt16 Y, byte HP) 
    {
        BattleField battleField = gameObject.GetComponent<BattleField>();
        Fighter fighter = battleField.CreateFighter(ID, Dir, X, Y, HP);
        if (fighter.gameObject.GetComponent<FighterController>() == null)
        {
            fighter.gameObject.AddComponent<FighterController>();
        }

        GameObject mainCamera = GameObject.Find("Main Camera");
        mainCamera.transform.parent = fighter.gameObject.transform;
        mainCamera.transform.localPosition = new Vector3(0, 0, -10);

        battleField.Player = fighter;
        battleField.Fighters.Add(ID, fighter);
    }

    protected override void CRT_OTHER_CHARACTER(UInt32 ID, byte Dir, UInt16 X, UInt16 Y, byte HP) 
    {
        BattleField battleField = gameObject.GetComponent<BattleField>();
        Fighter fighter = battleField.CreateFighter(ID, Dir, X, Y, HP);
        battleField.Fighters.Add(ID, fighter);
    }

    protected override void DEL_CHARACTER(UInt32 ID) 
    {
        BattleField battleField = gameObject.GetComponent<BattleField>();
        if (battleField.Fighters.ContainsKey(ID))
        {
            if (battleField.Fighters[ID] == battleField.Player)
            {
                // 에디터에서는 이 코드로 종료
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
    // 빌드된 게임에서는 이 코드로 종료
    Application.Quit();
#endif
            }
            else
            {
                GameObject.Destroy(battleField.Fighters[ID].gameObject);
                battleField.Fighters.Remove(ID);
            }
        }
    }
}
