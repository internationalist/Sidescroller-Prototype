using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Basic2DMovement : EntityStates {
	public CharacterController playerController;
    public CapsuleCollider playerCollider;
    private EntityInputAbstract input;
	[Header("Inscribed")]
	public float moveSpeed;
	public float jumpSpeed;
	public float gravity;
	public float groundedDistance;
    public float defaultControllerHeight;
    public float defaultControlleryPos;
    public float retreatSpeed;
    public int enemyLayer;
	[Header("Dynamic")]
	[SerializeField]
	public Vector3 moveDirection;
	public float currentMovement;
	private float previousMovement;
	private Animator anim;
	public bool isGrounded;
	[SerializeField]
	private float distanceToGround;

    public EntityInputAbstract Input
    {
        get
        {
            return input;
        }

        set
        {
            input = value;
        }
    }




    /// <summary>
    /// Can be called to modify the height and y position of the controller, in case character is bending or crouching.
    /// </summary>
    /// <param name="height"></param>
    /// <param name="yPos"></param>
    public void ModifyControllerSize(float height, float yPos)
    {
        Vector3 centerPosition = playerController.center;
        centerPosition.y = yPos;
        playerCollider.center = centerPosition;
        playerCollider.height = height;
    }

    public void ResetControllerSize() {
        Vector3 centerPosition = playerCollider.center;
        centerPosition.y = defaultControlleryPos;
        playerCollider.center = centerPosition;
        playerCollider.height = defaultControllerHeight;
    }

    // Use this for initialization
    void Start () {
        GameManager.RegisterHumanoidEntity(gameObject, this);
		playerController = GetComponent<CharacterController>();
        playerCollider = GetComponent<CapsuleCollider>();
        Input = GetComponent<EntityInputAbstract>();
        ResetControllerSize();
		anim = playerController.GetComponentInChildren<Animator> ();
		if (anim == null) {
			Debug.LogError ("Animator could not be obtained");
		}
		playerState = PlayerState.idle;
	}

    public void FixedUpdate()
    {
        //caclulate if character is grounded.
        IsGrounded();
        //evaluate current state.
        //EvaluateState();
    }

    // Update is called once per frame
    public void Update () {
        /*switch (playerState) {
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
				playerController.Move (moveDirection * Time.deltaTime);
				break;
		case PlayerState.movement:
				CalculateMovementWithTurn ();
				ExecuteMovementWithGravity ();
				break;
        case PlayerState.retreat:
                anim.SetBool("walkbackwards", true);
                CalculateRetreat();
                ExecuteMovementWithGravity();
                break;
		case PlayerState.airborne:
				ExecuteMovementWithGravity ();
				break;
            case PlayerState.block:
                anim.SetBool("block", true);
                break;
			default:
				break;		
		}
		anim.SetBool ("jump", !IsGrounded());*/
        currentMovement = Input.MovementAmount();
    }

	void EvaluateState ()
	{
        currentMovement = Input.MovementAmount();
        if (playerState.Equals(PlayerState.crouch)) //do nothing the crouch state machine behaviour is responsible for exiting crouch baesd upon user input.
        {
            return;
        }
        if (IsAttacking) {//do nothing. Attack state will exit when animation finishes and will be triggered by the state machine behaviour.
            return;
        }
        /*if(playerState.Equals(PlayerState.attacked))//do nothing. damage state will exit when animation finishes and will be triggered by the state machine behaviour.
        {
            return;
        }*/
        if (playerState.Equals(PlayerState.block)) //do nothing the block state machine behaviour is responsible for exiting block baesd upon user input.
        {
            return;
        }
        //User input based state changes.
        if (Input.ActivateJump() && isGrounded)
        {
            playerState = PlayerState.jump;
        }
        else if (!isGrounded)
        {
            playerState = PlayerState.airborne;
        } else if (Input.ActivateCrouch()) {
				playerState = PlayerState.crouch;
        } else if (Input.ActivateBlock())
        {
            playerState = PlayerState.block;
        } else if (Input.ActivateLightAttack()) {
				playerState = PlayerState.lightattack;
		} else if (Input.ActivateHeavyAttack()) {
				playerState = PlayerState.heavyattack;
		} else if (currentMovement != 0) {
            if(Input.IsEnemyInRange(enemyLayer)) {
                    if (IsretreatInput())
                    {
                        playerState = PlayerState.retreat;
                    }
                    else
                    {
                        playerState = PlayerState.movement;
                    }
            }
            else {
                playerState = PlayerState.movement;
            }
		} else {
            playerState = PlayerState.idle;
		}
	}

    public bool IsretreatInput()
    {
        if(Vector3.Dot(transform.right, Vector3.right) == 1)
        {
            return currentMovement >= 0;
        } else if(Vector3.Dot(transform.right, -1*Vector3.right) == 1) {
            return currentMovement <= 0;
        }
        return false;

    }

    //Private methods below
    public void Crouch() {
		anim.SetBool ("crouch", true);
	}
	public void ExecuteMovementWithGravity ()
	{
		playerController.Move (moveDirection * Time.deltaTime);		
		if (!isGrounded) {
			moveDirection.y -= gravity * Time.deltaTime;
		}
	}

    public void ExecuteHeavyAttack()
    {
        if (!isAttacking)//This makes sure attacks happen one at a time
        {
            anim.SetTrigger("kick");
        }
	}

    public void ExecuteLightAttack ()
	{
        if(!isAttacking)//This makes sure attacks happen one at a time
        {
            anim.SetTrigger("punch");
        }
	}

    public void ExecuteJump ()
	{
		if (isGrounded) {
            Debug.Log("Executing jump with " + jumpSpeed);
			moveDirection.y = jumpSpeed;
		}
	}

    public void CalculateMovement ()
    {
        anim.SetFloat("movement", Mathf.Abs(currentMovement));
        CalculateMoveDirection(moveSpeed);
    }

    public void CalculateMovementWithTurn()
    {
        anim.SetFloat("movement", Mathf.Abs(currentMovement));
        CheckForTurn();
        CalculateMoveDirection(moveSpeed);
    }

    public void CalculateRetreat()
    {
        CalculateMoveDirection(retreatSpeed);
    }

    private void CalculateMoveDirection(float speed)
    {
        if (moveDirection == null)
        {
            moveDirection = new Vector3(currentMovement * -1 * speed, 0, 0);
        }
        else
        {
            moveDirection.x = currentMovement * -1 * speed;
        }
        previousMovement = currentMovement;
    }

    public bool IsGrounded ()
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

    public void CheckForTurn ()
	{
		if (previousMovement < 0) {
			playerController.transform.eulerAngles = new Vector3 (0, 0, 0);
		} else if (previousMovement > 0) {
            playerController.transform.eulerAngles = new Vector3 (0, 180, 0);
        }
	}
}
