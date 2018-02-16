using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour {

    [SerializeField]
    private float speed;

    protected Animator myAnimator;

    private Vector2 direction;

    private Rigidbody2D myRigidbody;

    protected Coroutine attackShieldRoutine;

    protected bool isAttackingShield = false;
    protected bool isAttackingSword = false;
    protected bool isEnemyAttacking = false;

    [SerializeField]
    private float initHealth;

    [SerializeField]
    protected Transform hitBox;

    [SerializeField]
    protected Stat health;

    public Stat MyHealth {
        get {
            return health;
        }
    }

    public bool IsMoving {
        get {
            return Direction.x != 0 || Direction.y != 0;
        }
    }

    public Vector2 Direction {
        get {
            return direction;
        }

        set {
            direction = value;
        }
    }

    public float Speed {
        get {
            return speed;
        }

        set {
            speed = value;
        }
    }

    // Use this for initialization
    protected virtual void Start() {

        health.Initialize(initHealth, initHealth);

        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update() {
        HandleLayers();
    }

    private void FixedUpdate() {
        Move();
    }

    public void Move() {
        //old
        //transform.Translate (direction * speed * Time.deltaTime);
        myRigidbody.velocity = Direction.normalized * Speed;
    }

    public void HandleLayers() {
        if (IsMoving) {
            ActivateLayer("WalkLayer");
            myAnimator.SetFloat("x", Direction.x);
            myAnimator.SetFloat("y", Direction.y);
        } else if (isAttackingShield) {
            ActivateLayer("AttackShieldLayer");
        } else if (isAttackingSword) {
            ActivateLayer("AttackSwordLayer");
        } else if (isEnemyAttacking) {
            ActivateLayer("AttackEnemyLayer");
        } else {
            ActivateLayer("IdleLayer");
        }
    }

    public virtual void StopAttackShield() {
        if (attackShieldRoutine != null) {
            StopCoroutine(attackShieldRoutine);
            isAttackingShield = false;
            myAnimator.SetBool("attackShield", false);
        }
    }

    public virtual void StopAttackSword() {
        isAttackingSword = false;
        myAnimator.SetBool("attackSword", false);

    }

    public void ActivateLayer(string layerName) {
        for (int i = 0; i < myAnimator.layerCount; i++) {
            myAnimator.SetLayerWeight(i, 0);
        }
        myAnimator.SetLayerWeight(myAnimator.GetLayerIndex(layerName), 1);
    }

    public virtual void TakeDamage(int damage) {
        health.MyCurrentValue -= damage;

        if (health.MyCurrentValue <= 0) {
            myAnimator.SetTrigger("die");
        }
    }

}
