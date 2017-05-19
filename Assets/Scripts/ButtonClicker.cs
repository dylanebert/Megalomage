using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClicker : MonoBehaviour {

    SteamVR_LaserPointer pointer;
    Button button;
    GameObject eventSystem;
    bool pointerOnButton;

    private void Start() {
        pointer = GetComponent<SteamVR_LaserPointer>();
        eventSystem = GameObject.Find("EventSystem");
        pointer.PointerIn += PointerIn;
        pointer.PointerOut += PointerOut;
    }

    private void Update() {
        if(pointerOnButton) {
            if (SteamVR_Controller.Input((int)GetComponent<SteamVR_TrackedController>().controllerIndex).GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) {
                button.onClick.Invoke();
                button = null;
                pointerOnButton = false;
            }
        }
    }

    void PointerIn(object sender, PointerEventArgs args) {
        if (args.target.gameObject.GetComponent<Button>() != null && button == null) {
            button = args.target.gameObject.GetComponent<Button>();
            button.Select();
            pointerOnButton = true;
            SteamVR_Controller.Input((int)GetComponent<SteamVR_TrackedController>().controllerIndex).TriggerHapticPulse(2000);
        }
    }

    void PointerOut(object sender, PointerEventArgs args) {
        if (button != null) {
            pointerOnButton = false;
            eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
            button = null;
        }
    }
}
