using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : ScriptableObject {

    public float manaCost;
    public abstract void Initialize(Controller controller);
    public abstract void Cast(Controller controller);
    public abstract void Deinitialize(Controller controller);
}