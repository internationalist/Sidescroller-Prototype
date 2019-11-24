using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateComponent : StateMachineBehaviour
{
    protected Basic2DMovement movementComponent;
    protected EntityInputAbstract entityInput;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (movementComponent == null)
        {
            GameObject owner = animator.transform.parent.gameObject;
            movementComponent = owner.GetComponent<Basic2DMovement>();
            entityInput = movementComponent.Input;
        }
        //movementComponent.playerState = Basic2DMovement.PlayerState.idle;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GetStateFromInput(movementComponent.distanceToEnemy);
        EvaluateState(animator);
    }

    public virtual void EvaluateState(Animator animator)
    {
        //Not implemented
    }

    public virtual void GetStateFromInput(float distanceToEnemy)
    {
        //Not implemented
    }

    protected void ResetParametersExcept(Animator animator, string except)
    {
        foreach(AnimatorControllerParameter parameter in animator.parameters)
        {
            if(!parameter.name.Equals(except))
            {
                if(AnimatorControllerParameterType.Bool.Equals(parameter.GetType()))
                {
                    animator.SetBool(parameter.name, false);
                }
            }
        }
    }

}
