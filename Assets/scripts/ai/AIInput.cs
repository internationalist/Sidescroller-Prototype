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
    public bool throwProjectile;
    public bool runDash;
    
    ActionType[] actionTypes = { ActionType.LIGHT_ATTACK, ActionType.HEAVY_ATTACK, ActionType.BLOCK };
    public ActionType action;
    public float[] actionProbs;
    [SerializeField]
    private bool block;

    private EntityStates player;

    [SerializeField]
    private float movementAmount;

    private bool projectileThrow;

    private float lastTimeThrown;
    public float projectileThrowInterval;

    public float lastDashTime;
    public float dashInterval;
    public bool throwProjectileEnable;





    public void Start()
    {
        player = GetComponent<EntityStates>();
    }

    public void Update()
    {
        float distanceToEnemy = GetDistanceToEnemy(8);

        if (player.enemyFound && distanceToEnemy < player.attackRange)
        {
            ResetAttack();
            PerformCombatAction();
            movementAmount = 0;
        }
        else if (player.enemyFound && distanceToEnemy < player.spotRange)
        {
            ResetAttack();
            if (player.enemyFound && distanceToEnemy < player.aiDashRange && Time.time - lastDashTime > dashInterval)
            {
                lastDashTime = Time.time;
                string[] dashornot = { "dash", "not" };
                float[] probs = { 25, 75 };
                string outcome = UtilityStatic.getOutCome(dashornot, probs);

                if (outcome.Equals("dash"))
                {
                    Debug.Log(outcome);
                    runDash = true;
                }
            } else { 
                if (throwProjectileEnable && (Time.time - lastTimeThrown > projectileThrowInterval) && distanceToEnemy > player.throwRange)
                {
                    string[] dashornot = { "throw", "not" };
                    float[] probs = { 90, 10 };
                    string outcome = UtilityStatic.getOutCome(dashornot, probs);
                    if (outcome.Equals("throw"))
                    {
                        throwProjectile = true;
                    }
                    lastTimeThrown = Time.time;
                }
                else
                {
                    float transitionSpeed = .17f;
                    if (distanceToEnemy < 1.2f)
                    {
                        transitionSpeed = .08f;
                    }
                    movementAmount = Mathf.SmoothStep(movementAmount, Vector3.Dot(transform.right, Vector3.right), transitionSpeed);
                }
            }
        }
        else
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
                if (player.Opponent.IsAttacking)
                {
                    block = true;
                }
                break;
        }
        if (!player.Opponent.IsAttacking)
        {
            block = false;
        }
    }

    private void ResetAttack()
    {
        lightAttack = false;
        heavyAttack = false;
        block = false;
        throwProjectile = false;
        runDash = false;
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
        return movementAmount;
    }

    private ActionType GetActionType(ActionType[] actionArray, float[] probArray)
    {
        return UtilityStatic.getOutCome(actionArray, probArray);
    }

    public override bool ActivateDash()
    {
        return runDash;
    }

    public override bool ActivateThrow()
    {
        return throwProjectile;
    }

}
