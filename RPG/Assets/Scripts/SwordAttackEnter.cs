using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttackEnter : MonoBehaviour
{

	public Coroutine AttackSwordRoutine { get; set; }

	private bool atacando = false;

	private Transform collision;

	private Player parent;

	private float attackTime = 0.7f;

	private float attackTimeCounter;

	private Character cTarget;
	private void Start ()
	{
		parent = GetComponentInParent<Player> ();
		attackTimeCounter = attackTime;
	}

	private void Update() {
		if (attackTimeCounter > 0 && atacando) {
			Debug.Log ("timer ataque");
			attackTimeCounter -= Time.deltaTime;
		} else if (attackTimeCounter <= 0) {
			attackTimeCounter = attackTime;
			Debug.Log ("fim ataque");
			AttackSword ();
			parent.StopAttackSword ();
			atacando = false;
		} else {
			attackTimeCounter = attackTime;
		}

		Debug.Log (attackTimeCounter);

		if (Input.GetKeyDown(KeyCode.Space) && cTarget != null) {
			atacando = true;
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			atacando = false;
			attackTimeCounter = attackTime;
			parent.StopAttackSword ();
		}

	}

	private void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.tag == "SwordAttack") {
			Debug.Log ("enter collision");
			cTarget = collision.GetComponentInParent<Character> ();
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
		atacando = false;
		attackTimeCounter = attackTime;
	}

	private void OnTriggerStay2D (Collider2D collision)
	{
		if (collision.tag == "SwordAttack" && cTarget == null) {
			cTarget = collision.GetComponentInParent<Character> ();
		}
	}
}
