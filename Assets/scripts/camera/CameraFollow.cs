﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    private GameObject player;
    [Header("Generated")]
    [SerializeField]
    private float minXPosition;
    [SerializeField]
    private float maxXPosition;
    [SerializeField]
    private float playerMovementField;
    private float yDelta;
    //public float maxYDelta = 3.6f;
    private bool startLerpOnY = false;
    private bool catchupWithPlayerOnX = false;
    float fractionOfJourney = 0f;
    [Header("Derclared")]
    public float worldXStart;
    public float worldXLength;
    private float cameraFieldWidth;
    Camera thisCamera;
    private float cachedAspect;

    private void Awake()
    {
        SetupCamera();
    }

    private void SetupCamera()
    {
        player = GameObject.FindWithTag("Player");
        if(player)
        {
            yDelta = Mathf.Abs(transform.position.y - player.transform.position.y);
        }
        thisCamera = GetComponent<Camera>();
        cameraFieldWidth = thisCamera.orthographicSize * thisCamera.aspect;
        Vector3 position = transform.position;
        position.x = worldXStart + cameraFieldWidth;
        transform.position = position;
        minXPosition = position.x;
        maxXPosition = worldXStart + worldXLength - cameraFieldWidth;
        playerMovementField = cameraFieldWidth / 3;
        this.cachedAspect = thisCamera.aspect;
    }

    // Use this for initialalled every frame, if the Behaviour is enabled
    private void LateUpdate()
    {
        if (player)
        {
            if (thisCamera.aspect != cachedAspect)
            {
                SetupCamera();
            }
            float delta = Mathf.Abs(player.transform.position.x - transform.position.x);
            if (delta > playerMovementField)
            {
                if (player.transform.position.x < transform.position.x && transform.position.x > minXPosition)
                {//move towards left
                    Vector3 newPosition = transform.position;
                    newPosition.x -= delta - playerMovementField;
                    transform.position = newPosition;
                }
                else if (player.transform.position.x > transform.position.x && transform.position.x < maxXPosition)
                {//move towards right
                    Vector3 newPosition = transform.position;
                    newPosition.x += delta - playerMovementField;
                    transform.position = newPosition;
                }
            }
        }
    }

    private void FollowOnXDirection()
    {
        if (catchupWithPlayerOnX)
        {
            return;
        }

        Vector3 target = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        fractionOfJourney += .0001f;
        transform.position = Vector3.Lerp(transform.position, target, fractionOfJourney);
        if (fractionOfJourney >= 1)
        {
            catchupWithPlayerOnX = true;
            fractionOfJourney = 0;
        }
    }

    private void FollowOnYDirection()
    {
        if (!startLerpOnY)
        {
            startLerpOnY = true;
        }

        Vector3 target = new Vector3(transform.position.x, player.transform.position.y + yDelta, transform.position.z);
        fractionOfJourney += .01f;
        transform.position = Vector3.Lerp(transform.position, target, fractionOfJourney);
        if (fractionOfJourney >= 1)
        {
            startLerpOnY = false;
            fractionOfJourney = 0;
        }
    }
}
