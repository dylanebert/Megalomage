﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float speed = 0.5f; //Units per second
    public float health = 100f;

    bool alive = true;
    GameManager gameManager;

    private void Start() {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void Update() {
        if (!gameManager.playing || !alive) return;
        transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
    }

    public void Damage(float damage) {
        if (!alive) return;
        health -= damage;
        if (health <= 0) {
            GetComponent<Animator>().SetTrigger("Die");
            GetComponent<Collider>().enabled = false;
            gameManager.enemies.Remove(this.gameObject);
            Destroy(this.gameObject, 2f);
            alive = false;
        }
    }
}