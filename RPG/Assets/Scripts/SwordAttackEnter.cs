using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttackEnter : MonoBehaviour {

    public Coroutine AttackSwordRoutine { get; set; }
    private bool atacando = false;

    Player parent;

    private void Start() {
        parent = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "SwordAttack") {
            if (Input.GetKey(KeyCode.Space) && !atacando) {
                AttackSwordRoutine = StartCoroutine(Attack(collision));
                atacando = true;
            }
            if (Input.GetKeyUp(KeyCode.Space) && atacando) {
                StopCoroutine(AttackSwordRoutine);

            }
        }
    }

    private IEnumerator Attack(Collider2D collision) {
        Debug.Log("Colidindo");
        //atacando = true;
        yield return new WaitForSeconds(0.35f);
        atacando = false;
        Debug.Log("Fim colisao");
        Character c = collision.GetComponentInParent<Character>();
        c.TakeDamage(parent.MySwordBook.GetEquipedSword().MyDamage, parent.transform);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (AttackSwordRoutine != null) {
            StopCoroutine(AttackSwordRoutine);
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "SwordAttack") {
            if (Input.GetKey(KeyCode.Space) && !atacando) {
                AttackSwordRoutine = StartCoroutine(Attack(collision));
                atacando = true;
            }
            if (Input.GetKeyUp(KeyCode.Space) && atacando) {
                StopCoroutine(AttackSwordRoutine);

            }
        }
    }
}
