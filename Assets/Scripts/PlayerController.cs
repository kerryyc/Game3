﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 5f;

    private int DOWN = 1;
    private int UP = 2;
    private int LEFT = 3;
    private int RIGHT = 4;
    private int direction = 1;

    private Rigidbody2D rb2d;
    private Animator anim;

	void Awake () {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); 
	}
	
	void Update () {
        Move();
	}

    private void Move() {
        //get directional input
        float horizontal = Input.GetAxisRaw("Horizontal") * speed;
        float vertical = Input.GetAxisRaw("Vertical") * speed;

        //set layer depending on direction
        SetLayer(direction, 0f); //set current layer weight to zero
        if (horizontal > 0)
            direction = RIGHT;
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
        rb2d.velocity = new Vector2(horizontal, vertical);
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
}