using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpStateBehaviour : StateComponent {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        movementComponent.playerState = CharacterManager.PlayerState.jump;
        movementComponent.ExecuteMovementWithGravity();
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool("dash", false);
        //ResetParametersExcept(animator, "jump");
        movementComponent.playerState = CharacterManager.PlayerState.idle;
	}

    public override void EvaluateState(Animator animator)
    {
        switch (movementComponent.playerState)
        {
            case CharacterManager.PlayerState.airborne:
                movementComponent.ExecuteMovementWithGravity();
                break;
            case CharacterManager.PlayerState.idle:
                animator.SetBool("jump", false);
                break;
        }
    }

    public override void GetStateFromInput(float distanceToEnemy)
    {
        if (!movementComponent.isGrounded)
        {
            movementComponent.playerState = CharacterManager.PlayerState.airborne;
        }
        else
        {
            movementComponent.playerState = CharacterManager.PlayerState.idle;
        }
    }
}
