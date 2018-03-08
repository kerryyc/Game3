using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float speed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Wall")
        {
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        Destroy(this.gameObject);
    }
}
