using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundHandler : MonoBehaviour {
    //other variables
    public static SoundHandler instance;
    public float surviveTime = 61f;

    //sound variables
    private static AudioSource soundSource;
    public AudioClip mainBGM;
    public AudioClip intenseBGM;
    public AudioClip gameOverTrack;
    public AudioClip winTrack;
    public AudioClip playerDeath;

    private static bool keepFadingIn = false;
    private static bool keepFadingOut = false;
    private bool playOnce = false;
    private bool transitionTrack = false;

    private GameObject[] players;
    private int numPlayers;
    private int alivePlayers;

    void Awake () {
        instance = this;
        soundSource = GetComponent<AudioSource>();
        FadeInCaller(mainBGM, 0.01f, 1f);
        players = GameObject.FindGameObjectsWithTag("Player");
        alivePlayers = numPlayers = players.Length;
    }

    void Update() {
        GameObject[] checkPlayers = GameObject.FindGameObjectsWithTag("Player"); //get all players

        if(checkPlayers.Length < alivePlayers) {
            soundSource.PlayOneShot(playerDeath);
            --alivePlayers;
        }

        //if all players are dead, play game over
        if (!playOnce && alivePlayers == 0) {
            //FadeOutCaller(0.01f);
            // Jansen Yan: Not sure if intended, but added in the game over track code
            soundSource.clip = gameOverTrack;
            soundSource.Play();
            soundSource.PlayOneShot(playerDeath);
            playOnce = true;
        }

        surviveTime -= Time.deltaTime;
        //if players won, play win
        if (!playOnce && (int)surviveTime == 0) {
            soundSource.clip = winTrack;
            soundSource.Play();
            playOnce = true;
        }
        else if(!transitionTrack && (int)surviveTime == 60f) {
            float time = soundSource.time;
            soundSource.clip = intenseBGM;
            soundSource.time = time;
            soundSource.Play();
            transitionTrack = true;
        }
    }

    public static void FadeInCaller(AudioClip track, float fadeSpeed, float maxVolume) {
        soundSource.volume = 0;
        instance.StartCoroutine(FadeIn(track, fadeSpeed, maxVolume));
    }

    public static void FadeOutCaller(float fadeSpeed) {
        instance.StartCoroutine(FadeOut(fadeSpeed));
    }

    static IEnumerator FadeIn(AudioClip track, float fadeSpeed, float maxVolume) {
        keepFadingIn = true;
        keepFadingOut = false;

        soundSource.clip = track;
        soundSource.Play();
        while (keepFadingIn && soundSource.volume < maxVolume) {
            soundSource.volume += fadeSpeed;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        keepFadingIn = false;
    }

    static IEnumerator FadeOut(float fadeSpeed) {
        keepFadingIn = false;
        keepFadingOut = true;

        while (keepFadingOut && soundSource.volume >= 0) {
            soundSource.volume -= fadeSpeed;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}
