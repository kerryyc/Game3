using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {
    // Use this for initialization
    public GameObject enemy;
    public float delay;
    float timer;

    void Start()
    {
        timer = delay;

    }

	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            GameObject newEnemy = Instantiate(enemy, new Vector2(transform.position.x, transform.position.y), transform.rotation);
            newEnemy.GetComponent<Enemy>().alivePlayers = players.Length;
            timer = delay;
        }
    }
}
