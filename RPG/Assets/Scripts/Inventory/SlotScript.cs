using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable {

    private ObservableStack<Item> items = new ObservableStack<Item>();

    [SerializeField]
    private Image icon;

    public bool IsEmpty {
        get {
            return items.Count == 0;
        }
    }

    public bool IsFull {
        get {
            if (IsEmpty) {
                return false;
            } else if (MyCount < MyItem.MyStackSize) {
                return false;
            }

            return true;

        }
    }
    public Item MyItem {
        get {
            if (!IsEmpty) {
                return items.Peek();
            }

            return null;
        }
    }

    public Image MyIcon {
        get {
            return icon;
        }

        set {
            icon = value;
        }
    }

    public int MyCount {
        get {
            return items.Count;
        }
    }

    //referência ao bag que o slot pertence
    public BagScript MyBag { get; set; }

    [SerializeField]
    private Text stackSize;

    public Text MyStackSize {
        get {
            return stackSize;
        }
    }

    public bool AddItem(Item item) {

        items.Push(item);
        icon.sprite = item.MyIcon;
        icon.color = Color.white;
        item.MySlot = this;
        return true;
    }

    public bool AddItems(ObservableStack<Item> newItems) {

        if (IsEmpty || newItems.Peek().GetType() == MyItem.GetType()) {
            int count = newItems.Count;
            for (int i = 0; i < count; i++) {
                if (IsFull) {
                    return false;
                }
                AddItem(newItems.Pop());
            }
            return true;
        }

        return false;
    }

    public void RemoveItem(Item item) {
        if (!IsEmpty) {
            items.Pop();

        }
    }

    private void Awake() {
        items.OnPop += new UpdateStackEvent(UpdateSlot);
        items.OnPush += new UpdateStackEvent(UpdateSlot);
        items.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    public void OnPointerClick(PointerEventData eventData) {

        if (eventData.button == PointerEventData.InputButton.Left) {
            if (InventoryScript.MyInstance.FromSlot == null && !IsEmpty) { //se não tiver item, pega
                HandScript.MyInstance.TakeMoveable(MyItem as IMoveable); //mover item
                InventoryScript.MyInstance.FromSlot = this;
            } else if (InventoryScript.MyInstance.FromSlot == null && IsEmpty && (HandScript.MyInstance.MyMoveable is Bag)) {
                //desequipa um bag
                Bag bag = (Bag)HandScript.MyInstance.MyMoveable;

                if (bag.MyBagScript != MyBag && InventoryScript.MyInstance.MyEmptySlotCount - bag.Slots > 0) {
                    AddItem(bag);
                    bag.MyBagButton.RemoveBag();
                    HandScript.MyInstance.Drop();
                }

            } else if (InventoryScript.MyInstance.FromSlot != null) { //se tiver, deixe ele no lugar selecionado
                if (PutItemBack() ||
                    MergeItems(InventoryScript.MyInstance.FromSlot) ||
                    SwapItems(InventoryScript.MyInstance.FromSlot) ||
                    AddItems(InventoryScript.MyInstance.FromSlot.items)) {
                    HandScript.MyInstance.Drop();
                    InventoryScript.MyInstance.FromSlot = null;
                }
            }
        }

        if (eventData.button == PointerEventData.InputButton.Right) {
            UseItem();
        }
    }

    public void UseItem() {
        if (MyItem is IUsable) {
            (MyItem as IUsable).Use();
        }
    }

    public bool StackItem(Item item) {
        if (!IsEmpty && item.name == MyItem.name && items.Count < MyItem.MyStackSize) {
            items.Push(item);
            item.MySlot = this;
            return true;
        }
        return false;
    }

    private void UpdateSlot() {
        UIManager.MyInstance.UpdateStackSize(this);
    }

    public bool PutItemBack() {
        if (InventoryScript.MyInstance.FromSlot == this) {
            InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
            return true;
        }

        return false;
    }

    private bool SwapItems(SlotScript from) {
        if (IsEmpty) {
            return false;
        }

        if (from.MyItem.GetType() != MyItem.GetType() || from.MyCount + MyCount > MyItem.MyStackSize) {
            //cópia de todos os itens
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.items);

            //limpa slot A
            from.items.Clear();
            //Todos os itens do slot B, copia para o slot A
            from.AddItems(items);

            //limpa slot B
            items.Clear();
            //Move os itens cópia do slot A para o slot B
            AddItems(tmpFrom);

            return true;
        }

        return false;
    }

    private bool MergeItems(SlotScript from) {
        if (IsEmpty) {
            return false;
        }
        if (from.MyItem.GetType() == MyItem.GetType() && !IsFull) {
            int free = MyItem.MyStackSize - MyCount;

            for (int i = 0; i < free; i++) {
                AddItem(from.items.Pop());
            }
            return true;
        }

        return false;
    }

    public void Clear() {
        if (items.Count > 0) {
            items.Clear();
        }
    }

}
