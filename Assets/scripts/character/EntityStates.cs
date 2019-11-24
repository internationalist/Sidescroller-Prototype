using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStates : MonoBehaviour {
    [SerializeField]
    protected bool isAttacking;

    public float attackRange;
    public float spotRange;
    public float minDistance;
    public int lightAttack;
    public int heavyAttack;
    public int maxHealth;
    public float distanceToEnemy;
    public EntityStates opponent;
    public bool enemyFound;
    public string id;
    public bool ladderInRange;


    //Player action states
    public enum PlayerState
    {
        idle, movement, lightattack, heavyattack, jump, airborne, crouch, retreat, attacked, block, attackblocked, stunned,outofstamina,onedge, dash,nearladder
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

    private void Awake()
    {
    }


    public PlayerState playerState;


}
