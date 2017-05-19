using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Caster : MonoBehaviour {

    public Image manaBar;
    public float maxMana = 100f;
    public float manaRegen = 10f;

    Color manaColor;
    float mana;

    private void Start() {
        mana = maxMana;
        manaColor = manaBar.color;
    }

    private void Update() {
        mana = Mathf.Clamp(mana + Time.deltaTime * manaRegen, 0, maxMana);
    }

    private void OnGUI() {
        float v = Mathf.Clamp01(mana / maxMana);
        manaBar.fillAmount = v;
        manaBar.color = Color.Lerp(Color.red, manaColor, v);
    }

    public void ResetMana() {
        mana = maxMana;
    }

    public void TryCast(Spell spell, Controller controller) {
        if (mana > spell.manaCost) {
            mana -= spell.manaCost;
            spell.Cast(controller);
        }
    }
}