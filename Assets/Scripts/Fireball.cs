using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fireball : MonoBehaviour {

    public Image chargeBar;
    public ParticleSystem orb;
    public GameObject fireballObj;
    public float chargeRate = 0.2f;
    public float cooldown = 1.5f;

    FireballObj fireball;
    ParticleSystem.MainModule orbModule;
    [HideInInspector]
    public bool charging = false;
    float intensity;
    float cooldownTimer;

    private void Start() {
        orbModule = orb.main;
    }

    private void Update() {
        if (fireball != null) return;

        if (charging && cooldownTimer <= 0) {
            intensity = Mathf.Clamp01(intensity + Time.deltaTime * chargeRate);
            if (intensity >= 1f) {
                fireball = Instantiate(fireballObj, transform).GetComponent<FireballObj>();
            }
        } else {
            intensity = Mathf.Clamp01(intensity - Time.deltaTime * 1f / cooldown);
            cooldownTimer -= Time.deltaTime;
        }
    }

    private void OnGUI() {
        chargeBar.fillAmount = intensity;
        if (cooldownTimer > 0) {
            chargeBar.color = Color.grey;
            //orbModule.startSize = 0;
            orb.transform.localScale = Vector3.one * .01f;
        }
        else {
            chargeBar.color = Color.Lerp(Color.white, Color.yellow, intensity);
            //orbModule.startSize = intensity * 0.25f;
            orb.transform.localScale = Vector3.one * Mathf.Max(intensity, .01f);
        }
    }

    public void Cast(object sender, ClickedEventArgs args) {
        if (fireball == null) return;
        fireball.Launch();
        cooldownTimer = cooldown;
        fireball = null;
    }
}
