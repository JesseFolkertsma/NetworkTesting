using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    Behaviour[] compToDisable;

    [SerializeField]
    Camera mainCamera;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            for (int i = 0; i < compToDisable.Length; i++)
            {
                compToDisable[i].enabled = false;
            }
        }

        else
        {
            mainCamera = Camera.main;
            if (mainCamera != null)
                mainCamera.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (mainCamera != null)
            mainCamera.gameObject.SetActive(true);
    }
}
