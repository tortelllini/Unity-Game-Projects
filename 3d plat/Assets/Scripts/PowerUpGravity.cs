using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpGravity : MonoBehaviour {

    public PlayerController playController;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //gameObject.SetActive(false);     
                   
            playController.gravity = -6;
        }
    }
}
