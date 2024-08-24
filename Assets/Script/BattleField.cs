using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleField : MonoBehaviour
{
    public Dictionary<UInt32, Fighter> Fighters = new Dictionary<uint, Fighter>();
    public Fighter Player = null;

    // Start is called before the first frame update
    void Start()
    {
        // FPS ∞Ì¡§
        Application.targetFrameRate = 50;

        RPC.Instance.Initiate(GameServer.IP, GameServer.Port);

        if(GetComponent<FightGameCrtDel>() == null)
        {
            gameObject.AddComponent<FightGameCrtDel>(); 
        }
        if (GetComponent<FightGameMove>() == null)
        {
            gameObject.AddComponent<FightGameMove>();
        }
        if (GetComponent<FightGameAttack>() == null)
        {
            gameObject.AddComponent<FightGameAttack>();
        }
        if (GetComponent<FightGameDamage>() == null)
        {
            gameObject.AddComponent<FightGameDamage>();
        }
        if (GetComponent<FightGameComm>() == null)
        {
            gameObject.AddComponent<FightGameComm>();
        }
    }

    public Fighter CreateFighter(UInt32 ID, byte Dir, UInt16 X, UInt16 Y, byte HP)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Fighter");
        //GameObject fighter = Instantiate(prefab, new Vector3(X, Y, 0), Quaternion.identity);
        GameObject fighterObj = Instantiate(prefab);
        Fighter fighter = fighterObj.GetComponent<Fighter>();  
        fighter.direction = (Direction)Dir;
        fighter.X = X;
        fighter.Y = Y;
        fighter.HP_orgn = HP;
        fighter.HP_now = HP;
        return fighter.GetComponent<Fighter>(); 
    }
}
