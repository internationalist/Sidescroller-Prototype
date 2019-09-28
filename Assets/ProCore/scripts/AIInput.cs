using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInput : EntityInputAbstract
{
    private float lastActionTime;
    public float actionDelayInSeconds;
    public enum ActionType { LIGHT_ATTACK, HEAVY_ATTACK, BLOCK, CROUCH, JUMP};
    private bool lightAttack;
    private bool heavyAttack;
    [SerializeField]
    private bool block;
    private EntityStates player;

    public void Start()
    {
        player = GetComponent<TargetComponent>().opponent;
    }

    public void Update()
    {
        if(player.IsAttacking)
        {
            block = true;
        } else
        {
            block = false;
        }
        /*
        if(IsEnemyInRange(8) && CanRunAction())
        {
            ActionType actionType = GetActionType();
            switch(actionType)
            {
                case ActionType.LIGHT_ATTACK:
                    lightAttack = true;
                    break;
                case ActionType.HEAVY_ATTACK:
                    heavyAttack = true;
                    break;
            }
        } else
        {
            ResetAttack();
        }*/
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
        if((GetDistanceToEnemy(8) < .9f) && !block)
        {
            return -1;
        }
        return 0;
    }

    private ActionType GetActionType()
    {
        int retValue = 0;
        int[] probArray = { 50, 50};
        ActionType[] actionArray = { ActionType.LIGHT_ATTACK, ActionType.HEAVY_ATTACK };
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

}
