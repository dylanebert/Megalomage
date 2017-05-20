using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
=======
[CreateAssetMenu(menuName = "Spells/Missile")]
>>>>>>> 0c7af0f844669d3799a247222e1c46432d916e20
public class Missile : Spell {

    public GameObject missileObj;

<<<<<<< HEAD
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
=======
    public override void Initialize(Controller controller) {
        controller.ToggleAimPath(true);
    }

    public override void Cast(Controller controller) {
        GameObject missile = Instantiate(missileObj, controller.transform.position, Quaternion.identity);
        missile.transform.forward = controller.transform.forward;
        missile.GetComponent<Rigidbody>().AddForce(missile.transform.forward * 50f, ForceMode.Impulse);
        controller.Vibrate(1000);
    }

    public override void Deinitialize(Controller controller) {
        controller.ToggleAimPath(false);
>>>>>>> 0c7af0f844669d3799a247222e1c46432d916e20
    }
}
