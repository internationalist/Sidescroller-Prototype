using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGroundSound : MonoBehaviour {


    int lastClip = -1;

    public AudioClip GetClipSound(int n = -1)
    {
        // Check two edge cases
        if (GameManager.Clips == null || GameManager.Clips.Count == 0)
        {
            return null;
        }
        if (GameManager.Clips.Count == 1)
        {
            return GameManager.Clips[0];
        }
        // If n was not specified or was not valid, randomly choose a clip
        if (n < 0 || n >= GameManager.Clips.Count)
        {
            // Choose a random clip and try to make it not the same clip as last time
            do
            {
                n = Random.Range(0, GameManager.Clips.Count);
            } while (n == lastClip);
        }
        lastClip = n;
        return GameManager.Clips[n];
    }

    public AudioClip GetClip()
    {
/*        if (footstepsSO == null)
        {
            return null;
        }*/
        return GetClipSound();
    }
}
