using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class AttackState : IState {

    private Enemy parent;

    public void Enter(Enemy parent) {
        this.parent = parent;
    }

    public void Exit() {
    }

    public void Update() {
        if(parent.Target != null) {
            //testar alcance e atacar
            float distance = Vector2.Distance(parent.Target.position, parent.transform.position);
            if(distance >= parent.MyAttackRange) {
                parent.ChangeState(new FollowState());
            }
        } else {
            parent.ChangeState(new IdleState());
        }
    }
}
