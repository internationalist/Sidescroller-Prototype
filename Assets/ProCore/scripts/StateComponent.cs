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
        movementComponent.playerState = Basic2DMovement.PlayerState.idle;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GetStateFromInput();
        EvaluateState(animator);
    }

    public abstract void GetStateFromInput();
    public abstract void EvaluateState(Animator animator);

}
