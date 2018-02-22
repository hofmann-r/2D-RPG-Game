using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerClickHandler {

    public Button MyButton { get; set; }

    public IUsable MyUsable { get; set; }

    public Image MyIcon {
        get {
            return icon;
        }

        set {
            icon = value;
        }
    }

    [SerializeField]
    private Image icon;

    public void OnPointerClick(PointerEventData eventData) {


    }

    public void OnClick() {
        if (MyUsable != null) {
            MyUsable.Use();
        }
    }

    // Use this for initialization
    void Start() {
        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update() {

    }
}
