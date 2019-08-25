using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackComponent : MonoBehaviour {
	public bool hitLanded;
	public Transform player;
	public float hitForce;

	void OnTriggerEnter(Collider other)
	{
		Debug.Log ("attack landed");
		//Debug.DrawRay (transform.position, transform.forward*5, Color.red);
		hitLanded = true;
		if (GameManager.IS_ATTACK) {
			other.attachedRigidbody.AddForceAtPosition (transform.position, player.right * hitForce);
		}
	}
	public void Update() {
		if (hitLanded) {
			Debug.DrawRay (transform.position, player.right*5, Color.red);		
		}
	}
}
