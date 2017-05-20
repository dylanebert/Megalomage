using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Fireball")]
public class Fireball : Spell {

    public GameObject fireballObj;

    GameObject fireball;
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
        if (caster.mana < manaCost) return;
        charging = true;
        fireball = Instantiate(fireballObj, transform);
        caster.mana -= manaCost;
    }

    void TriggerUp(object sender, ClickedEventArgs args) {
        if (!charging || fireball == null) return;
        charging = false;
        fireball.GetComponent<FireballObj>().Launch();
        fireball.transform.parent = null;
    }
}
