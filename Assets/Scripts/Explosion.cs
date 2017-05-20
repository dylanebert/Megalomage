using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public float damage = 100f;
    public float radius = 3f;

    private void Start() {
        Destroy(this.gameObject, 5f);
        foreach(Collider col in Physics.OverlapSphere(transform.position, radius, 1 << 9)) {
            if(!col.isTrigger)
                col.GetComponent<Enemy>().Damage(damage);
        }
    }
}
