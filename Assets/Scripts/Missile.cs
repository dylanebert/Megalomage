using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Spell {

    public GameObject missileObj;

    GameObject missile;
    bool charging;

    private void OnEnable() {
        caster.castController.TriggerClicked += TriggerDown;
        caster.castController.TriggerUnclicked += TriggerUp;
    }

    private void OnDisable() {
        caster.castController.TriggerClicked -= TriggerDown;
        caster.castController.TriggerUnclicked -= TriggerUp;
    }

    void TriggerDown(object sender, ClickedEventArgs args) {
        if (charging) return;
        charging = true;
        missile = Instantiate(missileObj, transform);
    }

    void TriggerUp(object sender, ClickedEventArgs args) {
        if (!charging) return;
        charging = false;
        missile.GetComponent<MissileObj>().Launch();
        missile.transform.parent = null;
    }
}
