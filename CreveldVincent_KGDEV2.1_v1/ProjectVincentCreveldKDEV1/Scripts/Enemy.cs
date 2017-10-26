using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IBubblable {

    public bool isMovingRight;
    public bool isTrapped;
    public int maxTeleportCount = 3;
    public Transform sprite;
    public delegate void EnemyEvent();
    public EnemyEvent enemyDeath;
    public EnemyEvent enrageEvent;


    private Rigidbody rb;
    private Animator anim;
    private Transform childOfChild;
    private float height;
    private int teleportCount = 0;
    private int speedLimit = 1;

    private void Awake() {
        //sprite = transform.GetChild(0);
        childOfChild = sprite.GetChild(0);
        anim = sprite.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        height = GetComponent<Collider>().bounds.extents.y;
        if(isMovingRight)
            sprite.localScale = new Vector3(-sprite.localScale.x, sprite.localScale.y, sprite.localScale.z);
        else
            sprite.localScale = new Vector3(sprite.localScale.x, sprite.localScale.y, sprite.localScale.z);

        GameManager.instance.player.playerDeath += CeaseActivity;
        enemyDeath += Die;
        enrageEvent += Enrage;
        //sprite.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Update() {
        if(!isTrapped) {
            if(isMovingRight) { //Move Right
                                //Debug.Log("MoveRight!" + transform.name);
                rb.AddForce(Vector3.right * .5f, ForceMode.VelocityChange);
                anim.SetBool("isWalking", true);
            }
            else if(!isMovingRight) { //Move Left
                                      //Debug.Log("MoveLeft!" + transform.name);
                rb.AddForce(Vector3.left * .5f, ForceMode.VelocityChange);
                anim.SetBool("isWalking", true);
            }
            else {
                anim.SetBool("isWalking", false);
            }
        }
        else {
            anim.SetBool("Bubbled", true);
            rb.AddForce(Vector3.up);
        }

        if(IsGrounded()) {
            CalculateDrag();
        }
        else {
            LimitSpeed();
        }
    }

    public void OnCollisionEnter(Collision collision) {
        if(collision.transform.tag == "LevelBorder") {
            isMovingRight = !isMovingRight;
            sprite.localScale = new Vector3(-sprite.localScale.x, sprite.localScale.y, sprite.localScale.z);
        }
        if(collision.transform.tag == "Bubble") {
            GetTrapped();
        }
        if(collision.transform.tag == "Player") {
            if(!isTrapped) {
                Attack(collision.transform);
            }
            else {
                enemyDeath();
            }
        }
    }

    //IBubblable interface members
    public void GetTrapped() {
        isTrapped = true;
        childOfChild.gameObject.SetActive(true);
        rb.useGravity = false;
        Invoke("Release", 10f);
    }

    public void Release() {
        isTrapped = false;
        anim.SetBool("Bubbled", false);
        childOfChild.gameObject.SetActive(false);
        rb.useGravity = true;
        Enrage();
    }

    public void Attack(Transform other) {
        anim.SetTrigger("Attack");
        other.GetComponent<PlayerController>().Die();
    }

    public void Die() {
        childOfChild.gameObject.SetActive(false);
        CancelInvoke("Release");
        GetComponent<Collider>().enabled = false;
        anim.SetTrigger("Die");
        GameManager.instance.player.playerDeath -= CeaseActivity;
        Destroy(gameObject, 2f);
    }

    //This function tracks whether or not the enemy has gone off screen too often and if it has, it changes the zombie
    public void AddTeleportCount() {
        teleportCount++;
        if(teleportCount >= maxTeleportCount) {
            Enrage();
        }
    }

    private void Enrage() {
        Debug.Log("Enrage");
        speedLimit = 2;
        Debug.Log(sprite.name);
        sprite.GetComponent<SpriteRenderer>().material.color = Color.red;
    }

    //Returns whether or not the unit is grounded
    private bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, height + 0.25f);
    }

    //Movementspeed limiting functions
    private void LimitSpeed() {
        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -speedLimit, speedLimit), rb.velocity.y, Mathf.Clamp(rb.velocity.z, -speedLimit, speedLimit));
    }

    private void CalculateDrag() {
        LimitSpeed();
        rb.AddForce(rb.velocity * -0.8f, ForceMode.Impulse);
    }

    //Member of event when player dies, stops all movement
    private void CeaseActivity() {
        anim.SetBool("isWalking", false);
        GetComponent<Enemy>().enabled = false;
    }
}
