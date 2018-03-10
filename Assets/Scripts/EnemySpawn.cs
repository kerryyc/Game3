using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {
    public GameObject enemy;
    public GameObject seeker;
    public float delay;
    public float surviveTime = 60f;
    
    float timer;

    void Start()
    {
        timer = delay;
       
    }

	void Update () {
        surviveTime -= Time.deltaTime;
        timer -= Time.deltaTime;
        GameObject typeOfEnemy = enemy; 
        if (timer <= 0)
        {
            if ((int)surviveTime <= 30)
            {
                int randNum = Random.Range(1, 4);
                Debug.Log(randNum);
                // it is likely that 1/3 of the spawns after 30 seconds will be seekers
                if (randNum > 1)
                {
                    typeOfEnemy = enemy;
                }
                else
                {
                    typeOfEnemy = seeker;
                }

            }
            Instantiate(typeOfEnemy, new Vector2(transform.position.x, transform.position.y), transform.rotation); //create new enemy
            timer = delay; //set delay
        }

    }
}
