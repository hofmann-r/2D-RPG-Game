using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class AttackState : IState {

    private Enemy parent;

    private float attackCooldown = 3;

    private float extraRange = .1f; //range para comear a seguir o player

    private Player player;

    public void Enter(Enemy parent) {
        this.parent = parent;
    }

    public void Exit() {
    }

    public void Update() {

        if(parent.MyAttacktime >= attackCooldown && !parent.IsEnemyAttacking) {

            parent.MyAttacktime = 0;

            parent.StartCoroutine(Attack());
        }

        if(parent.MyTarget != null) {
            //testar alcance e atacar
            float distance = Vector2.Distance(parent.MyTarget.position, parent.transform.position);

            if (distance >= parent.MyAttackRange+extraRange && !parent.IsEnemyAttacking) {
                parent.ChangeState(new FollowState());
            }
        } else {
            parent.ChangeState(new IdleState());
        }
    }

    public IEnumerator Attack() {

        parent.IsEnemyAttacking = true;
       // MyTar = collision.GetComponentInParent<Character>();
        parent.MyAnimator.SetTrigger("attackEnemy");
        player = parent.MyTarget.GetComponent<Player>();
        player.MyHealth.MyCurrentValue -= 3;
        yield return new WaitForSeconds(parent.MyAnimator.GetCurrentAnimatorStateInfo(2).length);

        parent.IsEnemyAttacking = false;


    }
}
