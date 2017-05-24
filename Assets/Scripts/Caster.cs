using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Caster : MonoBehaviour {

    public SteamVR_TrackedController mainHand;
    public SteamVR_TrackedController offHand;
    public Fireball fireball;

    Color manaColor;
    float vibrateTimer;

    private void Start() {
        mainHand.TriggerUnclicked += fireball.Cast;
    }

    private void Update() {
        fireball.charging = mainHand.triggerPressed;
    }

    public void Vibrate(ushort vibrate) {
        SteamVR_Controller.Input((int)mainHand.controllerIndex).TriggerHapticPulse(vibrate);
    }
}