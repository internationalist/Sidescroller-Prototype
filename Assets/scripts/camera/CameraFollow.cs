using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    private GameObject player;
    public float minXPosition;
    private float yDelta;
    public float maxYDelta = 3.6f;
    private bool startLerp = false;
    float fractionOfJourney = 0f;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        yDelta = Mathf.Abs(transform.position.y - player.transform.position.y);
    }

    // Use this for initialalled every frame, if the Behaviour is enabled
    private void LateUpdate()
    {
        if (player.transform.position.x > minXPosition) {
            Vector3 position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
            transform.position = position;
        }
        if(Mathf.Abs(transform.position.y - player.transform.position.y) > maxYDelta || startLerp)
        {
            if(!startLerp)
            {
                startLerp = true;
            }

            Vector3 target = new Vector3(transform.position.x, player.transform.position.y + yDelta, transform.position.z);
            fractionOfJourney += .01f;
            transform.position = Vector3.Lerp(transform.position, target, fractionOfJourney);
            if(fractionOfJourney >= 1)
            {
                startLerp = false;
                fractionOfJourney = 0;
            }
        }
    }


}
