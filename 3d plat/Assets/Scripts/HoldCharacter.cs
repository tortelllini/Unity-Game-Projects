using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldCharacter : MonoBehaviour {

    void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag == "Player") {
            other.gameObject.transform.parent = gameObject.transform;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            other.gameObject.transform.parent = null;
        }
    }

}
