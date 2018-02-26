using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BagButton : MonoBehaviour, IPointerClickHandler {

    private Bag bag;

    [SerializeField]
    private Sprite full, empty;

    public Bag MyBag {
        get {
            return bag;
        }

        set {
            if (value != null) {
                GetComponent<Image>().sprite = full;
            } else {
                GetComponent<Image>().sprite = empty;
            }
            bag = value;
        }
    }

    public void OnPointerClick(PointerEventData eventData) {

        if (eventData.button == PointerEventData.InputButton.Left) {

            //  if (Input.GetKey(KeyCode.LeftShift)) {
            HandScript.MyInstance.TakeMoveable(MyBag);
            // } 
        }

        if (eventData.button == PointerEventData.InputButton.Right) {
            if (bag != null) {
                bag.MyBagScript.OpenClose();
            }
        }
    }

    public void RemoveBag() {
        InventoryScript.MyInstance.RemoveBag(MyBag);
        MyBag.MyBagButton = null;
        MyBag = null;
    }
}
