using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour {

    [SerializeField]
    private float speed;

    public Animator MyAnimator { get; set; }

    private Vector2 direction;

    private Rigidbody2D myRigidbody;

    protected Coroutine attackShieldRoutine;

    protected bool isAttackingShield = false;
    protected bool isAttackingSword = false;

    public bool IsEnemyAttacking { get; set; }

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

    public bool IsAlive {
        get {
            return health.MyCurrentValue > 0;
        }
    }

    // Use this for initialization
    protected virtual void Start() {

        health.Initialize(initHealth, initHealth);

        MyAnimator = GetComponent<Animator>();
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
        if (IsAlive) {
            myRigidbody.velocity = Direction.normalized * Speed;

        }
    }

    public void HandleLayers() {
        if (IsAlive) {
            if (IsMoving) {
                ActivateLayer("WalkLayer");
                MyAnimator.SetFloat("x", Direction.x);
                MyAnimator.SetFloat("y", Direction.y);
            } else if (isAttackingShield) {
                ActivateLayer("AttackShieldLayer");
            } else if (isAttackingSword) {
                ActivateLayer("AttackSwordLayer");
            } else if (IsEnemyAttacking) {
                ActivateLayer("AttackEnemyLayer");
            } else {
                ActivateLayer("IdleLayer");
            }
        } else {
            ActivateLayer("DeathLayer");
        }
    }

    public virtual void StopAttackShield() {
        if (attackShieldRoutine != null) {
            StopCoroutine(attackShieldRoutine);
            isAttackingShield = false;
            MyAnimator.SetBool("attackShield", false);
        }
    }

    public virtual void StopAttackSword() {
        isAttackingSword = false;
        MyAnimator.SetBool("attackSword", false);

    }

    public void ActivateLayer(string layerName) {
        for (int i = 0; i < MyAnimator.layerCount; i++) {
            MyAnimator.SetLayerWeight(i, 0);
        }
        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), 1);
    }

    public virtual void TakeDamage(int damage) {
        health.MyCurrentValue -= damage;

        if (health.MyCurrentValue <= 0) {
            Direction = Vector2.zero;
            myRigidbody.velocity = Direction;
            MyAnimator.SetTrigger("die");
        }
    }

}
