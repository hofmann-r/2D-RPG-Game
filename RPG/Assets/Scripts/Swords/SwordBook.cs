using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBook : MonoBehaviour {

    [SerializeField]
    private Swords[] swords;
    private int equipedSword = 0;

    private Swords equiped;

    private void Start() {
        equiped = swords[equipedSword];
    }
    public Swords GetEquipedSword() {
        return equiped;
    }

    public void SetEquipedSword(int e) {
        equiped = swords[e];
    }
}
