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
    public float defaultControllerHeight;
    public float defaultControlleryPos;
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
    [SerializeField]
    private bool isAttacking;

	//Player action states
	public enum PlayerState {
		idle, movement, lightattack, heavyattack, jump, airborne, crouch
	}
	public PlayerState playerState;

    public bool IsAttacking
    {
        get
        {
            return isAttacking;
        }

        set
        {
            isAttacking = value;
        }
    }
    /// <summary>
    /// Can be called to modify the height and y position of the controller, in case character is bending or crouching.
    /// </summary>
    /// <param name="height"></param>
    /// <param name="yPos"></param>
    public void ModifyControllerSize(float height, float yPos)
    {
        Vector3 centerPosition = player.center;
        centerPosition.y = yPos;
        player.center = centerPosition;
        player.height = height;
    }

    public void ResetControllerSize() {
        Vector3 centerPosition = player.center;
        centerPosition.y = defaultControlleryPos;
        player.center = centerPosition;
        player.height = defaultControllerHeight;
    }

    // Use this for initialization
    void Start () {
        GameManager.RegisterHumanoidEntity(gameObject, this);
		player = GetComponent<CharacterController>();
        ResetControllerSize();
		anim = player.GetComponentInChildren<Animator> ();
		if (anim == null) {
			Debug.LogError ("Animator could not be obtained");
		}
		playerState = PlayerState.idle;
	}
	
	// Update is called once per frame
	void Update () {
        //caclulate if character is grounded.
		IsGrounded ();
        //evaluate current state.
        EvaluateState();
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
				break;
		case PlayerState.movement:
				CalculateMovement ();
				ExecuteMovementWithGravity ();
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
        if(anim.GetBool("crouch")) //do nothing the crouch state machine behaviour is responsible for exiting crouch baesd upon user input.
        {
            return;
        }
        if (IsAttacking) {//do nothing. Attack state will exit when animation finishes and will be triggered by the state machine behaviour.
            return;
        }
        //User input based state changes.
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerState = PlayerState.jump;
        }
        else if (!isGrounded)
        {
            playerState = PlayerState.airborne;
        } else if (Input.GetButton ("Crouch")) {
				playerState = PlayerState.crouch;
		} else if (Input.GetMouseButtonDown (0)) {
				playerState = PlayerState.lightattack;
		} else if (Input.GetMouseButtonDown (1)) {
				playerState = PlayerState.heavyattack;
		} else if (Input.GetAxis ("Horizontal") != 0) {
				playerState = PlayerState.movement;
		} else {
				playerState = PlayerState.idle;
		}
	}
		
	//Private methods below
	void Crouch() {
		anim.SetBool ("crouch", true);
	}
	void ExecuteMovementWithGravity ()
	{
		player.Move (moveDirection * Time.deltaTime);		
		if (!isGrounded) {
			moveDirection.y -= gravity * Time.deltaTime;
		}
	}

    void ExecuteHeavyAttack()
    {
        if (!isAttacking)//This makes sure attacks happen at a time
        {
            anim.SetTrigger("kick");
        }
	}

	void ExecuteLightAttack ()
	{
        if(!isAttacking)//This makes sure attacks happen at a time
        {
            anim.SetTrigger("punch");
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
