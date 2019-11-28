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

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (entityInput.ActivateDash())
        {
            animator.SetBool("block", false);
            animator.SetBool("dash", true);
        }
        else if (entityInput.ActivateLightAttack()) {
            ExecuteAttack("punch", animator);
        }
        else if (entityInput.ActivateHeavyAttack())
        {
            ExecuteAttack("kick", animator);
        }
        else if (!entityInput.ActivateBlock())
        {
            animator.SetBool("block", false);
            movementComponent.playerState = Basic2DMovement.PlayerState.idle;
        } else
        {
            movementComponent.playerState = Basic2DMovement.PlayerState.block;
        }
    }

    public void ExecuteAttack(string attack, Animator animator)
    {
        if (!movementComponent.isAttacking && movementComponent.stamina > 15)
        {
            animator.SetBool("block", false);
            GameManager.S.actionRegister = 1;
            movementComponent.UseStamina(15);
            animator.SetTrigger(attack);
        }
    }


}
