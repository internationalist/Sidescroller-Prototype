using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour {
	private static GameManager _S;
	//private Basic2DMovement _player;
    private Dictionary<GameObject, Basic2DMovement> humanoidEntityCache;
    private bool _paused = false;
    public bool gameOver;
    public GameObject menupanel;
    public GameObject controlpanel;
    private Text gameStateText;
    public int actionRegister=0;
    public AudioMixerGroup combatSound;
    public AudioMixerGroup damageSound;
    public AudioClip youLoseSound;
    private AudioSource ambientSoundSource;
    private bool combatSoundPlaying;
    private bool damageSoundPlaying;
    public List<AudioClip> clips = new List<AudioClip>();
    public static GameManager S {
		get {
			if (_S == null) {
				Debug.LogError ("Attempt to access GameManager singleton before initialization");
			}
			return _S;
		}
		set { 
			if (_S != null) {
				Debug.LogError ("Redundant attempt to create already initialized singleton. ");
			} else {
				_S = value;				
			}
		}
	}

	void Awake () {
        this.humanoidEntityCache = new Dictionary<GameObject, Basic2DMovement>();
		_S = this;
        gameStateText = menupanel.GetComponentInChildren<Text>();
        ambientSoundSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(!gameOver && Input.GetButtonDown("Cancel"))
        {
            if(_paused)
            {
                menupanel.SetActive(false);
                _paused = false;
                Time.timeScale = 1;
            } else
            {
                gameStateText.text = "Paused";
                menupanel.SetActive(true);
                _paused = true;
                Time.timeScale = 0;
            }
        }
    }

    public static void GameOver(float delay)
    {
        S.StartCoroutine(GameManager.FinishGame(delay));
    }

    private static IEnumerator FinishGame(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        S.gameStateText.text = "You Died";
        S.menupanel.SetActive(true);
        S._paused = true;
        Time.timeScale = 0;
        S.gameOver = true;
    }

    public static void RegisterHumanoidEntity(GameObject gObj, Basic2DMovement movementData) {
        if(!S.humanoidEntityCache.ContainsKey(gObj))
        {
            S.humanoidEntityCache.Add(gObj, movementData);
        }
    }

	public static bool IsAtacking(GameObject entityKey) {
        Basic2DMovement entityData = S.humanoidEntityCache[entityKey];
        return entityData.IsAttacking;		
	}
    /// <summary>
    /// Utility function that returns a hit object if enemy is found.
    /// The hit object contains information regarding the distance as well as the enemy game object.
    /// If no object is found then the function returns a hit object with distance of -1
    /// </summary>
    /// <param name="enemyLayer"></param>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static RaycastHit GetDistanceToEnemyAtFront(int enemyLayer, Transform transform)
    {
        RaycastHit hit;
        int layerMask = 1 << enemyLayer;
        if(!Physics.Raycast(transform.position + new Vector3(0, 1f), transform.right, out hit, Mathf.Infinity, layerMask))
        {
            hit.distance = -1;
        }
        return hit;
    }

    /// <summary>
    /// Utility function that returns a hit object if enemy is found.
    /// The hit object contains information regarding the distance as well as the enemy game object.
    /// If no object is found then the function returns a hit object with distance of -1
    /// </summary>
    /// <param name="enemyLayer"></param>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static RaycastHit GetDistanceToEnemyAtBack(int enemyLayer, Transform transform)
    {
        RaycastHit hit;
        int layerMask = 1 << enemyLayer;
        if (!Physics.Raycast(transform.position + new Vector3(0, 1f), -1*transform.right, out hit, Mathf.Infinity, layerMask))
        {
            hit.distance = -1;
        }
        return hit;
    }

    public void Replay()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ShowControls()
    {
        controlpanel.SetActive(true);
    }

    public void HideControls()
    {
        controlpanel.SetActive(false);
    }

    public static void PlayAttackSound(AudioClip clip, AudioSource audioSource)
    {
        if (clip != null && !S.combatSoundPlaying)
        {
            S.combatSoundPlaying = true;
            audioSource.outputAudioMixerGroup = S.combatSound;
            audioSource.PlayOneShot(clip);
            S.combatSoundPlaying = false;
        }
    }
    public static void PlayDamageSound(AudioClip clip, AudioSource audioSource)
    {
        if (clip != null && !S.damageSoundPlaying)
        {
            S.damageSoundPlaying = true;
            audioSource.outputAudioMixerGroup = S.damageSound;
            audioSource.PlayOneShot(clip);
            S.damageSoundPlaying = false;
        }
    }

    public static void OnDeathSound()
    {
        S.ambientSoundSource.clip = S.youLoseSound;
        S.ambientSoundSource.Play();
    
    }

    public static List<AudioClip> Clips {
        get
        {
            return _S.clips;
        }
    }
}
