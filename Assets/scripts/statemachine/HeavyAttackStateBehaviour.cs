using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttackStateBehaviour : StateComponent {


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        movementComponent.IsAttacking = true;
        movementComponent.playerState = CharacterManager.PlayerState.heavyattack;
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        GameManager.S.actionRegister = 0;
        movementComponent.IsAttacking = false;
		movementComponent.playerState = CharacterManager.PlayerState.idle;
    }
		
}
