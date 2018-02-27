using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour {
    public int levelNum = 1;
    public string sceneName;
    public GameObject canvas;
    public GameObject gameOver;
    private bool pause = false;

    void Update() {
        //if (Time.timeScale == 1) {
        //    canvas.SetActive(true);
        //}
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        //if all players are dead
        if (players.Length == 0) {
            Time.timeScale = 0;
            gameOver.SetActive(true);
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
        Time.timeScale = 0;
        canvas.SetActive(false);

        //player.GetComponent<SpriteRenderer>().enabled = false;
        SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
    }

    public void UnPauseScene() {
        Time.timeScale = 1;

        //player.GetComponent<SpriteRenderer>().enabled = true;
        SceneManager.UnloadSceneAsync("PauseMenu");
    }
}
