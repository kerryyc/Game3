﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {
    public bool isMenu = false;
    public int levelNum = 1;
    public string sceneName;
    public float surviveTime = 60f;
    public GameObject canvas;
    public GameObject gameOver;
    public GameObject winObject;
    public GameObject pauseObject;
    public Text timer;

    void Update() {
        if (isMenu) return; //skip if menu script

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); //get all players
        //if all players are dead
        if (players.Length == 0) {

            Time.timeScale = 0;
            gameOver.SetActive(true);
        }

        if (Input.GetButtonDown("Pause")) {
            PauseScene();
        }
        if (Input.GetButtonDown("Quit")) {
            QuitGame();
        }

        //calculate survive time and update text
        surviveTime -= Time.deltaTime;
        timer.text = ((int)surviveTime).ToString();

        //if survive time is 0, players have won
        if((int)surviveTime == 0) {
            Time.timeScale = 0;
            winObject.SetActive(true);
        }

        //make player sprite visible even if game is paused
        if(Time.timeScale == 0) {
            EnableSpriteRend(players);
        }
    }

    public void StartGame() {
        // added timeScale = 1 to prevent freezing when loading level1
        Time.timeScale = 1;
        string currLevel = "Level" + levelNum;
        SceneManager.LoadScene(currLevel, LoadSceneMode.Single);
    }

    public void QuitGame() {
        //quits application
        Application.Quit();
    }

    public void ReloadScene() {
        //reloads current scene
        if (EventSystem.current.IsPointerOverGameObject()) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void LoadScene() {
        //loads a scene under scenename
        SceneManager.LoadScene(sceneName);
    }

    public void PauseScene() {
        //pauses game and enables pause UI element
        pauseObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void UnPauseScene() {
        //resumes game and disables pause UI element
        pauseObject.SetActive(false);
        Time.timeScale = 1;
    }

    private void EnableSpriteRend(GameObject[] players) {
        //enable SpriteRenderer component on players
        foreach (GameObject player in players) {
            player.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
