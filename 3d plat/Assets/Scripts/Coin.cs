using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    public Transform coinEffect;
    public PlayerController playController;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Transform effect = Instantiate(coinEffect, transform.position, transform.rotation) as Transform;
            Destroy(effect.gameObject, 3);

            Destroy(gameObject);

            playController.winCount += 1;
        }
    }
}
