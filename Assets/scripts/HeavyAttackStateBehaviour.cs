using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttackStateBehaviour : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        GameObject owner = animator.transform.parent.gameObject;
        GameManager.SetAtacking(owner,true);
        GameManager.SET_PLAYER_STATE(owner, Basic2DMovement.PlayerState.heavyattack);
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        GameObject owner = animator.transform.parent.gameObject;
        GameManager.SetAtacking(owner, false);
		GameManager.SET_PLAYER_STATE (owner, Basic2DMovement.PlayerState.idle);
	}
		
}
