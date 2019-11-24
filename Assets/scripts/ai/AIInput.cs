using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInput : EntityInputAbstract
{
    public float lastActionTime;
    public float actionDelayInSeconds;

    public enum ActionType { LIGHT_ATTACK, HEAVY_ATTACK, CROUCH, JUMP, BLOCK, NONE };
    public bool lightAttack;
    public bool heavyAttack;
    private float movementAmount;
    ActionType[] actionTypes = { ActionType.LIGHT_ATTACK, ActionType.HEAVY_ATTACK, ActionType.BLOCK };
    public ActionType action;
    public float[] actionProbs;
    [SerializeField]
    private bool block;

    private EntityStates player;





    public void Start()
    {
        player = GetComponent<EntityStates>();
    }

    public void Update()
    {
        float distanceToEnemy = GetDistanceToEnemy(8);

        if (player.enemyFound && distanceToEnemy < player.attackRange)
        {
            PerformCombatAction();
            movementAmount = 0;
        }
        else if(player.enemyFound && distanceToEnemy < player.spotRange)
        {
            ResetAttack();
            movementAmount = Mathf.SmoothStep(movementAmount, Vector3.Dot(transform.right, Vector3.right), .17f);
        } else
        {
            ResetAttack();
            movementAmount = 0;
        }
    }

    private void PerformCombatAction()
    {
        action = GetActionType(actionTypes, actionProbs);
        switch (action)
        {
            case ActionType.LIGHT_ATTACK:
                if (CanRunAction())
                {
                    lightAttack = true;
                }
                else
                {
                    lightAttack = false;
                }
                break;
            case ActionType.HEAVY_ATTACK:
                if (CanRunAction())
                {
                    heavyAttack = true;
                }
                else
                {
                    heavyAttack = false;
                }
                break;
            case ActionType.BLOCK:
                if (player.opponent.IsAttacking)
                {
                    block = true;
                }
                break;
        }
        if (!player.opponent.IsAttacking)
        {
            block = false;
        }
    }

    private void ResetAttack()
    {
        lightAttack = false;
        heavyAttack = false;
        block = false;
    }

    public override bool ActivateBlock()
    {
        return block;
    }

    public override bool ActivateCrouch()
    {
        return false;
    }

    public override bool ActivateHeavyAttack()
    {
        return heavyAttack;
    }

    public override bool ActivateJump()
    {
        return false;
    }

    public override bool ActivateLightAttack()
    {
        return lightAttack;
    }

    private bool CanRunAction()
    {
        if (Time.realtimeSinceStartup - lastActionTime > actionDelayInSeconds)
        {
            lastActionTime = Time.realtimeSinceStartup;
            return true;
        }
        else
        {
            return false;
        }
    }

    public override float MovementAmount()
    {
       /* if((GameManager.GetDistanceToEnemy(8, transform) < .9f) && !block)
        {
            return -.5f;
        }*/
        return movementAmount;
    }

    private ActionType GetActionType(ActionType[] actionArray, float[] probArray)
    {
        int retValue = 0;
        float total = 0;
        foreach (int element in probArray)
        {
            total += element;
        }

        float randomPoint = Random.value * total;
        for (int i = 0; i < probArray.Length; i++)
        {
            if (randomPoint < probArray[i])
            {
                retValue = i;
                break;
            } else
            {
                randomPoint -= probArray[i];
            }
        }
        return actionArray[retValue];
    }

    public override bool ActivateDash()
    {
        return false;
    }
}
