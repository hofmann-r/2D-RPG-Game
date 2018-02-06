using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{

	private Rigidbody2D myRigidBody;

	[SerializeField]
	private float speed;

	private Transform target;


	// Use this for initialization
	void Start ()
	{
		myRigidBody = GetComponent<Rigidbody2D> ();	
		//TESTE APENAS
		target = GameObject.Find ("Target").transform;
	}

	public void Fire ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	private void FixedUpdate ()
	{
		Vector2 direction = target.position - transform.position;
		myRigidBody.velocity = direction.normalized * speed;

		float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;

		transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
	}
}
