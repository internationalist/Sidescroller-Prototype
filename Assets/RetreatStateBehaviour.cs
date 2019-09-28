using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatStateBehaviour : StateComponent {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        movementComponent.playerState = Basic2DMovement.PlayerState.retreat;
    }

    public override void EvaluateState(Animator animator)
    {
        switch (movementComponent.playerState)
        {
            case Basic2DMovement.PlayerState.retreat:
                movementComponent.CalculateRetreat();
                movementComponent.ExecuteMovementWithGravity();
                break;
            case Basic2DMovement.PlayerState.idle:
                animator.SetBool("walkbackwards", false);
                break;
        }
    }

    public override void GetStateFromInput()
    {
        if (movementComponent.currentMovement == 0)
        {
            movementComponent.playerState = Basic2DMovement.PlayerState.idle;
        }
    }
}
