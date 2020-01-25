using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateBehaviour : StateComponent
{

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        movementComponent.playerState = CharacterManager.PlayerState.idle;
        ResetParametersExcept(animator, "movement");
    }



    public override void EvaluateState(Animator animator)
    {
        switch (movementComponent.playerState)
        {
            case CharacterManager.PlayerState.crouch:
                movementComponent.Crouch();
                break;
            case CharacterManager.PlayerState.heavyattack:
                movementComponent.ExecuteHeavyAttack(null); // control moves onto the heavy attack state.
                break;
            case CharacterManager.PlayerState.lightattack:
                movementComponent.ExecuteLightAttack(null);
                break;
            case CharacterManager.PlayerState.idle:
                movementComponent.SetIdle();
                break;
            case CharacterManager.PlayerState.jump:
                movementComponent.ExecuteJump();
                break;
            case CharacterManager.PlayerState.movement:
                movementComponent.CalculateMovementWithTurn();
                movementComponent.ExecuteMovementWithGravity();
                break;
            case CharacterManager.PlayerState.retreat:
                animator.SetBool("walkbackwards", true);
                movementComponent.CalculateRetreat();
                movementComponent.ExecuteMovementWithGravity();
                break;
            case CharacterManager.PlayerState.block:
                animator.SetBool("block", true);
                break;
            case CharacterManager.PlayerState.airborne:
                animator.SetBool("jump", true);
                break;
            case CharacterManager.PlayerState.onedge:
                movementComponent.TeeterAtEdge();
                break;
            case CharacterManager.PlayerState.dash:
                movementComponent.TriggerDash();
                break;
            default:
                break;
        }
    }

    public override void GetStateFromInput(float distanceToEnemy)
    {
        if (entityInput.ActivateJump() && movementComponent.isGrounded)
        {
            movementComponent.playerState = CharacterManager.PlayerState.jump;
        }
        else if (!movementComponent.isGrounded)
        {
            movementComponent.playerState = CharacterManager.PlayerState.airborne;
        }
        else if (entityInput.ActivateCrouch())
        {
            movementComponent.playerState = CharacterManager.PlayerState.crouch;
        }
        else if (entityInput.ActivateBlock())
        {
            if(movementComponent.CanBlock())
            {
                movementComponent.playerState = CharacterManager.PlayerState.block;
            }
        }
        else if (entityInput.ActivateLightAttack())
        {
            movementComponent.playerState = CharacterManager.PlayerState.lightattack;
        }
        else if (entityInput.ActivateHeavyAttack())
        {
            movementComponent.playerState = CharacterManager.PlayerState.heavyattack;
        }
        else if (entityInput.ActivateDash() && movementComponent.isGrounded)
        {
            movementComponent.playerState = CharacterManager.PlayerState.dash;
        }
        else if (entityInput.MovementAmount() != 0)
        {
            #region commented retreat code.
            /*            if (distanceToEnemy < movementComponent.attackRange && distanceToEnemy != -1)
                        {
                            if (movementComponent.IsretreatInput())
                            {
                                movementComponent.playerState = Basic2DMovement.PlayerState.retreat;
                            }
                            else
                            {
                            movementComponent.playerState = Basic2DMovement.PlayerState.movement;
                                //}
                                //else
                                //{
                                    //movementComponent.playerState = Basic2DMovement.PlayerState.idle;
                                //}
                            //}
                        }
                        else
                        {*/
            #endregion
            movementComponent.playerState = CharacterManager.PlayerState.movement;
        }
        else if (movementComponent.isOnEdge)
        {
            movementComponent.playerState = CharacterManager.PlayerState.onedge;
        }
        else
        {
            movementComponent.playerState = CharacterManager.PlayerState.idle;
        }
    }
}
