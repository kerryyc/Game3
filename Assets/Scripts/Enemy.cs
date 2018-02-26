using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float health = 1f;
    public float speed = 2f;
    public float knockback = 250f;

    private float damageCoolDown = 1f;
    private GameObject player;
    private Rigidbody2D rb2d;

    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (health <= 0)
            Destroy(this.gameObject);

        Vector3 detectDistance = transform.position - player.transform.position;
        //Debug.Log(detectDistance);
        //moves toward player until it reaches certain distance
        if (Mathf.Abs(detectDistance.x) > 1 || Mathf.Abs(detectDistance.y) > 1.4) {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else {
            //otherwise remove all forces from object and begin damaging player
            rb2d.velocity = new Vector2(0, 0);
            damagePlayer();
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        //only take damage if player is attacking, otherwise player is damaged
        if (other.gameObject.tag == "Player" && player.GetComponent<PlayerController>().attack) {
            --health;
            //allow for enemies to get knocked back upon getting hit
            var force = transform.position - other.transform.position;
            force.Normalize();
            rb2d.AddForce(force * knockback);
            Invoke("StopForce", 0.2f);
        }
        else if (other.gameObject.tag == "Player") {
            //stop moving if collided with idle player (currently not working)
            rb2d.velocity = new Vector2(0, 0);
        }
    }

    private void StopForce() {
        //stop knockback force from being applied forever
        var force = transform.position - player.transform.position;
        force.Normalize();
        rb2d.AddForce(-force * knockback);
    }

    private void damagePlayer() {
        if (Time.time > damageCoolDown) {
            player.GetComponent<PlayerController>().health--; //subtract health
            player.GetComponent<PlayerController>().startBlinking = true; //begin sprite blinking
            damageCoolDown += Time.time; //reset cooldown
        }

    }
}
