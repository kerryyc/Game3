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
    private float waveNum = 1;
    public int attempts = 3;
    private int numPlayers;

    //UI
    public GameObject canvas;
    public GameObject gameOver;
    public GameObject winObject;
    public GameObject pauseObject;
    public Text timer;
    public Text attemptText;
    public Text waveText;

    //other GameObjects
    public GameObject SoundHandler;
    public GameObject[] EnemySpawners;
    private GameObject[] players;

    void Awake() {
        Time.timeScale = 1;
        if (!isMenu) {
            players = GameObject.FindGameObjectsWithTag("Player");
            numPlayers = players.Length;
            EnableWaveText("Wave 1");
            attemptText.text = "Lives: " + attempts;
        }
    }

    void Update() {
        if (isMenu) return; //skip if menu script

        GameObject[] checkPlayers = GameObject.FindGameObjectsWithTag("Player"); //get all players
        //if all players are dead
        if (checkPlayers.Length == 0) {
            attemptText.text = "Lives: 0";
            checkGameOver();
        }
        
        if (Input.GetButtonDown("Pause")) {
            PauseScene();
        }
        if (Input.GetButtonDown("Quit")) {
            QuitGame();
        }

        //update the text with the wave timer interval
        waveTimeInterval -= Time.deltaTime;
        surviveTime -= Time.deltaTime;

        // calculate the wave time interval and update the text
        timer.text = ((int)waveTimeInterval).ToString();

       //very 60 seconds, change the time back to 0
        if ((int)waveTimeInterval == 0)
        {
            waveNum++;
            resetField("Wave " + waveNum);
        }
        //if survive time is 0, players have won
        if((int)surviveTime == 0) {
            Time.timeScale = 0;
            winObject.SetActive(true);
        }

        //make player sprite visible even if game is paused
        if(Time.timeScale == 0) {
            EnableSpriteRend(checkPlayers);

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

    private void checkGameOver() {
        --attempts;
        if (attempts <= 0) {
            Time.timeScale = 0;
            gameOver.SetActive(true);
        }
        else {
            attemptText.text = "Lives: " + attempts;
            surviveTime += waveTime;
            if(attempts != 1)
                resetField(attempts + " Lives Left");
            else
                resetField("Last Life");
            SoundHandler.GetComponent<SoundHandler>().numPlayers = numPlayers;
        }
    }

    private void resetField(string text) {
        //reset back to 60
        waveTimeInterval = waveTime;
        timer.text = ((int)waveTimeInterval).ToString();

        //reset players and destroy all tombstones and enemies
        ResetPlayers();
        DestroyGameObjectsWithTag("Tombstone");
        DestroyGameObjectsWithTag("Enemy");

        //show players wave number
        EnableWaveText(text);

        //delay spawners
        AddSecondsToSpawnerDelay();
    }

    private void EnableSpriteRend(GameObject[] players) {
        //enable SpriteRenderer component on players
        foreach (GameObject player in players) {
            player.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void ResetPlayers() {
        foreach (GameObject player in players) {
            player.GetComponent<PlayerController>().health = 10;
            player.GetComponent<PlayerController>().spriteBlinkingTotalTimer = 0f;
            player.GetComponent<PlayerController>().startBlinking = false;
            player.SetActive(true);
        }
    }

    private void DestroyGameObjectsWithTag(string tag) {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects) {
            Destroy(obj);
        }
    }

    private void EnableWaveText(string text) {
        waveText.gameObject.SetActive(true);
        waveText.text = text;
        StartCoroutine(RemoveAfterSeconds(3, waveText.gameObject));
    }

    IEnumerator RemoveAfterSeconds(int seconds, GameObject obj) {
        yield return new WaitForSeconds(seconds);
        obj.SetActive(false);
    }

    private void AddSecondsToSpawnerDelay() {
        foreach (GameObject spawner in EnemySpawners) {
            spawner.GetComponent<EnemySpawn>().timer = spawner.GetComponent<EnemySpawn>().delay;
        }
    }
}
