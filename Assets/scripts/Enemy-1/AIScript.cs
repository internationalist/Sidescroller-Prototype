using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScript : MonoBehaviour {

    [SerializeField]
    private bool isAttacking;

    public enum AIWarriorStates
    {
        idle, lightattack, heavyattack
    }

    public AIWarriorStates enemyState;

    public bool IsAttacking
    {
        get
        {
            return isAttacking;
        }

        set
        {
            isAttacking = value;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
