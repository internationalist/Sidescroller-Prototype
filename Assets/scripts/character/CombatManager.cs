using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(CombatEffects))]
[RequireComponent(typeof(Animator))]
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
    bool isColliding;

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


    void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.tag.Equals("weapon")) //only record attacks that happen as a reasult of attack animations.
        {
            if (isColliding) return;
            isColliding = true;
            entity.KickCombo = false;
            entity.PunchCombo = false;
            if("player".Equals(entity.id))
            {
                Debug.Log("Stamina contained is " + entity.stamina);
            }

            if (entity.Opponent.IsAttacking && !entity.playerState.Equals(EntityStates.PlayerState.block))
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
        combatEffects.CheckIfDeadAndCreateHurtEffects(entity.Opponent.guardBreak,
                                                            EntityStates.PlayerState.guardbreak);
    }



    private void RunHeavyAttack(Vector3 effectPos)
    {
        anim.SetTrigger("heavyhit");
        entity.Opponent.KickCombo = true;
        anim.SetBool("block", false);
        GameManager.PlayAttackSound(entity.kicksound, entity.audioSources[0]);
        combatEffects.CheckIfDeadAndCreateHurtEffects(entity.Opponent.heavyAttack, 
                                                            EntityStates.PlayerState.heavyattack,
                                                            effectPos);
    }

    private void RunLightAttack(Vector3 effectPos)
    {
        anim.SetTrigger("lighthit");
        entity.Opponent.PunchCombo = true;
        anim.SetBool("block", false);
        GameManager.PlayAttackSound(entity.punchsound, entity.audioSources[0]);
        combatEffects.CheckIfDeadAndCreateHurtEffects(entity.Opponent.lightAttack,
                                                            EntityStates.PlayerState.lightattack,
                                                            effectPos);
    }

}
