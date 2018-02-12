using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBook : MonoBehaviour
{

	[SerializeField]
	private Spell[] spells;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public Spell CastSpell (int index)
	{
		

		return spells [index];
	}
}
