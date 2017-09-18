﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerController))]
public class PlayerInteraction : NetworkBehaviour {
    [SerializeField]
    Camera playerCam;

    public float interactRange = 3;
    public float pushForce = 300;
    public LayerMask remoteplayerLayer;
    public LayerMask pickupLayer;

    private void Update()
    {
        if (Input.GetButtonDown("Push"))
        {
            Push();
        }
    }

    void Push()
    {
        Debug.Log("Push dis nibba");
        RaycastHit hit;
        if(Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, interactRange, remoteplayerLayer))
        {
            CmdPushPlayer(hit.transform.name);
        }
    }

    [Command]
    void CmdPushPlayer(string playerID)
    {
        GameManager.GetPlayer(playerID).GetForce((playerCam.transform.forward + playerCam.transform.up) * 10000);
    }
}
