using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour {

    private static InventoryScript instance;

    [SerializeField]
    private BagButton[] bagButtons;

    public static InventoryScript MyInstance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<InventoryScript>();
            }

            return instance;
        }
    }

    private List<Bag> bags = new List<Bag>();

    [SerializeField]
    private Item[] items;

    public bool CanAddBag {
        get {
            return bags.Count < 3;
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.V)) { //debug
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(16);
            bag.Use();
        }

        if (Input.GetKeyDown(KeyCode.C)) { //debug
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(16);
            AddItem(bag);
        }

        if (Input.GetKeyDown(KeyCode.H)) { //debug
            HealthPotion potion = (HealthPotion)Instantiate(items[1]);
            AddItem(potion);
        }
        if (Input.GetKeyDown(KeyCode.Y)) { //debug
            ManaPotion potion = (ManaPotion)Instantiate(items[2]);
            AddItem(potion);
        }
    }

    private void Awake() {
        Bag bag = (Bag)Instantiate(items[0]);
        bag.Initialize(16);
        bag.Use();
    }

    public void AddItem(Item item) {
        if(item.MyStackSize > 0) {
            if (PlaceInStack(item)) {
                return;
            }
        }
        PlaceInEmpty(item);
    }


    public void OpenClose() {
        bool closedBag = bags.Find(x => !x.MyBagScript.IsOpen);
        //se for true, abre todos os bags
        //se for false, fecha todos os bags
        foreach (Bag bag in bags) {
            if (bag.MyBagScript.IsOpen != closedBag) {
                bag.MyBagScript.OpenClose();
            }
        }

    }

    public void AddBag(Bag bag) {
        foreach (BagButton bagButton in bagButtons) {
            if (bagButton.MyBag == null) {
                bagButton.MyBag = bag;
                bags.Add(bag);
                break;
            }
        }
    }

    private void PlaceInEmpty(Item item) {
        foreach (Bag  bag in bags) {
            if (bag.MyBagScript.AddItem(item)) {
                return;
            }
        }
    }

    private bool PlaceInStack(Item item) {
        foreach (Bag bag in bags) {
            foreach (SlotScript slots in bag.MyBagScript.MySlots) {
                if (slots.StackItem(item)) {
                    return true;
                }
            }
        }
        return false;
    }
}
