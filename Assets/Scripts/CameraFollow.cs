using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject player;

    // Update is called once per frame
    void LateUpdate()
    {
        player ??= GameObject.FindWithTag("Player"); //if player is null assign the player object to player

        if (player is not null) OffsetCamera();
    }

    private void OffsetCamera()
    {
        Vector3 centerOfScreen = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 distanceFromCenter = centerOfScreen - Input.mousePosition; //mouse distance from center of screen

        float offsetFactorX = -150f;
        float offsetFactorY = -100f;

        Vector3 playerPos = player.transform.position;

        float offsetX = distanceFromCenter.x / offsetFactorX;
        float offsetY = distanceFromCenter.y / offsetFactorY;

        //move camera to players position + offset based on where the mouse is relative to the center of the screen
        transform.position = new Vector3(playerPos.x + offsetX, playerPos.y + offsetY, transform.position.z); 
    }
}