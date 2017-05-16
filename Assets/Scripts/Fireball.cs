using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Spell {

    public GameObject fireballObj;

    public override void OnReady(Controller controller) {
        controller.fire.Play();
        controller.aimPath.enabled = true;
    }

    public override void OnCast(Controller controller) {
        GameObject fireball = Instantiate(fireballObj, controller.controller.transform.position, Quaternion.identity);
        fireball.transform.forward = controller.controller.transform.forward;
        fireball.GetComponent<Rigidbody>().AddForce(fireball.transform.forward * 25f, ForceMode.Impulse);
        controller.fire.Stop();
        controller.aimPath.enabled = false;
        controller.Vibrate(3000);
    }
}
