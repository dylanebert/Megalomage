using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : ScriptableObject {

    public float manaCost;
<<<<<<< HEAD
    public Caster caster;
    public Sprite icon;
=======
    public abstract void Initialize(Controller controller);
    public abstract void Cast(Controller controller);
    public abstract void Deinitialize(Controller controller);
>>>>>>> 0c7af0f844669d3799a247222e1c46432d916e20
}