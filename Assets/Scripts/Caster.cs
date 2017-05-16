using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Caster : MonoBehaviour {

    public Controller[] controllers;
    public Spell[] spells;
    public float tolerance = 2f;

    Dictionary<SteamVR_TrackedController, Controller> controllerDict;

    private void Start() {
        controllerDict = new Dictionary<SteamVR_TrackedController, Controller>();
        foreach(Controller controller in controllers) {
            controllerDict.Add(controller.controller, controller);
            controller.controller.Gripped += Grip;
            controller.controller.Ungripped += Ungrip;
            controller.controller.TriggerClicked += Trigger;
        }
    }

    private void Update() {
        foreach(Controller controller in controllers) {
            if (controller.drawing) {
                controller.trail.transform.position = controller.controller.transform.position;
                if(Vector3.Distance(controller.prevPosition, controller.controller.transform.position) > 0.05f) {
                    controller.prevPosition = controller.controller.transform.position;
                    controller.path.Add(controller.controller.transform.position);
                }
            }
        }
    }

    void Grip(object sender, ClickedEventArgs args) {
        Controller controller = null;
        int controllerIndex = -1;
        for (int i = 0; i < 2; i++) {
            if (controllers[i].controller.GetInstanceID() == ((SteamVR_TrackedController)sender).GetInstanceID()) {
                controller = controllers[i];
                controllerIndex = i;
            }
        }
        if (controller == null || controller.readySpell != null) return;
        controller.drawing = true;

        controller.path.Clear();

        controller.helpPath.transform.position = controller.controller.transform.position;
        controller.helpPath.positionCount = spells[0].segments + 1;
        Vector3[] positions = new Vector3[spells[0].segments + 1];
        Vector3[] spellPath = controllerIndex == 0 ? spells[0].pathL : spells[0].pathR;
        for (int i = 0; i < positions.Length; i++) {
            positions[i] = controller.helpPath.transform.TransformPoint(spellPath[i]);
        }
        controller.helpPath.SetPositions(positions);
        controller.helpPath.enabled = true;
    }

    void Ungrip(object sender, ClickedEventArgs args) {
        Controller controller = null;
        int controllerIndex = -1;
        for(int i = 0; i < 2; i++) {
            if (controllers[i].controller.GetInstanceID() == ((SteamVR_TrackedController)sender).GetInstanceID()) {
                controller = controllers[i];
                controllerIndex = i;
            }
        }
        if (controller == null) return;
        controller.drawing = false;
        controller.helpPath.enabled = false;

        if (controller.readySpell != null) return;

        Dictionary<Spell, double> distances = new Dictionary<Spell, double>();
        foreach(Spell spell in spells) {
            Vector3[] positions = new Vector3[spells[0].segments + 1];
            Vector3[] spellPath = controllerIndex == 0 ? spell.pathL : spell.pathR;
            for (int i = 0; i < positions.Length; i++) {
                positions[i] = controller.helpPath.transform.TransformPoint(spellPath[i]);
            }
            SimpleDTW dtw = new SimpleDTW(controller.path.ToArray(), positions);
            dtw.computeDTW();
            distances.Add(spell, dtw.getSum());
        }
        if (distances.Count == 0) return;
        Spell min = distances.OrderBy(k => k.Value).FirstOrDefault().Key;
        Debug.Log(distances[min]);
        if(distances[min] < tolerance * spells[0].segments) {
            controller.readySpell = min;
            controller.readySpell.Ready(controller);
            Vector3[] positions = new Vector3[spells[0].segments + 1];
            Vector3[] spellPath = controllerIndex == 0 ? min.pathL : min.pathR;
            for (int i = 0; i < positions.Length; i++) {
                positions[i] = controller.helpPath.transform.TransformPoint(spellPath[i]);
            }
            StartCoroutine(BurnPath(controller, positions));
        }        
    }

    void Trigger(object sender, ClickedEventArgs args) {
        Controller controller = null;
        int controllerIndex = -1;
        for (int i = 0; i < 2; i++) {
            if (controllers[i].controller.GetInstanceID() == ((SteamVR_TrackedController)sender).GetInstanceID()) {
                controller = controllers[i];
                controllerIndex = i;
            }
        }
        if (controller == null || controller.readySpell == null) return;
        controller.readySpell.Cast(controller);
        controller.readySpell = null;
    }

    IEnumerator BurnPath(Controller controller, Vector3[] path) {
        foreach (ParticleSystem particle in controller.pathBurner.GetComponentsInChildren<ParticleSystem>())
            particle.Play();
        for(int i = 0; i < path.Length; i += Mathf.Max(1, Mathf.RoundToInt(path.Length / 50f))) {
            controller.pathBurner.transform.position = path[i];
            yield return new WaitForSeconds(.01f);
        }
        while(Vector3.Distance(controller.pathBurner.transform.position, controller.controller.transform.position) > 0.1f) {
            controller.pathBurner.transform.position = Vector3.MoveTowards(controller.pathBurner.transform.position, controller.controller.transform.position, .1f);
            yield return new WaitForSeconds(.01f);
        }
        foreach (ParticleSystem particle in controller.pathBurner.GetComponentsInChildren<ParticleSystem>())
            particle.Stop();
    }
}

[System.Serializable]
public class Controller {
    public SteamVR_TrackedController controller;
    public TrailRenderer trail;
    public LineRenderer helpPath;
    public ParticleSystem fire;
    public LineRenderer aimPath;
    public GameObject pathBurner;
    public bool drawing;
    [HideInInspector]
    public Spell readySpell;
    [HideInInspector]
    public List<Vector3> path;
    [HideInInspector]
    public Vector3 prevPosition;

    public void Vibrate(ushort duration) {
        SteamVR_Controller.Input((int)controller.controllerIndex).TriggerHapticPulse(duration);
    }

    public IEnumerator VibrateTwice(ushort duration, float interval) {
        SteamVR_Controller.Input((int)controller.controllerIndex).TriggerHapticPulse(duration);
        yield return new WaitForSeconds(interval);
        SteamVR_Controller.Input((int)controller.controllerIndex).TriggerHapticPulse(duration);
    }
}