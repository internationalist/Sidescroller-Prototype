using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Basic2DMovement : MonoBehaviour {
	private CharacterController player;
	[Header("Inscribed")]
	public float moveSpeed;
	public float jumpSpeed;
	public float gravity;
	public float groundedDistance;
	[Header("Dynamic")]
	[SerializeField]
	private Vector3 moveDirection;
	private float currentMovement;
	private float previousMovement;
	private Animator anim;
	[SerializeField]
	private bool isGrounded;
	[SerializeField]
	private float distanceToGround;
	// Use this for initialization
	void Start () {
		player = GetComponent<CharacterController>();
		anim = player.GetComponentInChildren<Animator> ();
		if (anim == null) {
			Debug.LogError ("Animator could not be obtained");
		}
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetBool ("jump", !IsGrounded ());
		if (player.isGrounded) {
			currentMovement = Input.GetAxis ("Horizontal");
			anim.SetFloat ("movement", Mathf.Abs (currentMovement));
			CheckForTurn ();
			moveDirection = new Vector3 (currentMovement * -1, 0, 0);
			moveDirection *= moveSpeed;
			if (Input.GetButtonDown("Jump") && distanceToGround <= player.center.y) {
				moveDirection.y = jumpSpeed;
			}
			if (Input.GetMouseButtonDown (0)) {
				Debug.Log ("Left mouse button clicked");
			}
			if (Input.GetMouseButtonDown (1)) {
				Debug.Log ("Right mouse button clicked");
			}
		}

		moveDirection.y -= gravity * Time.deltaTime;
		player.Move(moveDirection*Time.deltaTime);
		previousMovement = currentMovement;
	}

	bool IsGrounded ()
	{
		RaycastHit hit;
		if (Physics.Raycast (transform.position + new Vector3 (0, .1f), -transform.up, out hit, Mathf.Infinity)) {
			//Debug.DrawRay (transform.position + new Vector3 (0, .1f), -transform.up * hit.distance, Color.black);
			distanceToGround = hit.distance;
			isGrounded = hit.distance <= groundedDistance;
		}
		else {
			//Debug.DrawRay (transform.position, -transform.up * 1000, Color.green);
			Debug.Log ("Did not Hit ground, character is falling");
			isGrounded = false;
		}
		return isGrounded;
	}

	void CheckForTurn ()
	{ 
		if (previousMovement < 0) {
			player.transform.eulerAngles = new Vector3 (0, 0, 0);
		} else if (previousMovement > 0) {
			player.transform.eulerAngles = new Vector3 (0, 180, 0);
		}
	}
}
