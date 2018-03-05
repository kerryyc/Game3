using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {
    public GameObject enemy;
    public float delay;
    float timer;

    void Start()
    {
        timer = delay;
    }

	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Instantiate(enemy, new Vector2(transform.position.x, transform.position.y), transform.rotation); //create new enemy
            timer = delay; //set delay
        }
    }
}
