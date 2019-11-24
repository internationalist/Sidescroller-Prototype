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
}
