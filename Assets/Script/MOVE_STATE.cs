using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class MOVE_STATE : StateMachineBehaviour
{
    Fighter fighter;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        fighter = animator.gameObject.GetComponent<Fighter>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float deltaTime = Time.deltaTime;

        // ������ �̵�
        //animator.gameObject.transform.position = Vector2(animator.gameObject.transform.position.x * speed * deltaTime, .., ..);
        if(fighter.direction == Direction.DIR_UU || fighter.direction == Direction.DIR_LU || fighter.direction == Direction.DIR_RU)
        {
            Vector3 currentPosition = fighter.gameObject.transform.position; // ���� ��ġ ��������
            currentPosition.y += (Time.deltaTime / Delta.DELTA_TIME_PER_FRAME) * Delta.DELTA_Y;
            fighter.gameObject.transform.position = currentPosition; // ���ο� ��ġ ����
        }
        if(fighter.direction == Direction.DIR_DD || fighter.direction == Direction.DIR_LD || fighter.direction == Direction.DIR_RD)
        {
            Vector3 currentPosition = fighter.gameObject.transform.position; // ���� ��ġ ��������
            currentPosition.y -= (Time.deltaTime / Delta.DELTA_TIME_PER_FRAME) * Delta.DELTA_Y;
            fighter.gameObject.transform.position = currentPosition; // ���ο� ��ġ ����
        }
        if (fighter.direction == Direction.DIR_LL || fighter.direction == Direction.DIR_LU || fighter.direction == Direction.DIR_LD)
        {
            Vector3 currentPosition = fighter.gameObject.transform.position; // ���� ��ġ ��������
            currentPosition.x -= (Time.deltaTime / Delta.DELTA_TIME_PER_FRAME) * Delta.DELTA_X;
            fighter.gameObject.transform.position = currentPosition; // ���ο� ��ġ ����
        }
        if (fighter.direction == Direction.DIR_RR || fighter.direction == Direction.DIR_RU || fighter.direction == Direction.DIR_RD)
        {
            Vector3 currentPosition = fighter.gameObject.transform.position; // ���� ��ġ ��������
            currentPosition.x += (Time.deltaTime / Delta.DELTA_TIME_PER_FRAME) * Delta.DELTA_X;
            fighter.gameObject.transform.position = currentPosition; // ���ο� ��ġ ����
        }
    }
}
