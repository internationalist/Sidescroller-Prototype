using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetComponent : MonoBehaviour {
    private Animator anim;
    public EntityStates opponent;
    [SerializeField]
    private EntityStates actor;
    [SerializeField]
    bool isColliding;

    public GameObject lightHitEffect;
    public GameObject heavyHitEffect;
    public GameObject blockEffect;


    public void Start()
    {
        anim = GetComponentInChildren<Animator>();
        actor = GetComponent<EntityStates>();
    }

    public void Update()
    {
        if(!opponent.IsAttacking)
        {
            isColliding = false;
        }
    }


    void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.tag.Equals("weapon")) //only record attacks that happen as a reasult of attack animations.
        {
            if (isColliding) return;
            isColliding = true;
            if (opponent.IsAttacking && !actor.playerState.Equals(EntityStates.PlayerState.block))
            {
                Vector3 effectPos = new Vector3(other.transform.position.x, other.transform.position.y, -2.72f);
                if (opponent.playerState.Equals(EntityStates.PlayerState.lightattack))
                {

                    Instantiate(lightHitEffect, effectPos, Quaternion.identity);
                    anim.SetTrigger("lighthit");
                }
                else
                {
                    Instantiate(heavyHitEffect, effectPos, Quaternion.identity);
                    anim.SetTrigger("heavyhit");
                }
            }
            else if (actor.playerState.Equals(EntityStates.PlayerState.block))
            {
                Instantiate(blockEffect, other.transform.position, Quaternion.identity);
                Debug.Log("blocked attack");
            }
        }
	}

    IEnumerator AllowCollision()
    {
        yield return new WaitForSeconds(.7f);
        isColliding = false;
    }
}
