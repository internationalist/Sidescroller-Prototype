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
            case Basic2DMovement.PlayerState.block:
                animator.SetBool("block", true);
                break;
            case Basic2DMovement.PlayerState.idle:
                animator.SetBool("walkbackwards", false);
                break;
            case Basic2DMovement.PlayerState.heavyattack:
                movementComponent.ExecuteHeavyAttack(); // control moves onto the heavy attack state.
                break;
            case Basic2DMovement.PlayerState.lightattack:
                movementComponent.ExecuteLightAttack();
                break;
        }
    }

    public override void GetStateFromInput(float distanceToEnemy)
    {
        if (entityInput.ActivateLightAttack())
        {
            movementComponent.playerState = Basic2DMovement.PlayerState.lightattack;
        }
        else if (entityInput.ActivateHeavyAttack())
        {
            movementComponent.playerState = Basic2DMovement.PlayerState.heavyattack;
        } else if (entityInput.ActivateBlock())
        {
            movementComponent.playerState = Basic2DMovement.PlayerState.block;
        } else if (entityInput.MovementAmount() != 0)
        {
            if (entityInput.GetDistanceToEnemy(movementComponent.enemyLayer) < movementComponent.attackRange)
            {
                if (movementComponent.IsretreatInput())
                {
                    movementComponent.playerState = Basic2DMovement.PlayerState.retreat;
                }
                else
                {
                    movementComponent.playerState = Basic2DMovement.PlayerState.movement;
                }
            }
            else
            {
                movementComponent.playerState = Basic2DMovement.PlayerState.idle;
            }
        } else
        {
            movementComponent.playerState = Basic2DMovement.PlayerState.idle;
        }
    }
}
