using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARBubble : Bubble {

    protected override void Move() {
        if(rb.velocity.x <= 0.5f && rb.velocity.x >= -0.5f) {
            rb.velocity = new Vector3(Mathf.Lerp(rb.velocity.x, 0, 0.002f), rb.velocity.y, rb.velocity.z);  //new Vector3(0,rb.velocity.y, 
            StartCoroutine(recycleBubble());
        }
        else
            rb.AddForce(-rb.velocity / 8f, ForceMode.Impulse);
    }
}
