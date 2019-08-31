using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetComponent : MonoBehaviour {
    private Animator anim;
    public Basic2DMovement player;
    bool isColliding;


    public void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }


    void OnTriggerEnter(Collider other)
	{
        //if (isColliding) return;
        isColliding = true;
        Debug.Log("Got hit");
        if(player.IsAttacking)
        {
            anim.SetTrigger("headhit");
        }
        StartCoroutine(AllowCollision());
	}

    IEnumerator AllowCollision()
    {
        yield return new WaitForSeconds(.1f);
        isColliding = false;
    }
}
