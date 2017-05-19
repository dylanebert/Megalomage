using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    public Spell[] spells;
    public Caster caster;
    public GameObject aimPathObj;
    public ParticleSystem handFire;
    public SteamVR_TrackedController controller;
    [HideInInspector]
    public int selectedSpellIndex = -1;

    LineRenderer aimPath;

    private void Start() {
        aimPath = Instantiate(aimPathObj, this.transform).GetComponent<LineRenderer>();
        controller.TriggerClicked += Trigger;
    }

    public void ReadySpell(int index) {
        if (selectedSpellIndex != -1)
            spells[selectedSpellIndex].Deinitialize(this);
        selectedSpellIndex = index;
        spells[selectedSpellIndex].Initialize(this);
    }

    void Trigger(object sender, ClickedEventArgs args) {
        if(selectedSpellIndex != -1)
            caster.TryCast(spells[selectedSpellIndex], this);
    }

    public void ToggleAimPath(bool toggle) {
        aimPath.enabled = toggle;
    }

    public void SetAimPathColor(Color color) {
        aimPath.startColor = color;
        aimPath.endColor = color;
    }

    public void Vibrate(ushort intensity) {
        SteamVR_Controller.Input((int)controller.controllerIndex).TriggerHapticPulse(intensity);
    }
}
