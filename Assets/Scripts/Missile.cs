using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Missile")]
public class Missile : Spell {

    public GameObject missileObj;

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
    }
}
