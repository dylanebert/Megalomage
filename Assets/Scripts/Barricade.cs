using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barricade : MonoBehaviour {

    public Image healthBar;
    public GameManager gameManager;
    public float maxHealth;

    float health;
    Color c;
    Color r;

    private void Start() {
        health = maxHealth;
        c = new Color(0, 1, 1, 0.5f);
        r = new Color(1, 0, 0, 0.5f);
    }

    private void OnGUI() {
        float v = health / maxHealth;
        GetComponent<MeshRenderer>().material.color = Color.Lerp(r, c, v);
        healthBar.fillAmount = v;
        healthBar.color = Color.Lerp(Color.red, Color.green, v);
    }

    public void Damage(float damage) {
        health -= damage;
        if(health <= 0 && GetComponent<MeshRenderer>().enabled) {
            StartCoroutine(gameManager.LoseSequence());
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void ResetHealth() {
        health = maxHealth;
        GetComponent<MeshRenderer>().enabled = true;
    }
}
