using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Caster : MonoBehaviour {

    public Controller[] controllers;
    public Spell[] spells;
    public float tolerance = 2f;
    [HideInInspector]
    public Dictionary<SteamVR_TrackedController, Controller> controllerDict;

    GameManager gameManager;

    private void Start() {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        controllerDict = new Dictionary<SteamVR_TrackedController, Controller>();
        foreach(Controller controller in controllers) {
            controllerDict.Add(controller.controller, controller);
            controller.controller.Gripped += Grip;
            controller.controller.Ungripped += Ungrip;
            controller.controller.TriggerClicked += Trigger;
        }
    }

    private void Update() {
        foreach (Controller controller in controllers) {
            if (controller.drawing) {
                controller.trail.transform.position = controller.controller.transform.position;
                if(Vector3.Distance(controller.prevPosition, controller.controller.transform.position) > 0.05f) {
                    controller.prevPosition = controller.controller.transform.position;
                    controller.path.Add(controller.controller.transform.position);
                }
            } else {
                controller.helpPath.transform.position = controller.controller.transform.position;
                controller.DrawHelpPath();
            }
        }
    }

    void Grip(object sender, ClickedEventArgs args) {
        Controller controller = controllerDict[((SteamVR_TrackedController)sender)];
        if (controller.readySpell != null) return;

        controller.drawing = true;
        controller.path.Clear();
        controller.anchor.transform.position = controller.controller.transform.position;
    }

    void Ungrip(object sender, ClickedEventArgs args) {
        Controller controller = controllerDict[((SteamVR_TrackedController)sender)];
        controller.drawing = false;
        controller.helpPath.enabled = false;

        if (controller.readySpell != null) return;

        Dictionary<Spell, double> distances = new Dictionary<Spell, double>();
        Vector3[] controllerPath = new Vector3[controller.path.Count];
        for(int i = 0; i < controllerPath.Length; i++) {
            controllerPath[i] = controller.anchor.transform.InverseTransformPoint(controller.path[i]);
        }
        foreach(Spell spell in spells) {
            SimpleDTW dtw = new SimpleDTW(controllerPath, controller.index == 0 ? spell.pathL : spell.pathR);
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
            Vector3[] spellPath = controller.index == 0 ? min.pathL : min.pathR;
            for (int i = 0; i < positions.Length; i++) {
                positions[i] = controller.helpPath.transform.TransformPoint(spellPath[i]);
            }
            StartCoroutine(BurnPath(controller, positions));
        }        
    }

    void Trigger(object sender, ClickedEventArgs args) {
        Controller controller = controllerDict[((SteamVR_TrackedController)sender)];
        if (controller.readySpell == null) return;
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

    public void HelpHover(Spell spell) {
        foreach (Controller controller in controllers) {
            if (controller.drawing || controller.readySpell != null) return;
            controller.helpPathPositions = controller.index == 0 ? spell.pathL : spell.pathR;
            controller.controller.Ungripped += HelpUngrip;
        }
    }

    void HelpUngrip(object sender, ClickedEventArgs args) {
        Controller controller = controllerDict[(SteamVR_TrackedController)sender];
        controller.helpPath.enabled = false;
        controller.helpPathPositions = null;
        ((SteamVR_TrackedController)sender).Ungripped -= HelpUngrip;
    }

    public void ClearHelpPath() {
        foreach (Controller controller in controllers) {
            controller.helpPath.enabled = false;
            controller.helpPathPositions = null;
        }
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
    public int index;
    public Transform anchor;
    [HideInInspector]
    public Spell readySpell;
    [HideInInspector]
    public List<Vector3> path;
    [HideInInspector]
    public Vector3 prevPosition;
    [HideInInspector]
    public Vector3[] helpPathPositions;

    public void Vibrate(ushort duration) {
        SteamVR_Controller.Input((int)controller.controllerIndex).TriggerHapticPulse(duration);
    }

    public IEnumerator VibrateTwice(ushort duration, float interval) {
        SteamVR_Controller.Input((int)controller.controllerIndex).TriggerHapticPulse(duration);
        yield return new WaitForSeconds(interval);
        SteamVR_Controller.Input((int)controller.controllerIndex).TriggerHapticPulse(duration);
    }

    public void DrawHelpPath() {
        if (helpPathPositions == null) return;
        Vector3[] positions = new Vector3[helpPathPositions.Length];
        for(int i = 0; i < positions.Length; i++) {
            positions[i] = helpPath.transform.TransformPoint(helpPathPositions[i]);
        }
        helpPath.positionCount = positions.Length;
        helpPath.SetPositions(positions);
        helpPath.enabled = true;
    }
}