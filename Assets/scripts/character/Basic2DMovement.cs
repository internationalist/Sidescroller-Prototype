using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(CharacterController))]
public class Basic2DMovement : EntityStates {
	public CharacterController playerController;
    private EntityInputAbstract input;
    private TargetComponent targetComponent;
	[Header("Inscribed")]
	public float moveSpeed;
	public float jumpSpeed;
	public float gravity;
	public float groundedDistance;
    public float defaultControllerHeight;
    public float defaultControlleryPos;
    public float retreatSpeed;
    public int enemyLayer;
    private bool dead = false;
    [Header("Dynamic")]
	[SerializeField]
	public Vector3 moveDirection=Vector3.zero;
	public float currentMovement;
	private float previousMovement;
	private Animator anim;
    public Image healthBar;
    public Image staminaBar;
    [SerializeField]
	private float distanceToGround;
    public ParticleSystem trails;
    public AudioClip deathSound;
    public AudioSource[] audioSources;
    public float health;
    public float stamina;


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

    public bool RegisterDamage(int damage)
    {
        health -= damage;
        if(healthBar != null)
        {
            healthBar.fillAmount = health / maxHealth;
        }
        if (health <= 0 && !dead)
        {
            OnDeath();
        }
        return health <= 0;
    }

    public void OnDeath()
    {
        anim.SetTrigger("death");
        dead = true;
        playerController.enabled = false;

        GameManager.PlayDamageSound(deathSound, audioSources[0]);
        if (healthBar != null && !("Player").Equals(this.tag))
        {
            healthBar.transform.parent.gameObject.SetActive(false);
        }
        else {
            GameManager.OnDeathSound();
        }

    }

