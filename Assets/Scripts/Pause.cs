using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

    public GameManager gameManager;

    SteamVR_TrackedController controller;
    bool paused;

    private void Start() {
        controller = GetComponent<SteamVR_TrackedController>();
        controller.MenuButtonClicked += Menu;
    }

    void Menu(object sender, ClickedEventArgs args) {
        if (gameManager.mainMenu.activeSelf) return;
        if (paused) {
            gameManager.HidePauseMenu();
            paused = false;
        } else {
            gameManager.ShowPauseMenu();
            paused = true;
        }
    }
}