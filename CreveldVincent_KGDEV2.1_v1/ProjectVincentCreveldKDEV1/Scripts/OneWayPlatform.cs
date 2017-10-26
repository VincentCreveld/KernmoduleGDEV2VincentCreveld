using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour {

    private Collider platform;

    private void Awake() {
        platform = gameObject.transform.GetChild(0).gameObject.GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other) {
        Physics.IgnoreCollision(platform, other, true);
    }

    private void OnTriggerExit(Collider other) {
        //StartCoroutine(DelayEnable(other));
        Physics.IgnoreCollision(platform, other, false);
    }

    private IEnumerator DelayEnable(Collider other) {
        yield return new WaitForSeconds(0.5f);
        Physics.IgnoreCollision(platform, other, false);
    }

}
