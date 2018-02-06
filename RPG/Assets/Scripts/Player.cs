using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
	[SerializeField]
	private Stat health;

	[SerializeField]
	private Stat mana;

	private float maxHealth = 100;

	private float healthValue = 100;

	private float maxMana = 50;

	private float manaValue = 50;

	[SerializeField]
	private GameObject[] spellPrefab;

	[SerializeField]
	private Transform[] exitPoints;

	private int exitIndex = 2;

	protected override void Start ()
	{
		health.Initialize (healthValue, maxHealth);

		mana.Initialize (manaValue, maxMana);

		base.Start ();
	}

	protected override void Update ()
	{
		GetInput ();

		base.Update ();
	}

	private void GetInput ()
	{
		direction = Vector2.zero;


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
			if (mana.MyCurrentValue > 0) {
				mana.MyCurrentValue -= 10;
			}

		}
		if (Input.GetKeyDown (KeyCode.K)) {
			if (mana.MyCurrentValue < mana.MyMaxValue) {
				mana.MyCurrentValue += 10;

			}
		}

		if (Input.GetKey (KeyCode.W)) {
			exitIndex = 0;
			direction += Vector2.up;
		}
		if (Input.GetKey (KeyCode.A)) {
			exitIndex = 3;
			direction += Vector2.left;
		}
		if (Input.GetKey (KeyCode.S)) {
			exitIndex = 2;
			direction += Vector2.down;
		}
		if (Input.GetKey (KeyCode.D)) {
			exitIndex = 1;
			direction += Vector2.right;
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			if (!isAttackingShield && !IsMoving) {
				attackShieldRoutine = StartCoroutine (AttackShield ());
			}
		}

	}

	private IEnumerator AttackShield ()
	{

		myAnimator.SetBool ("attackShield", true);
		isAttackingShield = true;
		yield return new WaitForSeconds (1); //tempo de cast da magia
		CastSpell();

		StopAttackShield ();
	}

	public void CastSpell ()
	{
		Instantiate (spellPrefab [0], exitPoints[exitIndex].position, Quaternion.identity);  //quaternion não deixar rotacionar
	}
}
