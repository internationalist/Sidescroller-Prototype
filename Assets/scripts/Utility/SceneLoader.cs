using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    private bool loading = false;
    public Image loadingBar;
    // Update is called once per frame
    void Update()
    {
       if(Input.anyKeyDown && !loading)
        {
            loading = true;
            StartCoroutine(LoadGameScene());
        }
    }

    public IEnumerator LoadGameScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            loadingBar.fillAmount = asyncLoad.progress * 100;
            yield return null;
        }
    }
}
