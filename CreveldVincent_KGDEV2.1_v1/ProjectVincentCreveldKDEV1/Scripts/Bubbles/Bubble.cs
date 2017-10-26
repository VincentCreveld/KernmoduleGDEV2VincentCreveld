using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bubble : MonoBehaviour, IPoolable {

    public PoolEvent PoolEvent {
        get { return recycleEvent; }
        set { recycleEvent = value; }
    }
    protected Rigidbody rb;
    protected Vector3 moveDir;
    private PoolEvent recycleEvent;
    private bool calledRecycle = false;
    

    private void Awake() {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = false;
        
        gameObject.SetActive(false);
        //rb.velocity = rb.GetComponent<ConstantForce>().force = moveDir;
    }

    private void Start() {
        transform.position = transform.parent.position;
    }

    public void FixedUpdate() {
        Move();
    }


    public virtual void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.layer != 8)
            Recycle();
    }

    public void InitialiseObj(Vector3 v3, Transform t) {
        moveDir = v3;
        transform.rotation = t.rotation;
        transform.position = t.position + t.right;
        rb.AddForce(moveDir * 20, ForceMode.Impulse);
    }

    public void Recycle() {
        if(recycleEvent != null)
            recycleEvent(gameObject);
    }

    protected virtual void Move() {
        if(rb.velocity.x <= 0.5f && rb.velocity.x >= -0.5f) {
            rb.velocity = new Vector3(Mathf.Lerp(rb.velocity.x, 0, 0.002f), rb.velocity.y, rb.velocity.z);  //new Vector3(0,rb.velocity.y, 
            StartCoroutine(recycleBubble());
        }
        else
            rb.AddForce(-moveDir * 2, ForceMode.Impulse);
    }

    protected IEnumerator recycleBubble() {
        if(!calledRecycle) {
            rb.AddForce(Vector3.up, ForceMode.Impulse);
            calledRecycle = true;
        }
        yield return new WaitForSeconds(1f);
        calledRecycle = false;
        Recycle();
    }
}
