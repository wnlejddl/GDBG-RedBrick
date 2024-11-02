using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpToIdleBehaviour : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Attack 상태에서 나갈 때 호출되는 함수
        if (stateInfo.IsName("penguin_jump"))
        {
            // Idle 상태로 전환될 때 실행할 함수를 호출
            animator.GetComponent<PlayerController>().OnJumpEnd();
        }
    }

}
