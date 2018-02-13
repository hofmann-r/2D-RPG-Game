﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {


    [SerializeField]
    private Stat mana;

   



    private float maxMana = 50;

    private float manaValue = 50;

    [SerializeField]
    private Block[] blocks;

    [SerializeField]
    private Transform[] exitPoints;

    private int exitIndex = 2;

    private SpellBook spellBook;



    //private Transform target;

    public Transform MyTarget { get; set; }



    protected override void Start() {
        //inimigo fixo
        //target = GameObject.Find ("Target").transform;

        spellBook = GetComponent<SpellBook>();
        

        mana.Initialize(manaValue, maxMana);



        base.Start();
    }

    protected override void Update() {
        GetInput();

        base.Update();
    }

    private void GetInput() {
        direction = Vector2.zero;


        //DEBUG DA HEALTH DO CHAR
        if (Input.GetKeyDown(KeyCode.I)) {
            if (health.MyCurrentValue > 0) {
                health.MyCurrentValue -= 10;
            }

        }
        if (Input.GetKeyDown(KeyCode.O)) {
            if (health.MyCurrentValue < health.MyMaxValue) {
                health.MyCurrentValue += 10;

            }
        }

        //DEBUG DA MANA DO CHAR
        if (Input.GetKeyDown(KeyCode.J)) {
            if (mana.MyCurrentValue > 0) {
                mana.MyCurrentValue -= 10;
            }

        }
        if (Input.GetKeyDown(KeyCode.K)) {
            if (mana.MyCurrentValue < mana.MyMaxValue) {
                mana.MyCurrentValue += 10;

            }
        }

        if (Input.GetKey(KeyCode.W)) {
            exitIndex = 0;
            direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A)) {
            exitIndex = 3;
            direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S)) {
            exitIndex = 2;
            direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D)) {
            exitIndex = 1;
            direction += Vector2.right;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {

        }

    }

    private IEnumerator AttackShield(int spellIndex) {
        Transform currentTarget = MyTarget;

        Spell newSpell = spellBook.CastSpell(spellIndex);

        myAnimator.SetBool("attackShield", true);
        isAttackingShield = true;
        yield return new WaitForSeconds(newSpell.MyCastTime); //tempo de cast da magia

        //CastSpell ();

        if (currentTarget != null && InLineOfSight()) {
            SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();  //quaternion não deixar rotacionar

            s.Initialize(currentTarget, newSpell.MyDamage);
        }

        StopAttackShield();
    }

    public void CastSpell(int spellIndex) {

        Block();

        if (MyTarget != null && !isAttackingShield && !IsMoving && InLineOfSight()) {
            attackShieldRoutine = StartCoroutine(AttackShield(spellIndex));
        }

    }

    private bool InLineOfSight() {
        if (MyTarget != null) {
            Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), 256); //8 - block layer 
            if (hit.collider == null) {
                return true;
            }
        }


        return false;
    }

    private void Block() {
        foreach (Block block in blocks) {
            block.Deactivate();
        }

        blocks[exitIndex].Activate();
    }

    public override void StopAttackShield() {
        spellBook.StopCasting();
        base.StopAttackShield();
    }
}
