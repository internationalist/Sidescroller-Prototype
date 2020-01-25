using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoStaminaStateBehaviour : StateComponent
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        if (movementComponent.outOfStamina != null)
        {
            movementComponent.outOfStamina.Play();
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (movementComponent.outOfStamina != null)
        {
            movementComponent.outOfStamina.Stop();
        }
    }
}
