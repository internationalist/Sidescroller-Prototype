﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionStateBhaviour : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		GameManager.MOVEMENT_LOCK = true;
		GameManager.IS_ATTACK = true;
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		GameManager.MOVEMENT_LOCK = false;
		GameManager.IS_ATTACK = false;
		Debug.Log ("Going back to idle");
		GameManager.SET_PLAYER_STATE (Basic2DMovement.PlayerState.idle);
	}
		
}
