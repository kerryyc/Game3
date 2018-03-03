using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBarScript : MonoBehaviour {
    private float playerHealth;
    private float health;
    private Image healthbar;
	// Use this for initialization
	void Start () {

        
	}
	
	// Update is called once per frame
	void Update () {
        health = this.transform.root.GetComponent<PlayerController>().health;
        healthbar = this.gameObject.GetComponent<Image>();
        healthbar.fillAmount = (health / 10);

    }
}
