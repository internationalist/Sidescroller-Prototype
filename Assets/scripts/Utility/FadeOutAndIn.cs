using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutAndIn : MonoBehaviour
{
    public GameObject fadePanel;
    private Image fadePanelImg;
    public float fadeDuration;
    private bool fadeActive=false;
    private float fadeStartTime;
    private float fadePercent;

    private void Start()
    {
        fadePanelImg = fadePanel.GetComponent<Image>();
    }



    public bool FadeIn()
    {
        if(!fadeActive)
        {
            fadeStartTime = Time.time;
            fadePanel.SetActive(true);
        }
        fadeActive = true;

        float timeElapsedSoFar = Time.time - fadeStartTime;

        if (timeElapsedSoFar <= fadeDuration)
        {
            fadePercent = (timeElapsedSoFar / fadeDuration);
            Color fadeCol = fadePanelImg.color;
            fadeCol.a = fadePercent;
            fadePanelImg.color = fadeCol;
            return false;
        } else
        {
            return true;
        }
    }




}
