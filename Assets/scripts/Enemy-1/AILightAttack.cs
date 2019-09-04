using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILightAttack : StateMachineBehaviour {

    AIScript aiScript;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (aiScript == null)
        {
            GameObject owner = animator.gameObject.transform.root.gameObject;
            aiScript = owner.GetComponent<AIScript>();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        aiScript.enemyState = AIScript.AIWarriorStates.idle;
        aiScript.IsAttacking = false;
	}
}
