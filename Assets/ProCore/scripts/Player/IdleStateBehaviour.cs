using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateBehaviour : StateComponent
{

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        movementComponent.playerState = Basic2DMovement.PlayerState.idle;
    }



    public override void EvaluateState(Animator animator)
    {
        switch (movementComponent.playerState)
        {
            case Basic2DMovement.PlayerState.crouch:
                movementComponent.Crouch();
                break;
            case Basic2DMovement.PlayerState.heavyattack:
                movementComponent.ExecuteHeavyAttack(); // control moves onto the heavy attack state.
                break;
            case Basic2DMovement.PlayerState.lightattack:
                movementComponent.ExecuteLightAttack();
                break;
            case Basic2DMovement.PlayerState.idle:
                movementComponent.CalculateMovement();
                break;
            case Basic2DMovement.PlayerState.jump:
                animator.SetBool("jump", true);
                break;
            case Basic2DMovement.PlayerState.movement:
                movementComponent.CalculateMovementWithTurn();
                movementComponent.ExecuteMovementWithGravity();
                break;
            case Basic2DMovement.PlayerState.retreat:
                animator.SetBool("walkbackwards", true);
                movementComponent.CalculateRetreat();
                movementComponent.ExecuteMovementWithGravity();
                break;
            case Basic2DMovement.PlayerState.block:
                animator.SetBool("block", true);
                break;
            default:
                break;
        }
    }

    public override void GetStateFromInput()
    {
        if (entityInput.ActivateJump() && movementComponent.isGrounded)
        {
            movementComponent.playerState = Basic2DMovement.PlayerState.jump;
        }
        else if (!movementComponent.isGrounded)
        {
            movementComponent.playerState = Basic2DMovement.PlayerState.airborne;
        }
        else if (entityInput.ActivateCrouch())
        {
            movementComponent.playerState = Basic2DMovement.PlayerState.crouch;
        }
        else if (entityInput.ActivateBlock())
        {
            movementComponent.playerState = Basic2DMovement.PlayerState.block;
        }
        else if (entityInput.ActivateLightAttack())
        {
            movementComponent.playerState = Basic2DMovement.PlayerState.lightattack;
        }
        else if (entityInput.ActivateHeavyAttack())
        {
            movementComponent.playerState = Basic2DMovement.PlayerState.heavyattack;
        }
        else if (entityInput.MovementAmount() != 0)
        {
            if (entityInput.IsEnemyInRange(movementComponent.enemyLayer))
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
                movementComponent.playerState = Basic2DMovement.PlayerState.movement;
            }
        }
        else
        {
            movementComponent.playerState = Basic2DMovement.PlayerState.idle;
        }
    }
}
