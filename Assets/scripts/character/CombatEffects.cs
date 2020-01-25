using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterManager))]
public class CombatEffects: MonoBehaviour
{
    private CharacterManager entity;
    public GameObject blockText;


    private void Start()
    {
        entity = GetComponent<CharacterManager>();
    }


    public void CheckIfDeadAndCreateHurtEffects(int attack, EntityStates.PlayerState playerState,
                                                        Vector3 effectPos = default(Vector3))
    {
        bool dead = entity.RegisterDamage(attack);

        Vector3 moddedEffectPos = new Vector3(effectPos.x, transform.position.y + 2f, effectPos.z);
        if (!dead)
        {
            switch(playerState)
            {
                case EntityStates.PlayerState.lightattack:
                    entity.Opponent.lightHitEffect.transform.position = moddedEffectPos;
                    entity.Opponent.lightHitEffect.Play();
                    break;
                case EntityStates.PlayerState.heavyattack:
                    entity.Opponent.heavyHitEffect.transform.position = moddedEffectPos;
                    entity.Opponent.heavyHitEffect.Play();
                    break;
                case EntityStates.PlayerState.guardbreak:
                    entity.Opponent.guardBreakEffect.transform.position = transform.position + new Vector3(0, 2f, 0);
                    entity.Opponent.guardBreakEffect.Play();
                    break;
            }
            
        }
        else
        {
            if(entity.deathEffects != null && entity.deathEffects.Length > 0)
            {
                ParticleSystem deathEffect = entity.deathEffects[Random.Range(0, entity.deathEffects.Length)];
                deathEffect.Play();
            } else
            {
                entity.deadEffect.Play();
            }
        }
        LetOpponentKnowIfDead(dead);
        Vector3 textEffectPos = new Vector3(transform.position.x + transform.right.x * .3f, transform.position.y + 3f);
        if (blockText != null)
        {
            CreateEffectsText(textEffectPos, entity.impactText[Random.Range(0, 10)]);
        }
        GameManager.PlayDamageSound(entity.hurtsound, entity.audioSources[1]);
    }

    private void LetOpponentKnowIfDead(bool dead)
    {
        if (dead)
        {
            entity.Opponent.enemyFound = false;
        }
    }

    public void CreateEffectsText(Vector3 effectPos, string effectText)
    {
        if (blockText != null)
        {
            GameObject blockTextInstance = Instantiate(blockText, effectPos, Quaternion.identity);
            blockTextInstance.GetComponent<Renderer>().sortingLayerName = "Foreground";
            blockTextInstance.GetComponent<Renderer>().sortingOrder = 1;
            FloatingText text = blockTextInstance.GetComponent<FloatingText>();
            text.SetText(effectText);
        }
    }

    public void CreateBlockEffects()
    {
        entity.blockEffect.Play();
        Vector3 effectPos = new Vector3(transform.position.x + transform.right.x * .3f, transform.position.y + 3f);
        CreateEffectsText(effectPos, "Blocked!");
    }
}
