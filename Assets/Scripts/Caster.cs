using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Caster : MonoBehaviour {

    public SteamVR_TrackedController menuController;
    public SteamVR_TrackedController castController;
    public Image manaBar;
    public Image equippedSpellIcon;
    public float maxMana = 100f;
    public float manaRegen = 10f;
    public float mana;

    GameObject equippedSpell;
    Color manaColor;

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

    public void EquipSpell(GameObject spell) {
        if (equippedSpell != null)
            equippedSpell.SetActive(false);
        equippedSpell = spell;
        equippedSpell.SetActive(true);
        equippedSpellIcon.sprite = spell.GetComponent<Spell>().icon;
    }
}