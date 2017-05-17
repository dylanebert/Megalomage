using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [HideInInspector]
    public List<GameObject> enemies;
    public SteamVR_LaserPointer pointer;
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public Level[] levels;
    public bool playing;
    public float waveInterval = 15f;

    float gameTimer = 0f;
    float prevGameTime = -10f;
    int currentLevel;
    int currentWave;

    private void Start() {
        enemies = new List<GameObject>();
    }

    private void Update() {
        if (!playing) return;

        gameTimer += Time.deltaTime;
        if(gameTimer - prevGameTime >= waveInterval) {
            prevGameTime += waveInterval;
            if (currentWave < levels[currentLevel].numWaves)
                SpawnWave();
            else {
                if (enemies.Count == 0) {
                    Stop();
                    ShowMainMenu();
                }
            }
        }
    }

    void SpawnWave() {
        List<Vector3> emptyPositions = new List<Vector3>();
        for(float x = -3f; x <= 3f; x += 1.5f) {
            for(float z = 40f; z < 50f; z += 1f) {
                emptyPositions.Add(new Vector3(x, 0, z));
            }
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
        playing = true;
        gameTimer = 0f;
        prevGameTime = -10f;
        currentWave = 0;
    }

    public void Stop() {
        playing = false;
        while(enemies.Count > 0) {
            GameObject enemy = enemies[0];
            enemies.Remove(enemy);
            Destroy(enemy);
        }
    }

    public void ShowMainMenu() {
        mainMenu.SetActive(true);
        pointer.SetActive(true);
        playing = false;
    }

    public void HideMainMenu() {
        mainMenu.SetActive(false);
        pointer.SetActive(false);
    }

    public void ShowPauseMenu() {
        pauseMenu.SetActive(true);
        pointer.SetActive(true);
        playing = false;
    }

    public void HidePauseMenu() {
        pauseMenu.SetActive(false);
        pointer.SetActive(false);
        playing = true;
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
