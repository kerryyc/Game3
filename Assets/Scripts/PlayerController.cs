using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float health = 10f;
    public float speed = 5f;

    [HideInInspector] public bool attack = false;

    //direction variables
    private int DOWN = 1;
    private int UP = 2;
    private int LEFT = 3;
    private int RIGHT = 4;
    private int direction = 1;

    //damage over time variables
    private float lastDamageTime = 0f;
    public float damagePeriod = 0.5f;

    //private component variables
    private Rigidbody2D rb2d;
    private Animator anim;
    private SpriteRenderer spriteRend;

    //variables to enable blinking of sprite
    private float spriteBlinkingTimer = 0.0f;
    private float spriteBlinkingMiniDuration = 0.1f;
    [HideInInspector] public float spriteBlinkingTotalTimer = 0.0f;
    private float spriteBlinkingTotalDuration = 1.0f;
    [HideInInspector] public bool startBlinking = false;

    void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    void Update() {
        //when health reaches 0
        if (health <= 0) {
            //subtract alive player count
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
                enemy.GetComponent<Enemy>().alivePlayers -= 1;
            //deactive player
            this.gameObject.SetActive(false);
            return;
        }

        Move();
        Attack();

        if (startBlinking)
            SpriteBlinkingEffect();
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if (!attack && coll.gameObject.tag == "Enemy") {
            startBlinking = true; //start blinking effect
            health -= 1; //decrement health
            spriteBlinkingTotalTimer = 0f; //reset blinking timer
        }
    }

    void OnCollisionStay2D(Collision2D coll) {
        if(Time.time - lastDamageTime >= damagePeriod && coll.gameObject.tag == "Enemy") {
            startBlinking = true; //start blinking effect
            health -= 1; //decrement health
            spriteBlinkingTotalTimer = 0f; //reset blinking timer
            lastDamageTime = Time.time;
        }
    }

    private void Attack() {
        if (Input.GetButtonDown("Fire1")) {
            attack = true;
            anim.SetTrigger("attack");
            Invoke("DisableAttack", 0.2f); //allow player invulnerability while attacking, disable in 0.2f
        }
    }

    private void Move() {
        //get directional input
        float horizontal = Input.GetAxisRaw("Horizontal1") * speed;
        float vertical = Input.GetAxisRaw("Vertical1") * speed;

        //set layer depending on direction
        SetLayer(direction, 0f); //set current layer weight to zero
        if (horizontal > 0)
        {
            direction = RIGHT;

        }
         
        else if (horizontal < 0)
            direction = LEFT;
        else if (vertical > 0)
            direction = UP;
        else if (vertical < 0)
            direction = DOWN;
        SetLayer(direction, 1f); //set new layer weight to one

        //set move animation
        if (horizontal != 0 || vertical != 0)
            anim.SetBool("move", true);
        else
            anim.SetBool("move", false);

        //make player move
    
        rb2d.velocity = new Vector2(horizontal,vertical);
    }

    private void SetLayer(int dir, float weight) {
        //gets layer name and then sets the layer to specified weight
        string layerName = SetLayerName(dir);
        anim.SetLayerWeight(anim.GetLayerIndex(layerName), weight);
    }

    private string SetLayerName(int dir) {
        //get layer name depending on direction
        string layerName = "Base Layer";
        if (direction == UP)
            layerName = "Up Layer";
        else if (direction == DOWN)
            layerName = "Base Layer";
        else if (direction == RIGHT)
            layerName = "Right Layer";
        else if (direction == LEFT)
            layerName = "Left Layer";
        return layerName;
    }

    private void SpriteBlinkingEffect() {
        //turns on and off sprite renderer to create blinking effect
        spriteBlinkingTotalTimer += Time.deltaTime;
        if (spriteBlinkingTotalTimer >= spriteBlinkingTotalDuration) {
            startBlinking = false;
            spriteBlinkingTotalTimer = 0.0f;
            spriteRend.enabled = true;
            return;
        }

        spriteBlinkingTimer += Time.deltaTime;
        if (spriteBlinkingTimer >= spriteBlinkingMiniDuration) {
            spriteBlinkingTimer = 0.0f;
            if (spriteRend.enabled)
                spriteRend.enabled = false;
            else
                spriteRend.enabled = true;
        }
    }

    private void DisableAttack() {
        attack = false;
    }
}
