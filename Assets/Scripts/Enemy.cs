using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : MonoBehaviour {

    public float health = 1f;
    public float speed = 2f;
    public float knockback = 250f;

    private float damageCoolDown = 1f;
    private bool doKnockback = false;

    private int numPlayers = 2;
    private GameObject[] allPlayers;
    private GameObject player;
    private Rigidbody2D rb2d;

    void Awake() {
        allPlayers = GameObject.FindGameObjectsWithTag("Player");
        player = GameObject.FindGameObjectWithTag("Player");
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update() {
        //if not being knocked back, erase all forces
        if (!doKnockback)
            rb2d.velocity = new Vector2(0, 0);

        //when health is 0
        if (health <= 0)
            Destroy(this.gameObject);

        //checks for player that is closest to enemy (might need to be optimized)
        int minIndex = 0;
        float minDistance = float.MaxValue;
        for (int i = 0; i < numPlayers; ++i) {
            float distance = Vector2.Distance(allPlayers[i].transform.position, transform.position);
            if (distance < minDistance) {
                minIndex = i;
                minDistance = distance;
            }
        }
        player = allPlayers[minIndex];

        //moves toward player until it reaches certain distance
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D other) {
        //only take damage if player is attacking, otherwise player is damaged
        if (other.gameObject.tag == "Player") {
            //TODO: optimize script to allow for ease of changes if adding more players
            if ((other.gameObject.GetComponent<PlayerController>() != null && other.gameObject.GetComponent<PlayerController>().attack) ||
                (other.gameObject.GetComponent<Player2Controller>() != null && other.gameObject.GetComponent<Player2Controller>().attack)) {
                --health;
                doKnockback = true;
                //allow for enemies to get knocked back upon getting hit
                var force = transform.position - other.transform.position;
                force.Normalize();
                rb2d.AddForce(force * knockback);
                Invoke("StopForce", 0.2f);
            }
        }
    }

    private void StopForce() {
        //stop knockback force from being applied forever
        var force = transform.position - player.transform.position;
        force.Normalize();
        rb2d.AddForce(-force * knockback);
        doKnockback = false;
    }

    private void damagePlayer() {
        if (Time.time > damageCoolDown) {
            player.GetComponent<PlayerController>().health--; //subtract health
            player.GetComponent<PlayerController>().startBlinking = true; //begin sprite blinking
            damageCoolDown += Time.time; //reset cooldown
        }

    }
}
