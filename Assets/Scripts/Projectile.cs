using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    void Awake() 
    {
        Destroy(this.gameObject, 8f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
    }
}
