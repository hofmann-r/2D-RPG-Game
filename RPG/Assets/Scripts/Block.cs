using System;
using UnityEngine;

[Serializable]
public class Block {
	[SerializeField]
	private GameObject first, second;

	public void Deactivate() {
		first.SetActive (false);
		first.SetActive (false);
	}

	public void Activate() {
		first.SetActive (true);
		first.SetActive (true);
	}
}
