﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleStateBehaviour : StateMachineBehaviour {

    GameObject owner;
    AIScript aiScript;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	    if(owner == null)
        {
            owner = animator.gameObject.transform.root.gameObject;
            aiScript = owner.GetComponent<AIScript>();
        }
	}

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int layerMask = 1 << 8;
        RaycastHit hit;
        if (Physics.Raycast(owner.transform.position + new Vector3(0, 1f), owner.transform.forward, out hit, Mathf.Infinity, layerMask))
        {
            if(hit.distance < 1)
            {
                Debug.DrawRay(owner.transform.position + new Vector3(0, 1f), owner.transform.forward * hit.distance, Color.red);
                if(!aiScript.IsAttacking)
                {
                    aiScript.enemyState = AIScript.AIWarriorStates.lightattack;
                    Debug.Log("triggering attack");
                    animator.SetTrigger("lightattack");
                    aiScript.IsAttacking = true;
                }
            } else
            {
                Debug.DrawRay(owner.transform.position + new Vector3(0, 1f), owner.transform.forward * hit.distance, Color.yellow);
                aiScript.enemyState = AIScript.AIWarriorStates.idle;
            }

        }
        else
        {
            Debug.DrawRay(owner.transform.position + new Vector3(0, 1f), owner.transform.forward * 1000, Color.white);
        }
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}