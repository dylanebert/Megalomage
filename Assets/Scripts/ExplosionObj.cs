using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionObj : MonoBehaviour {

    private void Start() {
        Destroy(this.gameObject, 5f);
    }
}
