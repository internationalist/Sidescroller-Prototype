using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	private static GameManager _S;
	[Header("Dynamic")]
	[SerializeField]
	private bool movementLock=false;
	[SerializeField]
	private bool isAttacking;
	private Basic2DMovement _player;
	public static GameManager S {
		get {
			if (_S == null) {
				Debug.LogError ("Attempt to access GameManager singleton before initialization");
			}
			return _S;
		}
		set { 
			if (_S != null) {
				Debug.LogError ("Redundant attempt to create already initialized singleton. ");
			} else {
				_S = value;				
			}
		}
	}

	void Awake () {
		_S = this;
		_player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Basic2DMovement>();
	}

	public static void SET_PLAYER_STATE(Basic2DMovement.PlayerState playerState) {
		S._player.playerState = playerState;
	}

	public static bool MOVEMENT_LOCK {
		get { return S.movementLock;}
		set {S.movementLock = value;}
	}

	public static bool IS_ATTACK {
		get { return S.isAttacking;}
		set {S.isAttacking = value;}		
	}
}
