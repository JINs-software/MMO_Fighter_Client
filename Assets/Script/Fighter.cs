using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fighter : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;  

    private Direction m_direction;
    public Direction direction
    {
        get
        {
            return m_direction;
        }
        set
        {
            m_direction = value;
            if (m_direction == Direction.DIR_LL || m_direction == Direction.DIR_LD || m_direction == Direction.DIR_LU)
            {
                Vector3 currentScale = transform.localScale;
                currentScale.x = -Mathf.Abs(currentScale.x);
                transform.localScale = currentScale;

            }
            else if (m_direction == Direction.DIR_RR || m_direction == Direction.DIR_RD || m_direction == Direction.DIR_RU)
            {
                Vector3 currentScale = transform.localScale;
                currentScale.x = Mathf.Abs(currentScale.x);
                transform.localScale = currentScale;
            }
        }
    }

    public UInt16 X
    {
        get { return (UInt16)gameObject.transform.position.x; }
        set {
            Vector3 currentPos = gameObject.transform.position;
            gameObject.transform.position = new Vector3((float)value, currentPos.y, 0); 
        }
    }
    public UInt16 Y
    {
        get { return (UInt16)(BattleMap.RANGE_Y - gameObject.transform.position.y); }
        set
        {
            Vector3 currentPos = gameObject.transform.position;
            gameObject.transform.position = new Vector3(currentPos.x, BattleMap.RANGE_Y - value, 0);
        }
    }

    public Image HPBar;
    public byte HP_orgn;
    public byte HP_now;

    Color Color_orgn;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();    
        Color_orgn = spriteRenderer.color;
    }

    public void MOVE_START(Direction moveDir)
    {
        animator.SetBool("bMove", true);
        direction = moveDir;    
    }
    public void MOVE_START(byte moveDir)
    {
        animator.SetBool("bMove", true);
        direction = (Direction)moveDir;
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
    public void TAKE_DAMAGE(byte HP)
    {
        HP_now = HP;
        HPBar.fillAmount = (float)HP_now / HP_orgn;
        StartCoroutine(FlashRed());
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color_orgn;
    }
}
