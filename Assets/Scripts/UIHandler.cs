using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {
    //variables
    public bool isMenu = false;
    public int levelNum = 1;
    public string sceneName;
    public float surviveTime = 181f;
    public float waveTimeInterval = 61f;
    public float waveTime = 61f;

    //GameObjects
    public GameObject canvas;
    public GameObject gameOver;
    public GameObject winObject;
    public GameObject pauseObject;
    public Text timer;

    void Awake() {
        Time.timeScale = 1;
    }

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
        // Jansen Yan: (update the text with the wave timer interval)
        waveTimeInterval -= Time.deltaTime;
        surviveTime -= Time.deltaTime;

        // Jansen Yan: calculate the wave time interval and update the text
        // timer.text = ((int)surviveTime).ToString();

        timer.text = ((int)waveTimeInterval).ToString();

        // Jansen Yan: every 60 seconds, change the time back to 0
        if ((int)waveTimeInterval == 0)
        {
            // Jansen Yan: reset back to 60
            waveTimeInterval = waveTime;
            timer.text = ((int) waveTimeInterval).ToString();
        }
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
