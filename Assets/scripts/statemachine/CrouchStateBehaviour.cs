using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchStateBehaviour : StateComponent {


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        movementComponent.playerState = CharacterManager.PlayerState.crouch;
        movementComponent.ModifyControllerSize(1f, .66f);
    }

	override public void EvaluateState(Animator animator) {
		if (!entityInput.ActivateCrouch()) {
			animator.SetBool ("crouch", false);
            movementComponent.ResetControllerSize();
            movementComponent.playerState = CharacterManager.PlayerState.idle;
        }
	}
}
