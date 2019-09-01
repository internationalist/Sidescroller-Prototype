using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetComponent : MonoBehaviour {
    private Animator anim;
    public Basic2DMovement player;
    [SerializeField]
    bool isColliding;


    public void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void Update()
    {
        if(!player.IsAttacking)
        {
            isColliding = false;
        }
    }


    void OnTriggerEnter(Collider other)
	{
        if (isColliding) return;
        isColliding = true;
        if(player.IsAttacking)
        {
            if(player.playerState.Equals(Basic2DMovement.PlayerState.lightattack))
            {
                anim.SetTrigger("stomachhit");
            } else
            {
                anim.SetTrigger("headhit");
            }

        }
	}

    IEnumerator AllowCollision()
    {
        yield return new WaitForSeconds(.7f);
        isColliding = false;
    }
}
