using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileObj : MonoBehaviour {

    public float damage = 15f;

    bool active;

    private void Start() {
        Destroy(this.gameObject, 10f);
    }

    public void Launch() {
        active = true;
        GetComponent<Rigidbody>().AddForce(transform.forward * 50f, ForceMode.Impulse);
        GetComponentInChildren<LineRenderer>().enabled = false;
    }

    private void OnCollisionEnter(Collision collision) {
        if (active) {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            if(collision.gameObject.layer == 9) {
                collision.gameObject.GetComponent<Enemy>().Damage(damage);
            }
            foreach (ParticleSystem particle in GetComponentsInChildren<ParticleSystem>())
                particle.Stop();
            Destroy(this.gameObject, 5f);
            active = false;
        }
    }
}
