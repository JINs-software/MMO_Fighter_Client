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

    // Start is called before the first frame update
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
            // 공격 패킷 송신
            gameObject.GetComponent<Fighter>().ATTACK1();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            // Attack2
            // 이동 중지 패킷 송신
            // 공격 패킷 송신
            gameObject.GetComponent<Fighter>().ATTACK2();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Attack3
            // 이동 중지 패킷 송신
            // 공격 패킷 송신
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
                fighter.MOVE_STOP();
                return;
            }

            Direction direction = Direction.NONE;
            if(KeyUp && KeyLeft)
            {
                direction = Direction.DIR_LU;
            }
            else if(KeyUp && KeyRight)
            {
                direction = Direction.DIR_RU;
            }
            else if(KeyDown && KeyLeft)
            {
                direction = Direction.DIR_LD;
            }
            else if(KeyDown && KeyRight)
            {
                direction = Direction.DIR_RD;
            }
            else if (KeyUp)
            {
                direction = Direction.DIR_UU;
            }
            else if(KeyDown)
            {
                direction = Direction.DIR_DD;
            }
            else if (KeyLeft)
            {
                direction = Direction.DIR_LL;
            }
            else if (KeyRight)
            {
                direction = Direction.DIR_RR;
            }

            // MOVE_START 송신
            fighter.MOVE_START(direction);
        }
    }
}
