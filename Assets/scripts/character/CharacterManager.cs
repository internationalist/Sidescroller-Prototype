using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(CharacterController))]
public class CharacterManager : EntityStates {
	private CharacterController playerController;

    public delegate void SetupAttack();

    private EntityInputAbstract input;
    private bool dead = false;
	private float previousMovement;
	private Animator anim;
    [SerializeField]
	private float distanceToGround;
    //to track combos
    private int kickComboIndex;
    private int punchComboIndex;

    private AnimatorOverrideController animatorOverrideController;


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

    public CharacterController PlayerController
    {
        get
        {
            return playerController;
        }

        set
        {
            playerController = value;
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

    // Use this for initialization
    void Start()
    {
        audioSources = GetComponents<AudioSource>();
        PlayerController = GetComponent<CharacterController>();
        Input = GetComponent<EntityInputAbstract>();
        ResetControllerSize();

        anim = PlayerController.GetComponentInChildren<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator could not be obtained");
        }
        animatorOverrideController = new AnimatorOverrideController(anim.runtimeAnimatorController);
        anim.runtimeAnimatorController = animatorOverrideController;
        playerState = PlayerState.idle;
        health = maxHealth;
        stamina = maxStamina;
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

    public void FixedUpdate()
    {
        //caclulate if character is grounded.
        isGrounded = IsGrounded();
        isOnEdge = IsAtEdge();
        if (!dead && !id.Equals("player"))
        {
            AcquireTarget();
        }

        if (distanceToEnemy == -1)
        {
            enemyFound = false;
        }
    }

    /// <summary>
    /// Can be called to modify the height and y position of the controller, in case character is bending or crouching.
    /// </summary>
    /// <param name="height"></param>
    /// <param name="yPos"></param>
    public void ModifyControllerSize(float height, float yPos)
    {
        Vector3 centerPosition = PlayerController.center;
        centerPosition.y = yPos;
        PlayerController.center = centerPosition;
        PlayerController.height = height;
    }

    public void ResetControllerSize() {
        Vector3 centerPosition = PlayerController.center;
        centerPosition.y = defaultControlleryPos;
        PlayerController.center = centerPosition;
        PlayerController.height = defaultControllerHeight;
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

    public void Crouch() {
		anim.SetBool ("crouch", true);
	}
	public void ExecuteMovementWithGravity ()
	{
		PlayerController.Move (moveDirection * Time.deltaTime);
        if (!isGrounded) {
            
			moveDirection.y -= gravity * Time.deltaTime;
		}
	}

    public void ExecuteHeavyAttack(SetupAttack setup)
    {
        if (!isAttacking && stamina > minimumStamina)
        {
            if(setup != null)
            {
                setup();
            }
            GameManager.S.actionRegister = 1;
            UseStamina(heavyAttackStamina);
            kickComboIndex = HandleActionCombo(KickCombo,
                                               kicks,
                                               kickComboIndex,
                                               "back_kick",
                                               "kick");
        }
        else if(stamina < heavyAttackStamina)
        {
            OnStaminaBreak();
        }
    }

    public void ExecuteLightAttack(SetupAttack setup)
    {
        if (!isAttacking && stamina > minimumStamina)
        {
            if (setup != null)
            {
                setup();
            }
            GameManager.S.actionRegister = 1;
            UseStamina(lightAttackStamina);
            punchComboIndex = HandleActionCombo(PunchCombo,
                                                punches,
                                                punchComboIndex,
                                                "Right_Strike1",
                                                "punch");
        }
        else if (stamina < lightAttackStamina)
        {
            OnStaminaBreak();
        }
    }

    public void OnDeath()
    {
        if(deaths != null && deaths.Length > 0)
        {
            int deathAnimIndex = Random.Range(0, deaths.Length);
            animatorOverrideController["Death"] = deaths[deathAnimIndex];
        }
        anim.SetTrigger("death");
        dead = true;
        PlayerController.enabled = false;
        playerState = PlayerState.death;

        GameManager.PlayDamageSound(deathSound, audioSources[0]);
        if (healthBar != null && !("Player").Equals(this.tag))
        {
            healthBar.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            GameManager.OnDeathSound();
        }

    }


    public void UseStamina(float staminaExpended)
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

    public void OnStaminaBreak()
    {
        anim.SetTrigger("nostamina");
    }

    public bool CanBlock()
    {
        if (stamina > minimumStamina)
        {
            anim.SetBool("block", true);
            return true;
        }
        else {
            OnStaminaBreak();
        }
        return false;
    }

    public void ExecuteJump ()
	{
		if (isGrounded && stamina > minimumStamina) {
            UseStamina(10);
            moveDirection.y = jumpSpeed;
            anim.SetBool("jump", true);
        } else if(stamina < 10)
        {
            OnStaminaBreak();
        }
	}

    public void TriggerDash()
    {
        if(stamina > minimumStamina)
        {
            UseStamina(15);
            anim.SetBool("dash", true);
        } else
        {
            OnStaminaBreak();
        }
    }

    public void ExecuteDash(float decay)
    {
        Vector3 dir = transform.right * -1f * 8f * decay;
        moveDirection = dir;
        ExecuteMovementWithGravity();
    }
    public void TeeterAtEdge()
    {
        anim.SetBool("onedge", true);
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

    /**
     * PRIVATE METHODS BELOW
     */

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

        if (enemyHit.transform != null && enemyHitAtBack.transform != null)
        {
            if (distanceToEnemy <= distanceToEnemyAtBack)
            {
                Opponent = enemyHit.transform.root.gameObject.GetComponent<EntityStates>();
            }
            else
            {
                if (!id.Equals("player"))
                {
                    FindEnemyAtBack(enemyHitAtBack, distanceToEnemyAtBack);
                }
                Opponent = enemyHitAtBack.transform.root.gameObject.GetComponent<EntityStates>();
                distanceToEnemy = distanceToEnemyAtBack;
            }
            enemyFound = true;
        }
        else if (enemyHit.transform != null)
        {
            Opponent = enemyHit.transform.root.gameObject.GetComponent<EntityStates>();
            enemyFound = true;
        }
        else if (enemyHitAtBack.transform != null)
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
        Opponent = enemyHitAtBack.transform.root.gameObject.GetComponent<EntityStates>();
        distanceToEnemy = distanceToEnemyAtBack;
        PlayerController.transform.eulerAngles = new Vector3(PlayerController.transform.eulerAngles.x,
                                                PlayerController.transform.eulerAngles.y + 180,
                                                PlayerController.transform.eulerAngles.z);
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
        if (staminaBar != null)
        {
            staminaBar.fillAmount = this.stamina / maxStamina;
        }
    }

    private int HandleActionCombo(bool combo, AnimationClip[] actions, int actionIndex, string animationName, string triggerName)
    {
        if (combo)
        {
            if (actions != null && actions.Length > 0)
            {
                if (actionIndex < actions.Length - 1)
                {
                    ++actionIndex;
                }
                else
                {
                    actionIndex = 0;
                }
                animatorOverrideController[animationName] = actions[actionIndex];
            }
            anim.SetTrigger(triggerName);
        }
        else
        {
            if (actions != null && actions.Length > 0)
            {
                animatorOverrideController[animationName] = actions[0];
            }
            anim.SetTrigger(triggerName);
        }
        return actionIndex;
    }

    private void CalculateMoveDirection(float speed)
    {
        moveDirection.x = currentMovement * -1 * speed;
        previousMovement = currentMovement;
    }

    private bool IsGrounded ()
	{
		RaycastHit hit;
        bool isGrounded;
        int layerMask = 1 << 13;

        if (Physics.Raycast (transform.position + new Vector3 (0, .1f), -transform.up, out hit, Mathf.Infinity, ~layerMask)) {
			//Debug.DrawRay (transform.position + new Vector3 (0, .1f), -transform.up * hit.distance, Color.red);
			distanceToGround = hit.distance;
			isGrounded = distanceToGround <= groundedDistance;
		}
		else {
			Debug.DrawRay (transform.position, -transform.up * 1000, Color.yellow);
			isGrounded = false;
		}
		return isGrounded;
	}

    private bool IsAtEdge()
    {
        RaycastHit hit;
        int layerMask = 1 << 13;
        bool isAtEdge=false;
        Vector3 edgeDetectorPos = transform.right * .8f;
        if (Physics.Raycast(transform.position + new Vector3(edgeDetectorPos.x, 2f), -transform.up, out hit, Mathf.Infinity, ~layerMask))
        {
            //Debug.DrawRay(transform.position + new Vector3(edgeDetectorPos.x, 2f), -transform.up * hit.distance, Color.red);
            //isAtEdge = hit.distance > 5f;
        }
        else
        {
            isAtEdge = true;
        }
        return isGrounded && isAtEdge;
    }

    private void CheckForTurn ()
	{
		if (previousMovement < 0) {
			PlayerController.transform.eulerAngles = new Vector3 (0, 0, 0);
		} else if (previousMovement > 0) {
            PlayerController.transform.eulerAngles = new Vector3 (0, 180, 0);
        }
	}
}
