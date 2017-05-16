using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Spell), true)]
public class SpellEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        ((Spell)target).ApplyPaths();
    }
}
