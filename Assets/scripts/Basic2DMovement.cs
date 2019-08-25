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
	public float jumpCoolDown = .2f;
	[Header("Dynamic")]
	[SerializeField]
	private Vector3 moveDirection;
	private float currentMovement;
	private float previousMovement;
	private Animator anim;
	private float lastJumped;
	[SerializeField]
	private bool isGrounded;
	[SerializeField]
	private float distanceToGround;

	//Player action states
	public enum PlayerState {
		idle, movement, lightattack, heavyattack, jump, airborne, crouch
	}
	public PlayerState playerState;

	// Use this for initialization
	void Start () {
		player = GetComponent<CharacterController>();
		anim = player.GetComponentInChildren<Animator> ();
		if (anim == null) {
			Debug.LogError ("Animator could not be obtained");
		}
		playerState = PlayerState.idle;
	}
	
	// Update is called once per frame
	void Update () {
		//evaluate current state.
		IsGrounded ();
		EvaluateState ();
		switch (playerState) {
		case PlayerState.crouch:
				Crouch ();
				break;
			case PlayerState.heavyattack:
				ExecuteHeavyAttack ();
				break;
			case PlayerState.lightattack:
				ExecuteLightAttack ();
				break;
			case PlayerState.idle:
				CalculateMovement ();
				break;
		case PlayerState.jump:
				ExecuteJump ();
				player.Move (moveDirection * Time.deltaTime);
				//ExecuteMovementWithGravity ();
				break;
		case PlayerState.movement:
				CalculateMovement ();
				if (!GameManager.MOVEMENT_LOCK) {
					ExecuteMovementWithGravity ();
				}
				break;
			case PlayerState.airborne:
				ExecuteMovementWithGravity ();
				break;
			default:
				break;		
		}
		anim.SetBool ("jump", !IsGrounded());
	}

	void EvaluateState ()
	{
		if (Input.GetButtonDown ("Jump") && isGrounded) {
			//if (Time.realtimeSinceStartup - lastJumped > jumpCoolDown) {
				playerState = PlayerState.jump;
				//lastJumped = Time.realtimeSinceStartup;
			//}
		} else if (!isGrounded) {
			playerState = PlayerState.airborne;
		} else if (playerState.Equals (PlayerState.idle) || playerState.Equals (PlayerState.movement)) {
			if (Input.GetButton ("Crouch")) {
				playerState = PlayerState.crouch;
			} else if (Input.GetMouseButtonDown (0)) {
				Debug.Log ("State becomes light attack");
				playerState = PlayerState.lightattack;
			} else if (Input.GetMouseButtonDown (1)) {
				Debug.Log ("State becomes heavy attack");
				playerState = PlayerState.heavyattack;
			} else if (Input.GetAxis ("Horizontal") != 0) {
				playerState = PlayerState.movement;
			} else {
				playerState = PlayerState.idle;
			}
		} else {
			playerState = PlayerState.idle;
		}
	}
		
	//Private methods below
	void Crouch() {
		if (Input.GetButton ("Crouch")) {
			anim.SetBool ("crouch", true);
		}
	}
	void ExecuteMovementWithGravity ()
	{
		player.Move (moveDirection * Time.deltaTime);		
		if (!isGrounded) {
			moveDirection.y -= gravity * Time.deltaTime;
		}
	}

	void ExecuteHeavyAttack ()
	{
		if (Input.GetMouseButtonDown (1)) {
			anim.SetTrigger ("kick");
		}
	}

	void ExecuteLightAttack ()
	{
		if (Input.GetMouseButtonDown (0)) {
			anim.SetTrigger ("punch");
		}
	}

	void ExecuteJump ()
	{
		//if (Input.GetButtonDown ("Jump") && distanceToGround <= player.center.y) {
		if (isGrounded) {
			moveDirection.y = jumpSpeed;
		}
	}

	void CalculateMovement ()
	{
		currentMovement = Input.GetAxis ("Horizontal");
		anim.SetFloat ("movement", Mathf.Abs (currentMovement));
		CheckForTurn ();
		if (moveDirection == null) {
			moveDirection = new Vector3 (currentMovement * -1 * moveSpeed, 0, 0);
		} else {
			moveDirection.x = currentMovement * -1 * moveSpeed;
		}
		//moveDirection *= moveSpeed;
		previousMovement = currentMovement;
	}

	bool IsGrounded ()
	{
		RaycastHit hit;
		if (Physics.Raycast (transform.position + new Vector3 (0, .1f), -transform.up, out hit, Mathf.Infinity)) {
			Debug.DrawRay (transform.position + new Vector3 (0, .1f), -transform.up * hit.distance, Color.red);
			distanceToGround = hit.distance;
			isGrounded = distanceToGround <= groundedDistance;
		}
		else {
			Debug.DrawRay (transform.position, -transform.up * 1000, Color.yellow);
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
