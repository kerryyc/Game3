﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : MonoBehaviour {
    public float speed;
    public float stoppingDistance;
    public float retreatDistance;
    public float retreatSpeed = 0.5f;
    public float shootSpeed = 2f;

    private bool passedBoundary = false;

    public GameObject projectile;
    public float shotTimeInterval;
    private float coolDownTime;

    // from the Enemy script
    [HideInInspector] public bool doKnockback = false;

    private float lastDamageTime = 0f;
    public float damagePeriod = 0.5f;
    public float knockback = 250f;
    public float health = 1f;
    private float lastKnockTime = 0f;
    private float knockPeriod = 0.5f;

    private Rigidbody2D rb2d;
    private Animator anim;

    private GameObject player;
    private GameObject[] allPlayers;

    private bool retreat = false;

    //  death sound effect
    private AudioSource soundSource;
    public AudioClip enemyDeathSound;
    private bool isDeathSoundPlayed = false;

    // Use this for initialization
    void Start () {
        // at spawn, find the player with the lowest amount of health and follow this player
        allPlayers = GameObject.FindGameObjectsWithTag("Player");
        int indexPlayerLowestHP = findMinIndex();
        player = allPlayers[indexPlayerLowestHP];

        coolDownTime = shotTimeInterval;
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        soundSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        // currDistance is the distance between the player and enemy
        if (!doKnockback)
            rb2d.velocity = new Vector2(0, 0);

        //when health is 0
        if (health <= 0) {
            //trigger death animation, disable physics, then destroy
            // play death sound effect
            if (!isDeathSoundPlayed) {
                soundSource.PlayOneShot(enemyDeathSound); // play explosion
                isDeathSoundPlayed = true;
            }

            anim.Play("explosion");
            GetComponent<Collider2D>().enabled = false;
            rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            //Destroy(this.gameObject, 0.8f);
            // made time longer to account for the sound effect
            Destroy(this.gameObject, 1.4f);
        }
        else {
            float currDistance = Vector2.Distance(transform.position, player.transform.position);
            allPlayers = GameObject.FindGameObjectsWithTag("Player");
            int alivePlayers = allPlayers.Length;

            // if the player's health is not 0
            if (alivePlayers != 0) {
                if (player.GetComponent<PlayerController>().health > 0) {
                    retreat = false;
                    // if the distance between the enemy and player is greater than stopping distance
                    if (currDistance > stoppingDistance) {
                        // move towards player
                        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                    }

                    // if the distance between the enemy and player is between stopping distance and retreat distance
                    else if (currDistance < stoppingDistance &&
                        currDistance > retreatDistance) {
                        // stay still
                        transform.position = this.transform.position;
                    }
                    // if player is too close to the enemy
                    else if (currDistance < retreatDistance) {
                        // the enemy reverses its direction and retreats
                        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, -speed * retreatSpeed * Time.deltaTime);
                        retreat = true;
                    }

                }

                // if the player that the enemy is seeking dies, 
                // find another player that has the lowest hp 
                // and follow them instead
                else {
                    Debug.Log("Seeker killed player!");
                    // update the array of players
                    allPlayers = GameObject.FindGameObjectsWithTag("Player");
                    int index = findMinIndex();
                    player = allPlayers[index];
                }

                // shoot bullets
                shoot();
                updateAnimations();
            }
        }
        
	}
    void OnCollisionEnter2D(Collision2D other)
    {
        //only take damage if player is attacking, otherwise player is damaged
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<PlayerController>() != null && other.gameObject.GetComponent<PlayerController>().attack)
            {
                player = other.gameObject; //change target to player that attacked
                --health;
                lastDamageTime = Time.time; //update when enemy was last damaged
                performKnockback(other, knockback);
            }
        }

        //get knocked back if collision is an enemy getting knocked back
        if (other.gameObject.tag == "Enemy")
        {
            if (other.gameObject.GetComponent<Enemy>() != null && other.gameObject.GetComponent<Enemy>().doKnockback)
            {
                performKnockback(other, 250f);
            }
            else if (other.gameObject.GetComponent<Seeker>() != null && other.gameObject.GetComponent<Seeker>().doKnockback)
            {
                performKnockback(other, 250f);
            }
        }
        if (!passedBoundary && other.gameObject.tag == "PlayerBoundary")
        {
            // if enemy collides with player boundary, ignore collision
            Physics2D.IgnoreCollision(other.collider, this.GetComponent<Collider2D>());
        }
    }
    void OnCollisionStay2D(Collision2D other)
    {
        //if last time the enemy took damage is greater than the damage period
        if (other.gameObject.tag == "Player" && Time.time - lastDamageTime >= damagePeriod)
        {
            OnCollisionEnter2D(other);
        }

        //get knocked back if collision is an enemy getting knocked back
        if (Time.time - lastKnockTime >= knockPeriod && other.gameObject.tag == "Enemy")
        {
            if (other.gameObject.GetComponent<Enemy>() != null && other.gameObject.GetComponent<Enemy>().doKnockback)
            {
                performKnockback(other, 250f);
            }
            else if (other.gameObject.GetComponent<Seeker>() != null && other.gameObject.GetComponent<Seeker>().doKnockback)
            {
                performKnockback(other, 250f);
            }
        }


    }

    void OnCollisionExit2D(Collision2D other) 
    {
        //if it passes boundary, disable passing it
        if (other.gameObject.tag == "PlayerBoundary") {
            passedBoundary = true;
            Physics2D.IgnoreCollision(other.collider, this.GetComponent<Collider2D>(), false);
        }
    }

    private void StopForce()
    {
        //stop knockback force from being applied forever
        var force = transform.position - player.transform.position;
        force.Normalize();
        rb2d.AddForce(-force * knockback);
        doKnockback = false;
    }

    // find the player with the lowest health, and return the index
    private int findMinIndex()
    {
        int index = 0;
        float minHealth = float.MaxValue;

        for (int i = 0; i < allPlayers.Length; i++)
        {
            float curr = allPlayers[i].GetComponent<PlayerController>().health;
            if (curr <= minHealth)
            {
                minHealth = curr;
                index = i;
            }

        }
        return index;
    }

    private void shoot()
    {
        if (!retreat && coolDownTime <= 0)
        {
            
            GameObject bullet = Instantiate(projectile, transform.position, transform.rotation);
            Vector2 direction = player.transform.position - bullet.transform.position;
            direction.Normalize();
            bullet.GetComponent<Rigidbody2D>().velocity = (direction * shootSpeed);
            coolDownTime = shotTimeInterval;
        }
        else
        {
            coolDownTime -= Time.deltaTime;
        }
    }
    private void performKnockback(Collision2D other, float kForce)
    {
        doKnockback = true;
        //allow for enemies to get knocked back upon getting hit
        var force = transform.position - other.transform.position;
        force.Normalize();
        rb2d.AddForce(force * kForce);
        lastKnockTime = Time.time;
        Invoke("StopForce", 0.2f);
    }

    private void updateAnimations() 
    {
        //sets animation based upon which direction enemy is going toward
        Vector2 detectDistance = transform.position - player.transform.position;
        if (detectDistance.y > detectDistance.x && detectDistance.y > 0)
            anim.Play("skel_run_down");
        else if (detectDistance.y < detectDistance.x && detectDistance.y < 0)
            anim.Play("skel_run_up");
        else if (detectDistance.x > 0)
            anim.Play("skel_run_left");
        else if (detectDistance.x < 0)
            anim.Play("skel_run_right");
    }
}
