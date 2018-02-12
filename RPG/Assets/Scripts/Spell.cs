using System;
using UnityEngine;

[Serializable]
public class Spell
{
	[SerializeField]
	private string name;

	[SerializeField]
	private int damage;

	[SerializeField]
	private Sprite icon;

	[SerializeField]
	private float speed;

	[SerializeField]
	private float castTime;

	[SerializeField]
	private GameObject spellPrefab;

	[SerializeField]
	private Color barColor;
}

