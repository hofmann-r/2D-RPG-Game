using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	[SerializeField]
	private Player player;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		ClickTarget ();
	}

	private void ClickTarget ()
	{
		if (Input.GetMouseButtonDown (0)) {
			//setar o alvo
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);

			if (hit.collider != null) {
				//ataca apenas tags que contenham inimigos
				//FUTURO: colocar tags de objetos para destruir e achar itens
				if (hit.collider.tag == "Enemy") {
					player.MyTarget = hit.transform;
				}
			} else {
				//tirar o alvo
				player.MyTarget = null;
			}
	
		} 
	}
}
