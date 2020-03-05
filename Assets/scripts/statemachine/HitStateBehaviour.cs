using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStateBehaviour : StateComponent
{

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
                animator.SetBool(parameter.name, false);
        }
        base.OnStateEnter(animator, stateInfo, layerIndex);
        GameManager.TransitionToCombatSnapShot();
        movementComponent.playerState = CharacterManager.PlayerState.attacked;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        movementComponent.playerState = CharacterManager.PlayerState.idle;
        GameManager.TransitionToMainSnapShot();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        {
            Vector3 blowBack = -.2f * owner.transform.right;
            movementComponent.PlayerController.Move(blowBack * Time.deltaTime);
        }
    }
}
