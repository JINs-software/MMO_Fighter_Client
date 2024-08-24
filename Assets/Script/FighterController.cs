using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterController : MonoBehaviour
{
    Fighter fighter;
    bool KeyUp = false;
    bool KeyDown = false;
    bool KeyLeft = false;
    bool KeyRight = false;

    void Awake()
    {
        fighter = gameObject.GetComponent<Fighter>();  
    }

    // Update is called once per frame
    void Update()
    {
        bool press = false;
        bool release = false;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            // Attack1
            // 이동 중지 패킷 송신
            RPC.proxy.MOVE_STOP((byte)fighter.direction, fighter.X, fighter.Y);
            // 공격 패킷 송신
            RPC.proxy.ATTACK1((byte)fighter.direction, fighter.X, fighter.Y);
            gameObject.GetComponent<Fighter>().ATTACK1();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            // Attack2
            // 이동 중지 패킷 송신
            RPC.proxy.MOVE_STOP((byte)fighter.direction, fighter.X, fighter.Y);
            // 공격 패킷 송신
            RPC.proxy.ATTACK2((byte)fighter.direction, fighter.X, fighter.Y);
            gameObject.GetComponent<Fighter>().ATTACK2();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Attack3
            // 이동 중지 패킷 송신
            RPC.proxy.MOVE_STOP((byte)fighter.direction, fighter.X, fighter.Y);
            // 공격 패킷 송신
            RPC.proxy.ATTACK3((byte)fighter.direction, fighter.X, fighter.Y);
            gameObject.GetComponent<Fighter>().ATTACK3();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // 상 이동
            KeyUp = true;
            press = true;   
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            KeyUp = false;
            release = true;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // 하 이동
            KeyDown = true;
            press = true;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            KeyDown= false;
            release = true;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // 좌 이동
            KeyLeft = true;
            press = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            KeyLeft = false;
            release = true;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // 우 이동
            KeyRight = true;
            press = true;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            KeyRight= false;
            release = true;
        }

        if (press || release)
        {
            if(!KeyUp && !KeyDown && !KeyLeft && !KeyRight)
            {
                // MOVE_STOP 송신
                Debug.Log("[MOVE_STOP] X: " + fighter.X + ", Y: " + fighter.Y);
                RPC.proxy.MOVE_STOP((byte)fighter.direction, fighter.X, fighter.Y);
                fighter.MOVE_STOP();
                return;
            }

            Direction direction = Direction.NONE;
            if(KeyUp && KeyLeft)
            {
                Debug.Log("[Move Key] DIR_LU");
                direction = Direction.DIR_LU;
            }
            else if(KeyUp && KeyRight)
            {
                Debug.Log("[Move Key] DIR_RU");
                direction = Direction.DIR_RU;
            }
            else if(KeyDown && KeyLeft)
            {
                Debug.Log("[Move Key] DIR_LD");
                direction = Direction.DIR_LD;
            }
            else if(KeyDown && KeyRight)
            {
                Debug.Log("[Move Key] DIR_RD");
                direction = Direction.DIR_RD;
            }
            else if (KeyUp)
            {
                Debug.Log("[Move Key] DIR_UU");
                direction = Direction.DIR_UU;
            }
            else if(KeyDown)
            {
                Debug.Log("[Move Key] DIR_DD");
                direction = Direction.DIR_DD;
            }
            else if (KeyLeft)
            {
                Debug.Log("[Move Key] DIR_LL");
                direction = Direction.DIR_LL;
            }
            else if (KeyRight)
            {
                Debug.Log("[Move Key] DIR_RR");
                direction = Direction.DIR_RR;
            }

            // MOVE_START 송신
            Debug.Log("[MOVE_START] X: " + fighter.X + ", Y: " + fighter.Y);
            RPC.proxy.MOVE_START((byte)direction, fighter.X, fighter.Y);
            fighter.MOVE_START(direction);
        }
    }
}
