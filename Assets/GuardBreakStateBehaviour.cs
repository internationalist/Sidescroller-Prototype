using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardBreakStateBehaviour : StateComponent
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        movementComponent.playerState = CharacterManager.PlayerState.block;
    }
}
