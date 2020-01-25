using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeathStateBehaviour : StateComponent
{
    public override void EvaluateState(Animator animator)
    {
        if ("player".Equals(movementComponent.id))
        {
            GameManager.GameOver(1.15f);
        }
    }

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        movementComponent.playerState = CharacterManager.PlayerState.death;
    }


}
