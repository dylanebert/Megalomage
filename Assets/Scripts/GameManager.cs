﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [HideInInspector]
    public List<GameObject> enemies;
    public SteamVR_TrackedController[] controllers;
    public Barricade[] barricades;
    public SteamVR_LaserPointer pointer;
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public Level[] levels;
    public bool playing;
    public float waveInterval = 5f;

    Caster caster;
    float gameTimer = 0f;
    float prevGameTime = -10f;
    int currentLevel;
    int currentWave;

    private void Start() {
        enemies = new List<GameObject>();
        caster = GameObject.FindGameObjectWithTag("Player").GetComponent<Caster>();
        foreach (SteamVR_TrackedController controller in controllers)
            controller.MenuButtonClicked += TogglePauseMenu;
    }

    private void Update() {
        if (!playing) return;

        gameTimer += Time.deltaTime;
        if(gameTimer - prevGameTime >= waveInterval) {
            prevGameTime += waveInterval;
            if (currentWave < levels[currentLevel].numWaves)
                SpawnWave();
        }

        if (enemies.Count == 0 && currentWave >= levels[currentLevel].numWaves)
            StartCoroutine(WinSequence());
    }

    void SpawnWave() {
        List<Vector3> emptyPositions = new List<Vector3>();
        for(float x = -3f; x <= 3f; x += 1.5f) {
            emptyPositions.Add(new Vector3(x, 0, 40f));
        }
        for(int i = 0; i < levels[currentLevel].enemyCurves.Length; i++) {
            for(int j = Mathf.RoundToInt(levels[currentLevel].enemyCurves[i].spawnRate.Evaluate(currentWave)); j > 0; j--) {
                Vector3 position = emptyPositions[Mathf.RoundToInt(Random.Range(0, emptyPositions.Count - 1))];
                GameObject minion = Instantiate(levels[currentLevel].enemyCurves[i].enemy, position, Quaternion.Euler(0, 180, 0));
                emptyPositions.Remove(position);
                enemies.Add(minion);
            }
        }
        currentWave++;
    }

    public void StartGame() {
        ResetLevel();
        playing = true;
    }

    public void ResetLevel() {
        while (enemies.Count > 0) {
            GameObject enemy = enemies[0];
            enemies.Remove(enemy);
            Destroy(enemy);
        }
        foreach (Barricade barricade in barricades)
            barricade.ResetHealth();
        currentWave = 0;
        gameTimer = 0f;
        prevGameTime = -10f;
    }

    public void ShowMainMenu() {
        if(Time.timeScale == 0) {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
        ResetLevel();
        mainMenu.SetActive(true);
        pointer.SetActive(true);
        playing = false;
    }

    public void HideMainMenu() {
        mainMenu.SetActive(false);
        pointer.SetActive(false);
    }

    public void TogglePauseMenu(object sender, ClickedEventArgs args) {
        if (Time.timeScale != 0)
            ShowPauseMenu();
        else
            HidePauseMenu();
    }

    public void ShowPauseMenu() {
        if (mainMenu.activeSelf) return;
        pauseMenu.SetActive(true);
        pointer.SetActive(true);
        playing = false;
        Time.timeScale = 0;
    }

    public void HidePauseMenu() {
        if (mainMenu.activeSelf) return;
        pauseMenu.SetActive(false);
        pointer.SetActive(false);
        playing = true;
        Time.timeScale = 1;
    }

    public IEnumerator WinSequence() {
        yield return new WaitForSeconds(2f);
        ShowMainMenu();
    }

    public IEnumerator LoseSequence() {
        playing = false;
        foreach (GameObject enemy in enemies) {
            enemy.GetComponent<Animator>().SetTrigger("Victory");
        }
        yield return new WaitForSeconds(2f);
        ShowMainMenu();
    }
}

[System.Serializable]
public class Level {
    public int numWaves;
    public EnemyCurve[] enemyCurves;

    [System.Serializable]
    public struct EnemyCurve {
        public GameObject enemy;
        public AnimationCurve spawnRate;
    }
}
