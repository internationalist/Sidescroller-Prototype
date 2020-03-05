using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSceneLauncher : MonoBehaviour
{
    private bool loading = false;
    private bool fading = false;
    FadeOutAndIn faoi;

    private void Start()
    {
        faoi = GetComponent<FadeOutAndIn>();
    }


    void Update()
    {
        if (Input.anyKeyDown)
        {
            loading = true;
        }
        if (loading && !fading)
        {
            if(faoi.FadeIn())
            {
                fading = true;
                StartCoroutine(SceneLoader.LoadNextScene());
            }
        }
    }
}
