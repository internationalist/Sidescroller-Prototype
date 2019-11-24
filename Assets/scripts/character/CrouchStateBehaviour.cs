using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchStateBehaviour : StateMachineBehaviour {

    private Basic2DMovement movementComponent;
    private EntityInputAbstract entityInput;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (movementComponent == null)
        {
            GameObject owner = animator.transform.parent.gameObject;
            movementComponent = owner.GetComponent<Basic2DMovement>();
            entityInput = movementComponent.Input;
        }
        movementComponent.playerState = Basic2DMovement.PlayerState.crouch;
        movementComponent.ModifyControllerSize(1f, .66f);
    }

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (!entityInput.ActivateCrouch()) {
			animator.SetBool ("crouch", false);
            movementComponent.ResetControllerSize();
            movementComponent.playerState = Basic2DMovement.PlayerState.idle;
        }
	}
}
