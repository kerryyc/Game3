using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathSound : MonoBehaviour {
    private AudioSource soundSource;
    public AudioClip playerDeathSound;
	// Use this for initialization
	void Start () {
        
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnEnable()
    {
        soundSource = GetComponent<AudioSource>();
        soundSource.PlayOneShot(playerDeathSound);
    }
}
