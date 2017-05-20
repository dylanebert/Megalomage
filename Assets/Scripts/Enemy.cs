using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    public Image healthBar;
    public float speed = 0.5f; //Units per second
    public float health = 100f;

    float maxHealth;
    protected bool alive = true;

    protected GameManager gameManager;

    private void Start() {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        maxHealth = health;
    }

    private void Update() {
        if (!gameManager.playing || !alive) return;
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnGUI() {
        float v = Mathf.Clamp01(health / maxHealth);
        healthBar.fillAmount = v;
        healthBar.color = Color.Lerp(Color.red, Color.green, v);
    }

    public void Damage(float damage) {
        if (!alive) return;
        health -= damage;
        if (health <= 0) {
            GetComponent<Animator>().SetTrigger("Die");
            foreach(Collider col in GetComponents<Collider>())
                col.enabled = false;
            gameManager.enemies.Remove(this.gameObject);
            Destroy(this.gameObject, 2f);
            alive = false;
        }
    }
}
