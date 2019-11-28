using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TargetComponent : MonoBehaviour {
    private Animator anim;
    public EntityStates opponent;
    private Basic2DMovement entity;
    [SerializeField]
    bool isColliding;

    public ParticleSystem lightHitEffect;
    public ParticleSystem heavyHitEffect;
    public ParticleSystem blockEffect;
    public bool blockOver = false;
    public AudioClip kicksound;
    public AudioClip punchsound;
    public AudioClip hurtsound;

    public AudioClip blockSound;
    public GameObject blockText;




    public void Awake()
    {
    }
    public void Start()
    {
        anim = GetComponentInChildren<Animator>();
        entity = GetComponent<Basic2DMovement>();
    }

    public void Update()
    {
        if(entity.opponent != null && !entity.opponent.IsAttacking)
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
            if (entity.opponent.IsAttacking && !entity.playerState.Equals(EntityStates.PlayerState.block))
            {
                if (entity.opponent.playerState.Equals(EntityStates.PlayerState.lightattack))
                {
                    anim.SetBool("block", false);
                    GameManager.PlayAttackSound(punchsound, entity.audioSources[0]);
                    anim.SetTrigger("lighthit");
                    lightHitEffect.Play();
                    bool dead = entity.RegisterDamage(opponent.lightAttack);
                    LetOpponentKnowIfDead(dead);
                }
                else
                {
                    anim.SetBool("block", false);
                    GameManager.PlayAttackSound(kicksound, entity.audioSources[0]);
                    anim.SetTrigger("heavyhit");
                    heavyHitEffect.Play();
                    bool dead = entity.RegisterDamage(opponent.heavyAttack);
                    LetOpponentKnowIfDead(dead);
                }
                GameManager.PlayDamageSound(hurtsound, entity.audioSources[1]);
            }
            else if (entity.playerState.Equals(EntityStates.PlayerState.block))
            {
                GameManager.PlayAttackSound(blockSound, entity.audioSources[0]);
                anim.SetTrigger("blockreact");
                blockEffect.Play();
                Vector3 effectPos = new Vector3(transform.position.x + transform.right.x*.3f, transform.position.y + 3f);
                if(blockText != null)
                {
                    GameObject blockTextInstance = Instantiate(blockText, effectPos, Quaternion.identity);
                    blockTextInstance.GetComponent<Renderer>().sortingLayerName = "Foreground";
                    FloatingText text = blockTextInstance.GetComponent<FloatingText>();
                    text.SetText("Blocked!");
                }
                blockOver = true;
            }
        }
	}

    private void LetOpponentKnowIfDead(bool dead)
    {
        if (dead)
        {
            entity.opponent.enemyFound = false;
        }
    }

    IEnumerator AllowCollision()
    {
        yield return new WaitForSeconds(.7f);
        isColliding = false;
    }


}
