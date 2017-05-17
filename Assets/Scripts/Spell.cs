using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour {

    public bool twoHanded;
    public AnimationCurve XL;
    public AnimationCurve YL;
    public AnimationCurve ZL;
    public AnimationCurve XR;
    public AnimationCurve YR;
    public AnimationCurve ZR;
    public float drawScale = 1f;
    public int segments = 100;
    public Vector3[] pathL;
    public Vector3[] pathR;

    protected Caster caster;

    private void Start() {
        caster = GameObject.FindGameObjectWithTag("Player").GetComponent<Caster>();
    }

    public void ApplyPaths() {
        pathL = new Vector3[segments + 1];
        pathR = new Vector3[segments + 1];
        if (twoHanded) {
            for (int i = 0; i <= segments; i++) {
                pathL[i] = new Vector3(XL.Evaluate((float)i * 1f / (float)segments), YL.Evaluate((float)i * 1f / (float)segments), ZL.Evaluate((float)i * 1f / (float)segments)) * drawScale;
                pathR[i] = new Vector3(XR.Evaluate((float)i * 1f / (float)segments), YR.Evaluate((float)i * 1f / (float)segments), ZR.Evaluate((float)i * 1f / (float)segments)) * drawScale;
            }
        }
        else {
            for (int i = 0; i <= segments; i++) {
                pathL[i] = new Vector3(XL.Evaluate((float)i * 1f / (float)segments), YL.Evaluate((float)i * 1f / (float)segments), ZL.Evaluate((float)i * 1f / (float)segments)) * drawScale;
                pathR[i] = new Vector3(XL.Evaluate((float)i * 1f / (float)segments), YL.Evaluate((float)i * 1f / (float)segments), ZL.Evaluate((float)i * 1f / (float)segments)) * drawScale;
            }
        }
    }

    public void Cast(Controller controller) {
        this.OnCast(controller);
    }

    public void Ready(Controller controller) {
        this.OnReady(controller);
    }

    public virtual void OnCast(Controller controller) {

    }

    public virtual void OnReady(Controller controller) {

    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        for(int i = 0; i < segments; i++) {
            Gizmos.DrawLine(Vector3.up + pathR[i], Vector3.up + pathR[i + 1]);
        }
    }
}