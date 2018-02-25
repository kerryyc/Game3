using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkTowardsPlayer : MonoBehaviour {

    public float speed = 2f;

    private GameObject player;

    void Awake () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update () {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }
}
