using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Caster : MonoBehaviour {

<<<<<<< HEAD
    public SteamVR_TrackedController menuController;
    public SteamVR_TrackedController castController;
    public Image manaBar;
    public Image equippedSpellIcon;
    public float maxMana = 100f;
    public float manaRegen = 10f;
    public float mana;

    GameObject equippedSpell;
    Color manaColor;
=======
    public Image manaBar;
    public float maxMana = 100f;
    public float manaRegen = 10f;

    Color manaColor;
    float mana;
>>>>>>> 0c7af0f844669d3799a247222e1c46432d916e20

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
<<<<<<< HEAD
    }

    public void EquipSpell(GameObject spell) {
        if (equippedSpell != null)
            equippedSpell.SetActive(false);
        equippedSpell = spell;
        equippedSpell.SetActive(true);
        equippedSpellIcon.sprite = spell.GetComponent<Spell>().icon;
=======
    }

    public void ResetMana() {
        mana = maxMana;
    }

    public void TryCast(Spell spell, Controller controller) {
        if (mana > spell.manaCost) {
            mana -= spell.manaCost;
            spell.Cast(controller);
        }
>>>>>>> 0c7af0f844669d3799a247222e1c46432d916e20
    }
}