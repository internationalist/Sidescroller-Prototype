using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStates : MonoBehaviour {
    [SerializeField]
    protected bool isAttacking;
    //Player action states
    public enum PlayerState
    {
        idle, movement, lightattack, heavyattack, jump, airborne, crouch, retreat, attacked, block, attackblocked, stunned
    }

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


    public PlayerState playerState;

}
