using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ManaPotion", menuName = "Items/ManaPotion", order = 2)]
public class ManaPotion : Item, IUsable {

    [SerializeField]
    private int mana;

    public void Use() {
        if (Player.MyInstance.MyMana.MyCurrentValue < Player.MyInstance.MyMana.MyMaxValue) {
            Remove();
            Player.MyInstance.MyMana.MyCurrentValue += mana;
        }
    }
}
