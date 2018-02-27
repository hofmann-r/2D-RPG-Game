using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void HealthChanged(float health);

public delegate void CharacterRemoved();

public class NPC : Character {

    public event HealthChanged healthChanged;

    public event CharacterRemoved characterRemoved;

	[SerializeField]
	private Transform[] items;

    [SerializeField]
    private Sprite portrait;

    public Sprite MyPortrait {
        get {
            return portrait;
        }
    }

    public virtual void DeSelect() {
        healthChanged -= new HealthChanged(UIManager.MyInstance.UpdateTargetFrame);

        characterRemoved -= new CharacterRemoved(UIManager.MyInstance.HideTargetFrame);
    }

    public virtual Transform Select() {


        return hitBox;
    }

    public void OnHealthChanged(float health) {
        if (healthChanged != null) {
            healthChanged(health);
        }
    }

    public void OnCharacterRemoved() {
        if (characterRemoved != null) {
			int randomNumber = Random.Range (1, 11);
			if (randomNumber == 1 || randomNumber == 2) {
				Instantiate (items [0], transform.position, Quaternion.identity);
			}

			if(randomNumber == 3 || randomNumber == 4){
				Instantiate (items [1], transform.position, Quaternion.identity);
			}

			//inimigos não vão dropar bags
		/*	if(randomNumber == 7){
				Instantiate (items [2], transform.position, Quaternion.identity);
			}*/


            characterRemoved();
        }

        Destroy(gameObject);
    }
}
