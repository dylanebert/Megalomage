using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileObj : MonoBehaviour {

    public float damage = 15f;

<<<<<<< HEAD
    bool active;
=======
    bool active = true;
>>>>>>> 0c7af0f844669d3799a247222e1c46432d916e20

    private void Start() {
        Destroy(this.gameObject, 10f);
    }

<<<<<<< HEAD
    public void Launch() {
        active = true;
        GetComponent<Rigidbody>().AddForce(transform.forward * 50f, ForceMode.Impulse);
        GetComponentInChildren<LineRenderer>().enabled = false;
    }

=======
>>>>>>> 0c7af0f844669d3799a247222e1c46432d916e20
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
