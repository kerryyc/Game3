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
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); //find players
            GameObject newEnemy = Instantiate(enemy, new Vector2(transform.position.x, transform.position.y), transform.rotation); //create new enemy
            newEnemy.GetComponent<Enemy>().alivePlayers = players.Length; //set alive player count on new enemy
            timer = delay; //set delay
        }
    }
}
