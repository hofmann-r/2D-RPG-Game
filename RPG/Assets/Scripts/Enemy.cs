using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC {

    [SerializeField]
    private CanvasGroup healthGroup;

    public override Transform Select() {

        healthGroup.alpha = 1;

        return base.Select();
    }

    public override void DeSelect() {

        healthGroup.alpha = 0;

        base.DeSelect();
    }

    public override void TakeDamage(int damage) {
        base.TakeDamage(damage);

        OnHealthChanged(health.MyCurrentValue);
        
    }

}
