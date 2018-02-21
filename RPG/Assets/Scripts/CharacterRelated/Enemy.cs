using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC {

    [SerializeField]
    private CanvasGroup healthGroup;

    private IState currentState;

    public float MyAttackRange { get; set; }

    public float MyAttacktime { get; set; }

    public Vector3 MyStartPosition { get; set; }

    public float MyAggroRange { get; set; }

    public bool InRange {
        get {
            return Vector2.Distance(transform.position, MyTarget.position) < MyAggroRange;
        }
    }

    [SerializeField]
    private float initAggroRange;

    protected void Awake() {
        MyAggroRange = initAggroRange;

        MyStartPosition = transform.position;

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

    public override void TakeDamage(float damage, Transform source) {

        if (!(currentState is EvadeState)) {

            SetTarget(source);
            base.TakeDamage(damage, source);

            OnHealthChanged(health.MyCurrentValue);
        }

    }

    public void ChangeState(IState newState) {
        if (currentState != null) {
            currentState.Exit();
        }
        currentState = newState;

        currentState.Enter(this);
    }

    public void SetTarget(Transform target) {
        if (MyTarget == null && !(currentState is EvadeState)) {
            float distance = Vector2.Distance(transform.position, target.position);

            MyAggroRange = initAggroRange;

            MyAggroRange += distance;

            MyTarget = target;
        }
    }

    public void Reset() {
        this.MyTarget = null;
        this.MyAggroRange = initAggroRange;
        this.MyHealth.MyCurrentValue = this.MyHealth.MyMaxValue;
        OnHealthChanged(health.MyCurrentValue);
    }

}
