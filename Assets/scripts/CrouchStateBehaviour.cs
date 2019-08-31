using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchStateBehaviour : StateMachineBehaviour {

    private Basic2DMovement movementComponent;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (movementComponent == null)
        {
            GameObject owner = animator.transform.parent.gameObject;
            movementComponent = owner.GetComponent<Basic2DMovement>();
        }
        movementComponent.playerState = Basic2DMovement.PlayerState.crouch;
        movementComponent.ModifyControllerSize(1.2f, .66f);
    }

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (!Input.GetButton ("Crouch")) {
			animator.SetBool ("crouch", false);
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        movementComponent.ResetControllerSize();
        movementComponent.playerState = Basic2DMovement.PlayerState.idle;
	}
}
