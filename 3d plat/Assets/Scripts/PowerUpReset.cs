using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpReset : MonoBehaviour {

    public PlayerController playController;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //other.gameObject.SetActive(false);
            playController.jumpHeight = 1;
            playController.walkSpeed = 2;
            playController.runSpeed = 6;
            playController.gravity = -12;
        }
    }
}
