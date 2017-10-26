using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBubble : Bubble {

    public override void OnCollisionEnter(Collision collision) {
        if(collision.transform.tag == "Enemy") {
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.transform.GetComponent<Collider>());
            rb.AddForce(moveDir * 20, ForceMode.Impulse);
        }
        else if(collision.gameObject.layer != 8 && collision.transform.tag != "Enemy")
            Recycle();
    }
    protected override void Move() {
        //No slowing down required here!
    }
}
