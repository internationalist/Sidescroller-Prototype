using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashStateBehaviour : StateComponent {
    private float startTime;
    public float duration = .3f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.TransitionToEventSnapShot();
        GameManager.PlayEffectSound(GameManager.S.dashEffectSound);
        base.OnStateEnter(animator, stateInfo, layerIndex);
        movementComponent.trails.Play();
        movementComponent.playerState = CharacterManager.PlayerState.dash;
        startTime = Time.time;
    }

    public override void EvaluateState(Animator animator)
    {
        if(movementComponent.isGrounded)
        {
            float timeElapsed = Time.time - startTime;
            if (timeElapsed >= duration)
            {
                animator.SetBool("dash", false);
            }
            else
            {
                movementComponent.ExecuteDash(1 - timeElapsed / duration);
            }
        } else
        {
            animator.SetBool("jump", true);
        }

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.TransitionToMainSnapShot();
        base.OnStateExit(animator, stateInfo, layerIndex);

    }
}
