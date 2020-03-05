using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSceneTrigger : MonoBehaviour
{
    private bool loading = false;
    private bool fading = false;
    FadeOutAndIn faoi;

    void OnTriggerEnter(Collider other)
    {
        if (!loading)
        {
            loading = true;
        }
    }

    private void Start()
    {
        faoi = GetComponent<FadeOutAndIn>();
    }

    private void Update()
    {
        if (loading && !fading)
        {
            if (faoi.FadeIn())
            {
                fading = true;
                StartCoroutine(SceneLoader.LoadNextScene());
            }
        }
    }

}
