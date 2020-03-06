using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Reference to itself
    Camera cam;
    //For keeping track of where the start and exit rooms spawn
    public float lerpSpd;
    RoomTemplates template;
    Transform targ1;
    Transform targ2;

    //For following the player
    public bool followPlayer = false;
    Transform playerInstance;
    public float cameraSize;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        template = FindObjectOfType<RoomTemplates>();
    }

    private void FixedUpdate()
    {
        if (!followPlayer && template.rooms.Count > 2)
        {
            targ1 = template.rooms[0].transform;
            targ2 = template.rooms[template.rooms.Count - 1].transform;
            transform.position = Vector3.Lerp(transform.position, new Vector3((targ1.position.x + targ2.position.x) / 2f, (targ1.position.y + targ2.position.y / 2f), -10f), lerpSpd * Time.deltaTime);
        }

        if (followPlayer)
        {
            if (playerInstance == null) playerInstance = FindObjectOfType<PlayerController>().transform;
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, cameraSize, Time.deltaTime * lerpSpd);
            transform.position = Vector3.Lerp(transform.position, new Vector3(playerInstance.transform.position.x, playerInstance.transform.position.y, -10f), Time.deltaTime * lerpSpd);
        }
    }

    public void StartFollowingPlayer()
    {
        followPlayer = true;
    }
}
