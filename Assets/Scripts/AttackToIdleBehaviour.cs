using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackToIdleBehaviour : StateMachineBehaviour
{
// 상태에서 나갈 때 호출
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Attack 상태에서 나갈 때 호출되는 함수
        if (stateInfo.IsName("penguin_atack"))
        {
            // Idle 상태로 전환될 때 실행할 함수를 호출
            animator.GetComponent<PlayerController>().OnAttackEnd();
        }
    }
}
