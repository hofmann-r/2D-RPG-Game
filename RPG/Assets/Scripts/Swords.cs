using System;
using UnityEngine;

[Serializable]
public class Swords {

    [SerializeField]
    private string name;

    [SerializeField]
    private int damage;

    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private float range;

    public string MyName {
        get {
            return name;
        }

        set {
            name = value;
        }
    }

    public int MyDamage {
        get {
            return damage;
        }

        set {
            damage = value;
        }
    }

    public Sprite MyIcon {
        get {
            return icon;
        }

        set {
            icon = value;
        }
    }

    public float MyRange {
        get {
            return range;
        }

        set {
            range = value;
        }
    }
}
