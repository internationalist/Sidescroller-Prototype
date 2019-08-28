using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	private static GameManager _S;
	private Basic2DMovement _player;
    private Dictionary<GameObject, Basic2DMovement> humanoidEntityCache;
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
        this.humanoidEntityCache = new Dictionary<GameObject, Basic2DMovement>();
		_S = this;
		_player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Basic2DMovement>();
	}

    public static void RegisterHumanoidEntity(GameObject gObj, Basic2DMovement movementData) {
        if(!S.humanoidEntityCache.ContainsKey(gObj))
        {
            S.humanoidEntityCache.Add(gObj, movementData);
        }
    }

	public static void SET_PLAYER_STATE(GameObject entityKey, Basic2DMovement.PlayerState playerState) {
        Basic2DMovement entityData = S.humanoidEntityCache[entityKey];
		entityData.playerState = playerState;
	}

    public static void SetControllerSize(GameObject entityKey, float height, float yPos)
    {
        Basic2DMovement entityData = S.humanoidEntityCache[entityKey];
        entityData.ModifyControllerSize(height, yPos);
    }

    public static void ResetControllerSize(GameObject entityKey) {
        Basic2DMovement entityData = S.humanoidEntityCache[entityKey];
        entityData.ResetControllerSize();
    }

	public static bool IsAtacking(GameObject entityKey) {
        Basic2DMovement entityData = S.humanoidEntityCache[entityKey];
        return entityData.IsAttacking;		
	}

    public static void SetAtacking(GameObject entityKey, bool attacking)
    {
        Basic2DMovement entityData = S.humanoidEntityCache[entityKey];
        entityData.IsAttacking = attacking;
    }
}