    // Use this for initialization
    void Start()
    {
        audioSources = GetComponents<AudioSource>();
        GameManager.RegisterHumanoidEntity(gameObject, this);
        playerController = GetComponent<CharacterController>();
        Input = GetComponent<EntityInputAbstract>();
        ResetControllerSize();
        anim = playerController.GetComponentInChildren<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator could not be obtained");
        }
        playerState = PlayerState.idle;
        health = maxHealth;
        stamina = maxStamina;
        targetComponent = GetComponent<TargetComponent>();
    }
    // Update is called once per frame
    public void Update()
    {
        currentMovement = -1*Input.MovementAmount();

        if(!this.IsAttacking)
        {
            StaminaGain();

        }
    }

    private void StaminaGain()
    {
        float staminaGain = staminaGainPerSecond * Time.deltaTime;
        if (maxStamina - stamina > staminaGain)
        {
            this.stamina += staminaGain;
        }
        else
        {
            this.stamina = maxStamina;
        }
        if(staminaBar != null)
        {
            staminaBar.fillAmount = this.stamina / maxStamina;
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
        playerController.center = centerPosition;
        playerController.height = height;
    }

    public void ResetControllerSize() {
        Vector3 centerPosition = playerController.center;
        centerPosition.y = defaultControlleryPos;
        playerController.center = centerPosition;
        playerController.height = defaultControllerHeight;
    }

    public void FixedUpdate()
    {
        //caclulate if character is grounded.
        isGrounded = IsGrounded();
        isOnEdge = IsAtEdge();
        if(!dead)
        {
            AcquireTarget();
        }

        if(distanceToEnemy == -1)
        {
            enemyFound = false;
        }
    }

    private void AcquireTarget()
    {
        RaycastHit enemyHit = GameManager
                        .GetDistanceToEnemyAtFront(
                        enemyLayer,
                        transform);
        distanceToEnemy = enemyHit.distance;

        RaycastHit enemyHitAtBack = GameManager
                        .GetDistanceToEnemyAtBack(
                        enemyLayer,
                        transform);
        float distanceToEnemyAtBack = enemyHitAtBack.distance;

        if(enemyHit.transform != null && enemyHitAtBack.transform != null)
        {
            if(distanceToEnemy <= distanceToEnemyAtBack)
            {
                opponent = enemyHit.transform.root.gameObject.GetComponent<EntityStates>();
                targetComponent.opponent = opponent;
            } else
            {
                if(!id.Equals("player"))
                {
                    FindEnemyAtBack(enemyHitAtBack, distanceToEnemyAtBack);
                }
                opponent = enemyHitAtBack.transform.root.gameObject.GetComponent<EntityStates>();
                targetComponent.opponent = opponent;
                distanceToEnemy = distanceToEnemyAtBack;
            }
            enemyFound = true;
        } else if (enemyHit.transform != null)
        {
            opponent = enemyHit.transform.root.gameObject.GetComponent<EntityStates>();
            targetComponent.opponent = opponent;
            enemyFound = true;
        } else if (enemyHitAtBack.transform != null)
        {
            if (!id.Equals("player"))
            {
                FindEnemyAtBack(enemyHitAtBack, distanceToEnemyAtBack);
            }
            enemyFound = true;
        }
        else
        {
            enemyFound = false;
        }

    }

    private void FindEnemyAtBack(RaycastHit enemyHitAtBack, float distanceToEnemyAtBack)
    {
        opponent = enemyHitAtBack.transform.root.gameObject.GetComponent<EntityStates>();
        targetComponent.opponent = opponent;
        distanceToEnemy = distanceToEnemyAtBack;
        playerController.transform.eulerAngles = new Vector3(playerController.transform.eulerAngles.x,
                                                playerController.transform.eulerAngles.y + 180,
                                                playerController.transform.eulerAngles.z);
    }

    public bool IsretreatInput()
    {
        if(Vector3.Dot(transform.right, Vector3.right) == 1)
        {
            return currentMovement > 0;
        } else if(Vector3.Dot(transform.right, -1*Vector3.right) == 1) {
            return currentMovement < 0;
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
        if (!isAttacking && stamina > 20)
        {
            GameManager.S.actionRegister = 1;
            UseStamina(20);

            anim.SetTrigger("kick");
        }
    }

    public void UseStamina(int staminaExpended)
    {
        if (this.stamina > staminaExpended)
        {
            this.stamina -= staminaExpended;
        }
        else
        {
            this.stamina = 0;
        }
        if (staminaBar != null)
        {
            staminaBar.fillAmount = this.stamina / maxStamina;
        }
    }

    public void ExecuteLightAttack ()
	{
        if (!isAttacking && stamina > 15)
        {
            GameManager.S.actionRegister = 1;
            UseStamina(15);
            anim.SetTrigger("punch");
        }
	}

    public bool CanBlock()
    {
        if(stamina > 5)
        {
            UseStamina(1);
            anim.SetBool("block", true);
            return true;
        }
        return false;
    }

    public void ExecuteJump ()
	{
		if (isGrounded) {
			moveDirection.y = jumpSpeed;
		}
	}

    public void ExecuteDash(float decay)
    {
        Vector3 dir = transform.right * -1f* 8f * decay;
        moveDirection = dir;
        ExecuteMovementWithGravity();
    }
    public void TeeterAtEdge()
    {
        anim.SetBool("onedge", true);
    }

    public void CalculateMovement ()
    {
        anim.SetFloat("movement", Mathf.Abs(currentMovement));
        CalculateMoveDirection(moveSpeed);
    }

    public void SetIdle()
    {
        anim.SetFloat("movement", 0);
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
        moveDirection.x = currentMovement * -1 * speed;
        previousMovement = currentMovement;
    }

    public bool IsGrounded ()
	{
		RaycastHit hit;
        bool isGrounded;
        int layerMask = 1 << 13;

        if (Physics.Raycast (transform.position + new Vector3 (0, .1f), -transform.up, out hit, Mathf.Infinity, ~layerMask)) {
			Debug.DrawRay (transform.position + new Vector3 (0, .1f), -transform.up * hit.distance, Color.red);
			distanceToGround = hit.distance;
			isGrounded = distanceToGround <= groundedDistance;
		}
		else {
			Debug.DrawRay (transform.position, -transform.up * 1000, Color.yellow);
			isGrounded = false;
		}
		return isGrounded;
	}

    public bool IsAtEdge()
    {
        RaycastHit hit;
        int layerMask = 1 << 13;
        bool isAtEdge=false;
        Vector3 edgeDetectorPos = transform.right * .8f;
        if (Physics.Raycast(transform.position + new Vector3(edgeDetectorPos.x, 2f), -transform.up, out hit, Mathf.Infinity, ~layerMask))
        {
            Debug.DrawRay(transform.position + new Vector3(edgeDetectorPos.x, 2f), -transform.up * hit.distance, Color.red);
            //isAtEdge = hit.distance > 5f;
        }
        else
        {
            isAtEdge = true;
        }
        return isGrounded && isAtEdge;
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
