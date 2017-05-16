using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballObj : MonoBehaviour {

    public GameObject explosionObj;
    public float noise = 50f;

    bool active = true;

    private void Start() {
        Destroy(this.gameObject, 10f);
    }

    private void FixedUpdate() {
        if(active)
            GetComponent<Rigidbody>().AddRelativeForce(new Vector3(Random.Range(-noise, noise), 0, Random.Range(-noise, noise)));
    }

    private void OnCollisionEnter(Collision collision) {
        if (active) {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            if (collision.gameObject.layer != 4) {
                GameObject explosion = Instantiate(explosionObj, transform.position - transform.forward, Quaternion.identity);
                Destroy(explosion, 5f);
            }
            foreach (ParticleSystem particle in GetComponentsInChildren<ParticleSystem>())
                particle.Stop();
            Destroy(this.gameObject, 5f);
            active = false;
        }
    }
}
