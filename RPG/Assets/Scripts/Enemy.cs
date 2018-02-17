using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC {

    [SerializeField]
    private CanvasGroup healthGroup;

    private Transform target;

    private IState currentState;

    public float MyAttackRange { get; set; }

    public float MyAttacktime { get; set; }

    public Transform Target {
        get {
            return target;
        }

        set {
            target = value;
        }
    }

    protected void Awake() {
        MyAttackRange = 1.5f;
        ChangeState(new IdleState());
    }

    protected override void Update() {
        transform.rotation = Quaternion.identity;
        if (IsAlive) {

            if (!IsEnemyAttacking) {
                MyAttacktime += Time.deltaTime;
            }

            currentState.Update();
        }
        base.Update();
    }

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

    public void ChangeState(IState newState) {
        if (currentState != null) {
            currentState.Exit();
        }
        currentState = newState;

        currentState.Enter(this);
    }
}
