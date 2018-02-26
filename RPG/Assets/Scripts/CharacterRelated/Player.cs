using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{

	[SerializeField]
	private Stat mana;

	private float maxMana = 50;

	private float manaValue = 50;

	[SerializeField]
	private Block[] blocks;

	[SerializeField]
	private Transform[] exitPoints;

	private int exitIndex = 2;

	private SwordBook swordBook;

	private Vector3 min, max;

	[SerializeField]
	private GameObject[] swordBoxCollider;

	private float swordAttackRange;

	private SwordAttackEnter swordAttackEnter;

	public SwordBook MySwordBook {
		get {
			return swordBook;
		}

		set {
			swordBook = value;
		}
	}

	private static Player instance;

	public static Player MyInstance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<Player> ();
			}

			return instance;
		}
	}

	public float MyManaValue {
		get {
			return manaValue;
		}

		set {
			manaValue = value;
		}
	}

	public float MyMaxMana {
		get {
			return maxMana;
		}

		set {
			maxMana = value;
		}
	}

	public Stat MyMana {
		get {
			return mana;
		}

		set {
			mana = value;
		}
	}

	//private Transform target;


	protected override void Start ()
	{
		//inimigo fixo
		//target = GameObject.Find ("Target").transform;

		swordAttackRange = 1.5f;

		swordBook = GetComponent<SwordBook> ();

		MyMana.Initialize (MyManaValue, MyMaxMana);

		base.Start ();
	}

	protected override void Update ()
	{
		GetInput ();

		transform.position = new Vector3 (Mathf.Clamp (transform.position.x, min.x, max.x),
			Mathf.Clamp (transform.position.y, min.y, max.y),
			transform.position.z);

		if (MyHealth.MyCurrentValue <= 0) {
			Destroy (gameObject);
		}

		base.Update ();
	}

	private void GetInput ()
	{
		Direction = Vector2.zero;


		//DEBUG DA HEALTH DO CHAR
		if (Input.GetKeyDown (KeyCode.I)) {
			if (health.MyCurrentValue > 0) {
				health.MyCurrentValue -= 10;
			}

		}
		if (Input.GetKeyDown (KeyCode.O)) {
			if (health.MyCurrentValue < health.MyMaxValue) {
				health.MyCurrentValue += 10;

			}
		}

		//DEBUG DA MANA DO CHAR
		if (Input.GetKeyDown (KeyCode.J)) {
			if (MyMana.MyCurrentValue > 0) {
				MyMana.MyCurrentValue -= 10;
			}

		}
		if (Input.GetKeyDown (KeyCode.K)) {
			if (MyMana.MyCurrentValue < MyMana.MyMaxValue) {
				MyMana.MyCurrentValue += 10;
			}
		}

		if (Input.GetKey (KeybindManager.MyInstance.Keybinds ["UP"])) {
			exitIndex = 0;
			Direction += Vector2.up;
		}
		if (Input.GetKey (KeybindManager.MyInstance.Keybinds ["LEFT"])) {
			exitIndex = 3;
			Direction += Vector2.left;
		}
		if (Input.GetKey (KeybindManager.MyInstance.Keybinds ["DOWN"])) {
			exitIndex = 2;
			Direction += Vector2.down;
		}
		if (Input.GetKey (KeybindManager.MyInstance.Keybinds ["RIGHT"])) {
			exitIndex = 1;
			Direction += Vector2.right;
		}

		if (Input.GetKeyDown (KeybindManager.MyInstance.Keybinds ["SWRD"])) {
			if (MyTarget != null && !isAttackingSword && !IsMoving) {
				AttackSword ();
				if (swordAttackEnter != null) {
					swordAttackEnter.atacando = true;
				}
			}
		}
		if (Input.GetKeyUp (KeybindManager.MyInstance.Keybinds ["SWRD"])) {
			if (swordAttackEnter != null && swordAttackEnter.attackTimeCounter <= 0) {
				StopAttackSword ();
				swordAttackEnter.atacando = false;
				swordAttackEnter.attackTimeCounter = 0.4f;
			}
		}

		if (IsMoving) {
			StopAttackShield ();
			StopAttackSword ();
		}

		foreach (string action in KeybindManager.MyInstance.Actionbinds.Keys) {
			if (Input.GetKeyDown (KeybindManager.MyInstance.Actionbinds [action])) {
				UIManager.MyInstance.ClickActionButton (action);
			}
		}

	}

	public void SetLimits (Vector3 min, Vector3 max)
	{
		this.min = min;
		this.max = max;
	}

	public override void StopAttackSword ()
	{
		DeactivateAttackSword ();
		base.StopAttackSword ();
	}

	private IEnumerator AttackShield (string spellName)
	{
		Transform currentTarget = MyTarget;

		Spell newSpell = SpellBook.MyInstance.CastSpell (spellName, this);

		if (MyMana.MyCurrentValue >= newSpell.MyManaCost) {
			MyAnimator.SetBool ("attackShield", true);
			isAttackingShield = true;
			yield return new WaitForSeconds (newSpell.MyCastTime); //tempo de cast da magia

			//CastSpell ();

			if (currentTarget != null && InLineOfSight ()) {
				SpellScript s = Instantiate (newSpell.MySpellPrefab, exitPoints [exitIndex].position, Quaternion.identity).GetComponent<SpellScript> ();  //quaternion não deixar rotacionar

				s.Initialize (currentTarget, newSpell.MyDamage, transform);

				MyMana.MyCurrentValue -= newSpell.MyManaCost;
			}

			StopAttackShield ();
		}
	}

	private void AttackSword ()
	{
		Transform currentTarget = MyTarget;

		//Spell newSpell = spellBook.CastSpell(spellIndex);

		MyAnimator.SetBool ("attackSword", true);
		isAttackingSword = true;

		// float distance = Vector2.Distance(MyTarget.position, transform.position);

		// if (distance <= swordAttackRange && InLineOfSight()) {
		//     Debug.Log("Tirando vida do inimigo");
		// }

		ActivateAttackSword (exitIndex);

		//yield return new WaitForSeconds(1); //tempo teste

		//CastSpell ();

		// StopAttackSword();
	}


	public void CastSpell (string spellName)
	{

		Block ();

		if (MyTarget != null && MyTarget.GetComponentInParent<Character> ().IsAlive && !isAttackingShield && !IsMoving && InLineOfSight ()) {
			attackShieldRoutine = StartCoroutine (AttackShield (spellName));
		}

		StopBlock ();

	}

	private bool InLineOfSight ()
	{
		if (MyTarget != null) {
			Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;
			RaycastHit2D hit = Physics2D.Raycast (transform.position, targetDirection, Vector2.Distance (transform.position, MyTarget.transform.position), 256); //8 - block layer 
			if (hit.collider == null) {
				return true;
			}
		}


		return false;
	}

	private void StopBlock ()
	{
		foreach (Block block in blocks) {
			block.Deactivate ();
		}
	}

	private void Block ()
	{
		StopBlock ();

		blocks [exitIndex].Activate ();
	}

	public override void StopAttackShield ()
	{
		SpellBook.MyInstance.StopCasting ();
		base.StopAttackShield ();
	}

	public void ActivateAttackSword (int index)
	{
		swordBoxCollider [index].SetActive (true);
		swordAttackEnter = swordBoxCollider [index].GetComponent<SwordAttackEnter> ();
	}

	public void DeactivateAttackSword ()
	{
		if (swordAttackEnter != null) {
			swordAttackEnter.atacando = false;
		}
		for (int i = 0; i < swordBoxCollider.Length; i++) {
			swordBoxCollider [i].SetActive (false);
		}
	}
}
