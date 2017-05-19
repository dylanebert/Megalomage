using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : Enemy {

    public float attackDamage = 5f;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == 10) {
            StartCoroutine(Attack(other.gameObject.GetComponent<Barricade>()));
        }
    }

    IEnumerator Attack(Barricade barricade) {
        speed = 0;
        while(true) {
            GetComponent<Animator>().SetTrigger("Attack");
            yield return new WaitForSeconds(0.5f);
            if (!barricade.GetComponent<MeshRenderer>().enabled || !alive) break;
            barricade.Damage(attackDamage);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
