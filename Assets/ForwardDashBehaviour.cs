using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardDashBehaviour : StateComponent
{
    private float startTime;
    public float duration = .3f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        movementComponent.trails.Play();
        movementComponent.playerState = CharacterManager.PlayerState.forwarddash;
        startTime = Time.time;
    }
    public override void EvaluateState(Animator animator)
    {
        if (movementComponent.isGrounded)
        {
            float timeElapsed = Time.time - startTime;
            if (timeElapsed < duration)
            {
                movementComponent.ExecuteForwardDash(1 - timeElapsed / duration);
            }
        }
        else
        {
            animator.SetBool("jump", true);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        movementComponent.trails.Stop();
        base.OnStateExit(animator, stateInfo, layerIndex);
    }
}
