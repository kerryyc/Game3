using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {
    public GameObject enemy;
    public GameObject seeker;
    public float delay;
    public float surviveTime = 181f;
    
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
            // at 60 seconds, spawn only seekers
           
            if ((int)surviveTime <= 60)
            {
     
                typeOfEnemy = seeker;
            }
            // at 120 seconds, spawn mix of seekers and regular enemies
            else if ((int) surviveTime <= 120)
            {
                int randNum = Random.Range(1, 4);
                Debug.Log(randNum);
                // it is likely that 1/3 of the spawns will be seekers
                if (randNum > 1)
                {
                    typeOfEnemy = enemy;
                }
                else
                {
                    typeOfEnemy = seeker;
                }
            }
            // spawn only regular enemies
            else if ((int) surviveTime <= 180)
            {
                typeOfEnemy = enemy;
            }
            Instantiate(typeOfEnemy, new Vector2(transform.position.x, transform.position.y), transform.rotation); //create new enemy
            timer = delay; //set delay
        }

    }
}
