using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatStateBehaviour : StateComponent {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        movementComponent.playerState = CharacterManager.PlayerState.retreat;
    }

    public override void EvaluateState(Animator animator)
    {
        switch (movementComponent.playerState)
        {
            case CharacterManager.PlayerState.retreat:
                movementComponent.CalculateRetreat();
                movementComponent.ExecuteMovementWithGravity();
                break;
            case CharacterManager.PlayerState.block:
                animator.SetBool("block", true);
                break;
            case CharacterManager.PlayerState.idle:
                animator.SetBool("walkbackwards", false);
                break;
            case CharacterManager.PlayerState.heavyattack:
                movementComponent.ExecuteHeavyAttack(null); // control moves onto the heavy attack state.
                break;
            case CharacterManager.PlayerState.lightattack:
                movementComponent.ExecuteLightAttack(null);
                break;
        }
    }

    public override void GetStateFromInput(float distanceToEnemy)
    {
        if (entityInput.ActivateLightAttack())
        {
            movementComponent.playerState = CharacterManager.PlayerState.lightattack;
        }
        else if (entityInput.ActivateHeavyAttack())
        {
            movementComponent.playerState = CharacterManager.PlayerState.heavyattack;
        } else if (entityInput.ActivateBlock())
        {
            movementComponent.playerState = CharacterManager.PlayerState.block;
        } else if (entityInput.MovementAmount() != 0)
        {
            if (entityInput.GetDistanceToEnemy(movementComponent.enemyLayer) < movementComponent.attackRange)
            {
                if (movementComponent.IsretreatInput())
                {
                    movementComponent.playerState = CharacterManager.PlayerState.retreat;
                }
                else
                {
                    movementComponent.playerState = CharacterManager.PlayerState.movement;
                }
            }
            else
            {
                movementComponent.playerState = CharacterManager.PlayerState.idle;
            }
        } else
        {
            movementComponent.playerState = CharacterManager.PlayerState.idle;
        }
    }
}
