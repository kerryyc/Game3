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
            Instantiate(enemy, new Vector2(transform.position.x, transform.position.y), transform.rotation);
            timer = delay;
        }
    }
}
