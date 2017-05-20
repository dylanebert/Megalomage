using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Caster : MonoBehaviour {

    public SteamVR_TrackedController mainHand;
    public SteamVR_TrackedController offHand;
    public Fireball fireball;
    public Image manaBar;
    public float maxMana = 100f;
    public float manaRegen = 10f;
    public float mana;

    Color manaColor;

    private void Start() {
        mana = maxMana;
        manaColor = manaBar.color;
        mainHand.TriggerUnclicked += fireball.Cast;
    }

    private void Update() {
        mana = Mathf.Clamp(mana + Time.deltaTime * manaRegen, 0, maxMana);
        fireball.charging = mainHand.triggerPressed;
    }

    private void OnGUI() {
        float v = Mathf.Clamp01(mana / maxMana);
        manaBar.fillAmount = v;
        manaBar.color = Color.Lerp(Color.red, manaColor, v);
    }
}