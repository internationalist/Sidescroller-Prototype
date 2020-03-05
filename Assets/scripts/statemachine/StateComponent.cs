using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateComponent : StateMachineBehaviour
{
    protected CharacterManager movementComponent;
    protected EntityInputAbstract entityInput;
    protected GameObject owner;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (movementComponent == null)
        {
            owner = animator.transform.parent.gameObject;
            movementComponent = owner.GetComponent<CharacterManager>();
            entityInput = movementComponent.Input;
        }
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

    /// <summary>
    /// Stop all animations except the one specified by the string argument.
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="except"></param>
    protected void ResetParametersExcept(Animator animator, string except)
    {
        foreach(AnimatorControllerParameter parameter in animator.parameters)
        {
            if(!parameter.name.Equals(except))
            {
                if(AnimatorControllerParameterType.Bool.Equals(parameter.type))
                {
                    animator.SetBool(parameter.name, false);
                } else if(AnimatorControllerParameterType.Trigger.Equals(parameter.type))
                {
                    animator.ResetTrigger(parameter.name);
                }
            }
        }
    }

}
