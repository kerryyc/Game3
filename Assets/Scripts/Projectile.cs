using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    // Use this for initialization
    public float speed;

    private PlayerController player;
    private Vector2 target;
    public Rigidbody2D rb2d;

	void Start () {
        target = new Vector2(player.transform.position.x, player.transform.position.y);
        
        rb2d = GetComponent<Rigidbody2D>();
        
       
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            DestroyProjectile();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            DestroyProjectile();
        }
    }
    public void setPlayerController(PlayerController player)
    {
        this.player = player;
    }
    void DestroyProjectile()
    {
        Destroy(this.gameObject);
    }
}
