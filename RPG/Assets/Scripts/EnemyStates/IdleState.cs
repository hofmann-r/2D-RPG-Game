using UnityEngine;
using System.Collections;

class IdleState : IState {

    private Enemy parent;

    public void Enter(Enemy parent) {
        this.parent = parent;
        this.parent.Reset();
    }

    public void Exit() {
    }

    public void Update() {
        //trocar para follow state se o player estiver próximo
        if (parent.MyTarget != null) {
            parent.ChangeState(new FollowState());
        }
       // parent.transform.rotation = Quaternion.identity;

    }
}
