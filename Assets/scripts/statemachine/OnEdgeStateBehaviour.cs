using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEdgeStateBehaviour : StateComponent
{
    public override void EvaluateState(Animator animator)
    {
        switch (movementComponent.playerState)
        {
            case CharacterManager.PlayerState.movement:
                animator.SetBool("onedge", false);
                break;
        }
    }

    public override void GetStateFromInput(float distanceToEnemy)
    {
        if(entityInput.MovementAmount() != 0)
        {
            movementComponent.playerState = CharacterManager.PlayerState.movement;
        }
    }
}
