using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour {

	[SerializeField]
	private string type;

	private void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.tag == "Player" && type == "bag" && InventoryScript.MyInstance.CanAddBag) {
			Bag bag = (Bag)Instantiate(InventoryScript.MyInstance.MyItems[0]);
			bag.Initialize(16);
			bag.Use();
			Destroy (gameObject);
		} 

		if (collision.tag == "Player" && type == "health" && !InventoryScript.MyInstance.BagsEmpty) {
			HealthPotion potion = (HealthPotion)Instantiate(InventoryScript.MyInstance.MyItems[1]);
			InventoryScript.MyInstance.AddItem(potion);
			Destroy (gameObject);
		} 

		if (collision.tag == "Player" && type == "mana" && !InventoryScript.MyInstance.BagsEmpty) {
			ManaPotion potion = (ManaPotion)Instantiate(InventoryScript.MyInstance.MyItems[2]);
			InventoryScript.MyInstance.AddItem(potion);
			Destroy (gameObject);
		} 
	}
}
