using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader _S;
    public bool introScene;
    public static SceneLoader S
    {
        get
        {
            if (_S == null)
            {
                Debug.LogError("Attempt to access GameManager singleton before initialization");
            }
            return _S;
        }
        set
        {
            if (_S != null)
            {
                Debug.LogError("Redundant attempt to create already initialized singleton. ");
            }
            else
            {
                _S = value;
            }
        }
    }

    public void Awake()
    {
        _S = this;
    }
    private bool loading = false;
    public Image loadingBar;

    public static IEnumerator LoadNextScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        return S.LoadScene(sceneIndex);
    }
    

    public static IEnumerator LoadIntroScene()
    {
        return S.LoadScene(0);
    }

    public static IEnumerator LoadCurrentScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        return S.LoadScene(sceneIndex);
    }

    public static void LoadCurrentSceneSync()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }

    private IEnumerator LoadScene(int sceneIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            if (S.loadingBar)
            {
                S.loadingBar.fillAmount = asyncLoad.progress * 100;
            }
            yield return null;
        }
    }
}
