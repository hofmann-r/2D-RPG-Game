using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is an abstract class that all characters needs to inherit from
/// </summary>
public abstract class Character : MonoBehaviour
{

    [SerializeField]
    private float speed;

    protected Animator myAnimator;

    protected Vector2 direction;

    private Rigidbody2D myRigidbody;

    protected Coroutine attackShieldRoutine;
    protected bool isAttackingShield = false;

    public bool IsMoving {
        get {
            return direction.x != 0 || direction.y != 0;
        }
    }
    // Use this for initialization
    protected virtual void Start()
    {

        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        HandleLayers();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        //old
        //transform.Translate (direction * speed * Time.deltaTime);
        myRigidbody.velocity = direction.normalized * speed;
    }

    public void HandleLayers()
    {
        if (IsMoving) {
            ActivateLayer("WalkLayer");
            myAnimator.SetFloat("x", direction.x);
            myAnimator.SetFloat("y", direction.y);

            StopAttackShield();
        } else if (isAttackingShield) {
            ActivateLayer("AttackShieldLayer");
        } else {
            ActivateLayer("IdleLayer");
        }
    }

    public void StopAttackShield()
    {
        if (attackShieldRoutine != null) {
            StopCoroutine(attackShieldRoutine);
            isAttackingShield = false;
            myAnimator.SetBool("attackShield", false);
        }

    }

    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < myAnimator.layerCount; i++) {
            myAnimator.SetLayerWeight(i, 0);
        }
        myAnimator.SetLayerWeight(myAnimator.GetLayerIndex(layerName), 1);
    }

}
