using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBlock : MonoBehaviour {

    private GameObject partner;

	void Awake() {
        if(transform.GetChild(0) != false)
            partner = transform.GetChild(0).gameObject;
        else
            Debug.Log("No child found!");
    }


    private void OnTriggerStay(Collider other) {
        other.transform.position = partner.transform.position;

        if(other.GetComponent<Rigidbody>() != null) {
            other.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }

        if(other.GetComponent<Enemy>() != null) {
            other.GetComponent<Enemy>().AddTeleportCount();
        }
    }
}
