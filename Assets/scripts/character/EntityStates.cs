using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <para>State class that contains all the state variables that power a character.</para>
/// <para>Primary implementation is provided by Basic2DMovement.</para>
/// </summary>
public class EntityStates : MonoBehaviour {
    [SerializeField]
    public bool isAttacking;

    [Header("Inscribed")]
    public float moveSpeed;
    public float jumpSpeed;
    public float gravity;
    public float groundedDistance;
    public float defaultControllerHeight;
    public float defaultControlleryPos;
    public float retreatSpeed;
    public int enemyLayer;
    public float attackRange;
    public float spotRange;
    public float aiDashRange;
    public float throwRange;
    public float minDistance;
    public int lightAttack;
    public int heavyAttack;
    public int guardBreak;
    public int maxHealth;
    public int maxStamina;
    public float staminaGainPerSecond;
    public float distanceToEnemy;
    public bool enemyFound;
    public string id;
    public bool isGrounded;
    public bool isOnEdge;

    [Header("Dynamic")]
    [SerializeField]
    public Vector3 moveDirection = Vector3.zero;
    public float currentMovement;

    public Image healthBar;
    public Image staminaBar;

    public ParticleSystem trails;
    public AudioClip deathSound;
    public AudioSource[] audioSources;
    public float health;
    public float stamina;

    public int lightAttackStamina = 35;
    public int heavyAttackStamina = 40;
    public int blockAttackStamina = 5;
    public float blockStaminaPenalty = 10;
    public int minimumStamina = 5;
    public int guardBreakStamina = 5;


    public AnimationClip[] kicks;
    public AnimationClip[] punches;
    public AnimationClip[] deaths;


    public ParticleSystem outOfStamina;

    /* Combat effects */
    public ParticleSystem lightHitEffect;
    public ParticleSystem heavyHitEffect;
    public ParticleSystem blockEffect;
    public ParticleSystem deadEffect;
    public ParticleSystem guardBreakEffect;
    public ParticleSystem[] deathEffects;

    public AudioClip kicksound;
    public AudioClip punchsound;
    public AudioClip hurtsound;

    public AudioClip blockSound;

    public string[] impactText = { "BAM!", "BIFF!", "BANG!", "POW!", "WHAAM!", "BAMF!", "BOOM!", "KRUNK!", "THOK!", "BLAP!" };

    public PlayerState playerState;

    public bool animMotion = false;

    private EntityStates opponent;
    private bool kickCombo;
    private bool punchCombo;


    //Player action states
    public enum PlayerState
    {
        idle, movement, lightattack, heavyattack, jump, airborne, crouch, retreat, attacked, death, block, attackblocked, stunned,outofstamina,onedge, dash, guardbreak, throwobject,forwarddash
    }

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

    public bool KickCombo
    {
        get
        {
            return kickCombo;
        }

        set
        {
            kickCombo = value;
        }
    }

    public bool PunchCombo
    {
        get
        {
            return punchCombo;
        }

        set
        {
            punchCombo = value;
        }
    }

    public EntityStates Opponent
    {
        get
        {
            return opponent;
        }

        set
        {
            opponent = value;
        }
    }
}
