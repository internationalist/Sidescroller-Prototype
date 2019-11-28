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
        ResetParametersExcept(animator, "movement");
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
                movementComponent.SetIdle();
                break;
            case Basic2DMovement.PlayerState.jump:
                movementComponent.ExecuteJump();
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
            case Basic2DMovement.PlayerState.airborne:
                //movementComponent.ExecuteMovementWithGravity();
                animator.SetBool("jump", true);
                break;
            case Basic2DMovement.PlayerState.onedge:
                movementComponent.TeeterAtEdge();
                break;
            case Basic2DMovement.PlayerState.dash:
                animator.SetBool("dash", true);
                break;
            default:
                break;
        }
    }

    public override void GetStateFromInput(float distanceToEnemy)
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
            if(movementComponent.CanBlock())
            {
                movementComponent.playerState = Basic2DMovement.PlayerState.block;
            }
        }
        else if (entityInput.ActivateLightAttack())
        {
            movementComponent.playerState = Basic2DMovement.PlayerState.lightattack;
        }
        else if (entityInput.ActivateHeavyAttack())
        {
            movementComponent.playerState = Basic2DMovement.PlayerState.heavyattack;
        }
        else if (entityInput.ActivateDash() && movementComponent.isGrounded)
        {
            movementComponent.playerState = Basic2DMovement.PlayerState.dash;
        }
        else if (entityInput.MovementAmount() != 0)
        {
            if (distanceToEnemy < movementComponent.attackRange && distanceToEnemy != -1)
            {
                if (movementComponent.IsretreatInput())
                {
                    //movementComponent.playerState = Basic2DMovement.PlayerState.dash;
                    movementComponent.playerState = Basic2DMovement.PlayerState.retreat;
                }
                else
                {
                    if (distanceToEnemy > movementComponent.minDistance)
                    {
                        movementComponent.playerState = Basic2DMovement.PlayerState.movement;
                    }
                    else
                    {
                        movementComponent.playerState = Basic2DMovement.PlayerState.idle;
                    }
                }
            }
            else
            {
                movementComponent.playerState = Basic2DMovement.PlayerState.movement;
            }
        }
        else if (movementComponent.isOnEdge)
        {
            movementComponent.playerState = Basic2DMovement.PlayerState.onedge;
        }
        else
        {
            movementComponent.playerState = Basic2DMovement.PlayerState.idle;
        }
    }
}
