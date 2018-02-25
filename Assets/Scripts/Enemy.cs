using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float heatlh = 1f;

    private GameObject player;
    private Rigidbody2D rb2d;

    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        rb2d = GetComponent<Rigidbody2D>();
    }
}
