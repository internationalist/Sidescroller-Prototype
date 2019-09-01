using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttackStateBehaviour : StateMachineBehaviour {

    private Basic2DMovement movementComponent;

     // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(movementComponent == null)
        {
            GameObject owner = animator.transform.parent.gameObject;
            movementComponent = owner.GetComponent<Basic2DMovement>();
        }
        movementComponent.IsAttacking = true;
        movementComponent.playerState = Basic2DMovement.PlayerState.heavyattack;
        animator.SetBool("light2heavy", false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Input.GetMouseButtonDown(0))
        {
            movementComponent.playerState = Basic2DMovement.PlayerState.lightattack;
            animator.SetBool("heavy2light", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        movementComponent.IsAttacking = false;
		movementComponent.playerState = Basic2DMovement.PlayerState.idle;
	}
		
}
