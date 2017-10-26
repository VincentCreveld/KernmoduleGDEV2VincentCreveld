using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PlayerDeath();

public class PlayerController : MonoBehaviour, ICanPass {

    //Default bubbles
    public GameObject bubble;
    public Transform bubbleHolder;
    public ObjectPool<Bubble> bubblePool;

    //Sniper bubbles
    public GameObject sBubble;
    public Transform sBubbleHolder;
    public ObjectPool<Bubble> sBubblePool;

    //AR bubbles
    public GameObject arBubble;
    public Transform arBubbleHolder;
    public ObjectPool<Bubble> arBubblePool;

    public Transform shootPos;
    public GameObject muzzleFlash;
    public PlayerDeath playerDeath;

    public float fireRate;
    private float timeOfFire;

    private Vector3 left = new Vector3(-1, 0, 0);
    private Vector3 right = new Vector3(1, 0, 0);
    private bool isMoving = false;
    private float height;
    private float step = 180f;
    private Transform child;
    private Rigidbody rb;
    private Animator anim;
    private int selectedWeapon = 1;
    


    private void Awake() {
        bubblePool = new ObjectPool<Bubble>(10, bubbleHolder, bubble);
        sBubblePool = new ObjectPool<Bubble>(8, sBubbleHolder, sBubble);
        ARbubblePool = new ObjectPool<Bubble>(20, arBubbleHolder, arBubble);
        anim = transform.GetChild(0).GetComponent<Animator>();
        timeOfFire = fireRate;
    }

    // Use this for initialization
    private void Start () {
        height = GetComponent<Collider>().bounds.extents.y;
        child = transform.GetChild(0);
        rb = GetComponent<Rigidbody>();
        //InvokeRepeating("Shoot", 0, 3f);
    }

    private void Update() {
        if(Input.GetKeyDown("1")) {
            Debug.Log("Equipped Pistol");
            selectedWeapon = 1;
            fireRate = 0.5f;
        }
        else if(Input.GetKeyDown("2")) {
            Debug.Log("Equipped Sniper");
            selectedWeapon = 2;
            fireRate = 2.0f;
        }
        else if(Input.GetKeyDown("3")) {
            Debug.Log("Equipped AR");
            selectedWeapon = 3;
            fireRate = 1.0f;
        }


        if(Input.GetButtonDown("Fire1") && timeOfFire <= Time.time) {  //Shoot
            Shoot();
        }
        if(Input.GetButtonDown("Jump") && IsGrounded()) {   //Jump
            //Debug.Log("Jump!");
            rb.AddForce(Vector3.up * 11f, ForceMode.VelocityChange);
            anim.SetTrigger("Jump");
        }
        if(Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") > 0) { //Move Right
            //Debug.Log("MoveRight!");
            rb.AddForce(right * .5f, ForceMode.VelocityChange);
            child.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 0)), step);
            isMoving = true;
        }
        else if(Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") < 0) { //Move Left
            //Debug.Log("MoveLeft!");
            rb.AddForce(left * .5f, ForceMode.VelocityChange);
            child.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 180f, 0)), step);
            isMoving = true;
        }
        else {
            isMoving = false;
        }
        if(IsGrounded()) {
            CalculateDrag();
        }
        else {
            LimitSpeed();
        }

        if(isMoving) {
            anim.SetBool("IsIdle", false);
        }
        else {
            anim.SetBool("IsIdle", true);
        }
    }

    public void Die() {
        anim.SetTrigger("Die");
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        GetComponent<PlayerController>().enabled = false;
        playerDeath();
    }

    private bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, height + 0.25f);
    }

    private void Shoot() {
        switch(selectedWeapon) {
            case 1: //Pistol
                GameObject go0 = Instantiate(muzzleFlash, transform.forward, this.transform.rotation);
                go0.transform.position = shootPos.position;
                go0.transform.parent = child.GetChild(0);
                timeOfFire = Time.time + fireRate;
                Destroy(go0, 0.5f);
                DontDestroy.instance.ChangeAudioClip(DontDestroy.AudioClips.pistol);
                DontDestroy.instance.ShootWeapon();
                //Shoot bubble
                bubblePool.GetObj().InitialiseObj(child.right, child);
                anim.SetTrigger("Shoot");
                break;
            case 2: //Sniper
                GameObject go1 = Instantiate(muzzleFlash, transform.forward, this.transform.rotation);
                go1.transform.position = shootPos.position;
                go1.transform.parent = child.GetChild(0);
                timeOfFire = Time.time + fireRate;
                Destroy(go1, 0.5f);
                DontDestroy.instance.ChangeAudioClip(DontDestroy.AudioClips.sniper);
                DontDestroy.instance.ShootWeapon();
                //Shoot bubble
                SbubblePool.GetObj().InitialiseObj(child.right, child);
                anim.SetTrigger("Shoot");
                break;
            case 3: //AR burst
                
                StartCoroutine(ShootAR());
                break;
            default:
                Debug.Log("No weapon in that number!");
                break;
        }
        
    }

    private IEnumerator ShootAR() {
        for(int i = 0; i < 3; i++) {
            yield return new WaitForSeconds(0.2f);
            GameObject go = Instantiate(muzzleFlash, transform.forward, this.transform.rotation);
            go.transform.position = shootPos.position;
            go.transform.parent = child.GetChild(0);
            timeOfFire = Time.time + fireRate;
            Destroy(go, 0.5f);
            DontDestroy.instance.ChangeAudioClip(DontDestroy.AudioClips.ar);
            DontDestroy.instance.ShootWeapon();
            //Shoot
            ARbubblePool.GetObj().InitialiseObj(child.right, child);
            anim.SetTrigger("Shoot");
        }
    }

    private void LimitSpeed() {
        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -4, 4), rb.velocity.y, Mathf.Clamp(rb.velocity.z, -4, 4));
    }

    private void CalculateDrag() {
        LimitSpeed();
        rb.AddForce(rb.velocity * -0.65f, ForceMode.Impulse);
    }
}
