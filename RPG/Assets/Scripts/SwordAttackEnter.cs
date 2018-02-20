using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttackEnter : MonoBehaviour
{

	public Coroutine AttackSwordRoutine { get; set; }

	public bool atacando = false;

	private Transform collision;

	private Player parent;

	public float attackTime = 0.4f;

	public float attackTimeCounter;

	private Character cTarget;

	private void Start ()
	{
		parent = GetComponentInParent<Player> ();
		attackTimeCounter = attackTime;
	}

	private void Update ()
	{
		if ((attackTimeCounter > 0) && atacando) {
			attackTimeCounter -= Time.deltaTime;
		} else if (attackTimeCounter <= 0) {
			attackTimeCounter = attackTime;
			AttackSword ();
			parent.StopAttackSword ();
			atacando = false;
		} else {
			attackTimeCounter = attackTime;
		}
	}

	private void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.tag == "SwordAttack" && cTarget == null) {
			cTarget = collision.GetComponentInParent<Character> ();
		} else {
			cTarget = null;
		}
	}

	private void AttackSword ()
	{
		if (cTarget != null) {
			cTarget.TakeDamage (parent.MySwordBook.GetEquipedSword ().MyDamage, parent.transform);
		}
	}

	private void OnTriggerExit2D (Collider2D collision)
	{
		// atacando = false;
		attackTimeCounter = attackTime;
	}

	private void OnTriggerStay2D (Collider2D collision)
	{
		if (collision.tag == "SwordAttack") {
			cTarget = collision.GetComponentInParent<Character> ();
		} 
	}
}
