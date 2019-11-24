using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStateBehaviour : StateMachineBehaviour {

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

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (entityInput.ActivateDash())
        {
            animator.SetBool("block", false);
            animator.SetBool("dash", true);
        }
        else if (entityInput.ActivateLightAttack()) {
            animator.SetBool("block", false);
            animator.SetTrigger("punch");
        }
        else if (entityInput.ActivateHeavyAttack())
        {
            animator.SetBool("block", false);
            animator.SetTrigger("kick");
        }
        else if (!entityInput.ActivateBlock())
        {
            animator.SetBool("block", false);
            movementComponent.playerState = Basic2DMovement.PlayerState.idle;
        }
    }


}
