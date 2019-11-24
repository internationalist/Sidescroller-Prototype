using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashStateBehaviour : StateComponent {
    private float startTime;
    public float duration = .3f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        movementComponent.trails.Play();
        movementComponent.playerState = Basic2DMovement.PlayerState.dash;
        startTime = Time.time;
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //movementComponent.playerState = Basic2DMovement.PlayerState.idle;
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
                Debug.Log(timeElapsed / duration);
                movementComponent.ExecuteDash(1 - timeElapsed / duration);
            }
        } else
        {
            animator.SetBool("jump", true);
            //ddddanimator.SetBool("dash", false);
        }

    }

    public override void GetStateFromInput(float distanceToEnemy)
    {
            //Not implemented
    }
}
