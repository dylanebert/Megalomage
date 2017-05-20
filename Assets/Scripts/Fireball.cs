using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Fireball")]
public class Fireball : Spell {

    public GameObject fireballObj;

<<<<<<< HEAD
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
=======
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
>>>>>>> 0c7af0f844669d3799a247222e1c46432d916e20
    }

    public override void Deinitialize(Controller controller) {
        controller.ToggleAimPath(false);
        controller.handFire.Stop();
        controller.SetAimPathColor(Color.white);
    }
}
