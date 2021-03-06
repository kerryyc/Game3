﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundHandler : MonoBehaviour {
    //other variables
    public bool isMenu = false;
    public static SoundHandler instance;
    public float surviveTime = 61f;
    public GameObject UIHandler;

    //sound variables
    private static AudioSource soundSource;
    public AudioClip mainBGM;
    public AudioClip intenseBGM;
    public AudioClip gameOverTrack;
    public AudioClip winTrack;
    public AudioClip playerDeath;
    public AudioClip menuTheme;

    private static bool keepFadingIn = false;
    private static bool keepFadingOut = false;
    private bool playOnce = false;
    private bool transitionTrack = false;

    [HideInInspector] public int numPlayers;

    void Awake () {
        instance = this;
        soundSource = GetComponent<AudioSource>();
        if (!isMenu) {
            FadeInCaller(mainBGM, 0.01f, 1f);
            numPlayers = GameObject.FindGameObjectsWithTag("Player").Length;
        }
        else {
            soundSource.clip = menuTheme;
            soundSource.Play();
        }
    }

    void Update() {
        if (isMenu) return;
        GameObject[] checkPlayers = GameObject.FindGameObjectsWithTag("Player"); //get all players

        if(checkPlayers.Length < numPlayers) {
            soundSource.PlayOneShot(playerDeath);
            --numPlayers;
        }

        //if all players are dead, play game over
        if (!playOnce && numPlayers == 0 && UIHandler.GetComponent<UIHandler>().attempts <= 0) {
            //FadeOutCaller(0.01f);
            soundSource.time = 0f;
            soundSource.clip = gameOverTrack;
            soundSource.Play();
            soundSource.PlayOneShot(playerDeath);
            playOnce = true;
        }

        surviveTime -= Time.deltaTime;
        //if players won, play win
        if (!playOnce && (int)surviveTime <= 0) {
            soundSource.clip = winTrack;
            soundSource.time = 0;
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
