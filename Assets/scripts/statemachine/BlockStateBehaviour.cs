using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStateBehaviour : StateComponent {

    private float originalStaminaGain;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        originalStaminaGain = movementComponent.staminaGainPerSecond;
        if (movementComponent.stamina < movementComponent.blockAttackStamina)
        {
            animator.SetBool("block", false);
        }
        movementComponent.staminaGainPerSecond /= movementComponent.blockStaminaPenalty;
    }
    

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        movementComponent.staminaGainPerSecond = originalStaminaGain;
    }

    override public void EvaluateState(Animator animator) {
        if (entityInput.ActivateDash())
        {
            //animator.SetBool("block", false);
            animator.SetBool("dash", true);
        }
        else if (entityInput.ActivateLightAttack()) {
            //movementComponent.ExecuteLightAttack(() => { animator.SetBool("block", false); });
            movementComponent.ExecuteLightAttack(null);
        }
        else if (entityInput.ActivateHeavyAttack())
        {
            //movementComponent.ExecuteHeavyAttack(() => { animator.SetBool("block", false); });
            movementComponent.ExecuteHeavyAttack(null);
        }
        else if (!entityInput.ActivateBlock())
        {
            animator.SetBool("block", false);
            movementComponent.playerState = CharacterManager.PlayerState.idle;
        } else
        {
            movementComponent.playerState = CharacterManager.PlayerState.block;
        }
    }
}
