using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    private Animator animator;
    public Direction direction;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        direction = Direction.NONE;
    }

    public void MOVE_START(Direction moveDir)
    {
        animator.SetBool("bMove", true);
        direction = moveDir;    
        if(moveDir == Direction.DIR_LL || moveDir == Direction.DIR_LD || moveDir == Direction.DIR_LU)
        {
            Vector3 currentScale = transform.localScale;
            currentScale.x = -Mathf.Abs(currentScale.x);
            transform.localScale = currentScale;

        }
        else if(moveDir == Direction.DIR_RR || moveDir == Direction.DIR_RD || moveDir == Direction.DIR_RU)
        {
            Vector3 currentScale = transform.localScale;
            currentScale.x = Mathf.Abs(currentScale.x);
            transform.localScale = currentScale;
        }
    }
    public void MOVE_STOP()
    {
        animator.SetBool("bMove", false);
    }
    public void ATTACK1()
    {
        animator.SetTrigger("trAttack1");
    }
    public void ATTACK2()
    {
        animator.SetTrigger("trAttack2");
    }
    public void ATTACK3()
    {
        animator.SetTrigger("trAttack3");
    }
}
