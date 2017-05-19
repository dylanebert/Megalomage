using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Fireball")]
public class Fireball : Spell {

    public GameObject fireballObj;

    public override void Initialize(Controller controller) {
        controller.ToggleAimPath(true);
        controller.handFire.Play();
        controller.SetAimPathColor(Color.red);
    }

    public override void Cast(Controller controller) {
        GameObject fireball = Instantiate(fireballObj, controller.transform.position, Quaternion.identity);
        fireball.transform.forward = controller.transform.forward;
        fireball.GetComponent<Rigidbody>().AddForce(fireball.transform.forward * 25f, ForceMode.Impulse);
        controller.Vibrate(3000);
    }

    public override void Deinitialize(Controller controller) {
        controller.ToggleAimPath(false);
        controller.handFire.Stop();
        controller.SetAimPathColor(Color.white);
    }
}
