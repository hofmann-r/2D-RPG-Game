using System.Collections;
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

    private SwordBook swordBook;

    private Vector3 min, max;

    [SerializeField]
    private GameObject[] swordBoxCollider;

    private float swordAttackRange;

    [SerializeField]
    private GameObject enemyAttackCollider;

    private SwordAttackEnter swordAttackEnter;

    public SwordBook MySwordBook {
        get {
            return swordBook;
        }

        set {
            swordBook = value;
        }
    }

    //private Transform target;


    protected override void Start() {
        //inimigo fixo
        //target = GameObject.Find ("Target").transform;

        swordAttackRange = 1.5f;

        spellBook = GetComponent<SpellBook>();

        swordBook = GetComponent<SwordBook>();

        mana.Initialize(manaValue, maxMana);

        base.Start();
    }

    protected override void Update() {
        GetInput();

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x),
            Mathf.Clamp(transform.position.y, min.y, max.y),
            transform.position.z);

        base.Update();
    }

    private void GetInput() {
        Direction = Vector2.zero;


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
            Direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A)) {
            exitIndex = 3;
            Direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S)) {
            exitIndex = 2;
            Direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D)) {
            exitIndex = 1;
            Direction += Vector2.right;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (MyTarget != null && !isAttackingSword && !IsMoving) {
                AttackSword();
                enemyAttackCollider.SetActive(true);
                if (swordAttackEnter != null) {
                    swordAttackEnter.atacando = true;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Space)) {
            if (enemyAttackCollider != null) {
                enemyAttackCollider.SetActive(false);
            }
            if (swordAttackEnter != null && swordAttackEnter.attackTimeCounter <= 0) {
                StopAttackSword();
                swordAttackEnter.atacando = false;
                swordAttackEnter.attackTimeCounter = swordAttackEnter.attackTime;
            }
        }

        if (IsMoving) {
            StopAttackShield();
            StopAttackSword();
        }

    }

    public void SetLimits(Vector3 min, Vector3 max) {
        this.min = min;
        this.max = max;
    }

    public override void StopAttackSword() {
        DeactivateAttackSword();
        base.StopAttackSword();
    }

    private IEnumerator AttackShield(int spellIndex) {
        Transform currentTarget = MyTarget;

        Spell newSpell = spellBook.CastSpell(spellIndex);

        MyAnimator.SetBool("attackShield", true);
        isAttackingShield = true;
        yield return new WaitForSeconds(newSpell.MyCastTime); //tempo de cast da magia

        //CastSpell ();

        if (currentTarget != null && InLineOfSight()) {
            SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();  //quaternion não deixar rotacionar

            s.Initialize(currentTarget, newSpell.MyDamage, transform);
        }

        StopAttackShield();
    }

    private void AttackSword() {
        Transform currentTarget = MyTarget;

        //Spell newSpell = spellBook.CastSpell(spellIndex);

        MyAnimator.SetBool("attackSword", true);
        isAttackingSword = true;

       // float distance = Vector2.Distance(MyTarget.position, transform.position);

       // if (distance <= swordAttackRange && InLineOfSight()) {
       //     Debug.Log("Tirando vida do inimigo");
       // }

        ActivateAttackSword(exitIndex);

        //yield return new WaitForSeconds(1); //tempo teste

        //CastSpell ();

        // StopAttackSword();
    }


    public void CastSpell(int spellIndex) {

        Block();

        if (MyTarget != null && MyTarget.GetComponentInParent<Character>().IsAlive && !isAttackingShield && !IsMoving && InLineOfSight()) {
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

    public void ActivateAttackSword(int index) {
        swordBoxCollider[index].SetActive(true);
        swordAttackEnter = swordBoxCollider[index].GetComponent<SwordAttackEnter>();
    }

    public void DeactivateAttackSword() {
        if (swordAttackEnter != null) {
            swordAttackEnter.atacando = false;
        }
        for (int i = 0; i < swordBoxCollider.Length; i++) {
            swordBoxCollider[i].SetActive(false);
        }
    }
}
