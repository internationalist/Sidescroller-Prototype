using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(CombatEffects))]
[RequireComponent(typeof(CharacterManager))]
/// <summary>
/// Primary class that calculates the hit physics between weapon and target.
/// </summary>
/// 
public class CombatManager : MonoBehaviour {
    private Animator anim;
    private CharacterManager entity;
    CombatEffects combatEffects;
    [SerializeField]
    private bool isColliding;
    private string weaponTagName;
    private bool doubleTap;
    float lastHitTime;
    public bool multiTap;

    public bool blockOver = false;

    private float hitBlowBackStartPos;

    public void Awake()
    {
    }
    public void Start()
    {
        anim = GetComponentInChildren<Animator>();
        entity = GetComponent<CharacterManager>();
        combatEffects = GetComponent<CombatEffects>();
    }

    public void Update()
    {
        if(entity.Opponent != null && !entity.Opponent.IsAttacking)
        {
            isColliding = false;
            blockOver = false;
        }
    }

    private bool canReturn()
    {
        bool retValue = false;
        if(multiTap) {
            if ((Time.time - lastHitTime) < .3f)
            {
                retValue = true;
            }
            else if (isColliding)
            {
                doubleTap = true;
            }
            else
            {
                doubleTap = false;
            }
        } else
        {
            if(isColliding)
            {
                retValue = true;
            }
        }

        return retValue;
    }


    void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.tag.StartsWith("weapon")) //only record attacks that happen as a reasult of attack animations.
        {



            if (canReturn())
            {
                return;
            }

            lastHitTime = Time.time;
            weaponTagName = other.gameObject.tag;
            isColliding = true;
            entity.KickCombo = false;
            entity.PunchCombo = false;
            if(other.gameObject.tag.Equals("weaponprojectile"))
            {
                if(entity.isGrounded)
                {
                    anim.SetTrigger("throwback");
                }
                bool dead = entity.RegisterDamage(entity.Opponent.heavyAttack);
            } else if (entity.Opponent.IsAttacking && !entity.playerState.Equals(EntityStates.PlayerState.block))
            {
                Vector3 effectPos = new Vector3(other.transform.position.x, other.transform.position.y, 3f);
                if (entity.Opponent.playerState.Equals(EntityStates.PlayerState.lightattack))
                {
                    RunLightAttack(effectPos);
                }
                else
                {
                    RunHeavyAttack(effectPos);
                }
            }/* else if(entity.playerState.Equals(EntityStates.PlayerState.block) && (entity.stamina <= entity.guardBreakStamina))
            {
                RunGuardBreak();
            }*/
            else if (entity.playerState.Equals(EntityStates.PlayerState.block))
            {
                RunBlock(entity.Opponent.playerState);
            }
        }
	}

    private void RunBlock(EntityStates.PlayerState playerState)
    {
        switch(playerState)
        {
            case EntityStates.PlayerState.heavyattack:
                entity.UseStamina(entity.heavyAttackStamina/3f);
                break;
            case EntityStates.PlayerState.lightattack:
                entity.UseStamina(entity.lightAttackStamina/3f);
                break;
        }
        anim.SetTrigger("blockreact");
        entity.Opponent.KickCombo = false;
        entity.Opponent.PunchCombo = false;
        GameManager.PlayAttackSound(entity.blockSound, entity.audioSources[0]);
        combatEffects.CreateBlockEffects();
        blockOver = true;
    }


    private void RunGuardBreak()
    {
        anim.SetTrigger("guardbreak");
        GameManager.PlayAttackSound(entity.punchsound, entity.audioSources[0]);
        bool dead = entity.RegisterDamage(entity.Opponent.guardBreak);
        combatEffects.CheckIfDeadAndCreateHurtEffects(dead,
                                                            EntityStates.PlayerState.guardbreak);
    }



    private void RunHeavyAttack(Vector3 effectPos)
    {
        if (doubleTap)
        {
            anim.SetTrigger("heavyhit-2");
        }
        else
        {
            anim.SetTrigger("heavyhit");
        }
        entity.Opponent.KickCombo = true;
        anim.SetBool("block", false);
        GameManager.PlayAttackSound(entity.kicksound, entity.audioSources[0]);
        bool dead = entity.RegisterDamage(entity.Opponent.heavyAttack);
        combatEffects.CheckIfDeadAndCreateHurtEffects(dead, 
                                                            EntityStates.PlayerState.heavyattack,
                                                            effectPos);
    }

    private void RunLightAttack(Vector3 effectPos)
    {
        if (doubleTap)
        {
            anim.SetTrigger("lighthit-2");
        }
        else
        {
            anim.SetTrigger("lighthit");
        }
        entity.Opponent.PunchCombo = true;
        anim.SetBool("block", false);
        GameManager.PlayAttackSound(entity.punchsound, entity.audioSources[0]);
        bool dead = entity.RegisterDamage(entity.Opponent.lightAttack);
        combatEffects.CheckIfDeadAndCreateHurtEffects(dead,
                                                            EntityStates.PlayerState.lightattack,
                                                            effectPos);
    }

}
