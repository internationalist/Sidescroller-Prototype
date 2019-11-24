using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpStateBehaviour : StateComponent {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        movementComponent.playerState = Basic2DMovement.PlayerState.jump;
        movementComponent.ExecuteMovementWithGravity();
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool("dash", false);
        ResetParametersExcept(animator, "jump");
        movementComponent.playerState = Basic2DMovement.PlayerState.idle;
	}

    public override void EvaluateState(Animator animator)
    {
        switch (movementComponent.playerState)
        {
            case Basic2DMovement.PlayerState.airborne:
                movementComponent.ExecuteMovementWithGravity();
                break;
            case Basic2DMovement.PlayerState.idle:
                animator.SetBool("jump", false);
                break;
        }
    }

    public override void GetStateFromInput(float distanceToEnemy)
    {
        if (!movementComponent.isGrounded)
        {
            movementComponent.playerState = Basic2DMovement.PlayerState.airborne;
        }
        else
        {
            movementComponent.playerState = Basic2DMovement.PlayerState.idle;
        }
    }
}
