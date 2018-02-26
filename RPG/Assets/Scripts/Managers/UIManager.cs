using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System; 

public class UIManager : MonoBehaviour {

    private bool keybindOpen = false, spellBookOpen = false, inventoryOpen = false;

    private static UIManager instance;

    public static UIManager MyInstance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }

    [SerializeField]
    private ActionButton[] actionButtons;

    [SerializeField]
    private GameObject targetFrame;

    private Stat healthStat;

    [SerializeField]
    private Image portraitFrame;

	[SerializeField]
	private CanvasGroup keybindMenu;

    [SerializeField]
    private CanvasGroup spellBookMenu;

    private GameObject[] keybindButtons;

	private void Awake() {
		keybindButtons = GameObject.FindGameObjectsWithTag ("keybind");
	}

    // Use this for initialization
    void Start() {
        healthStat = targetFrame.GetComponentInChildren<Stat>();
        //debug
        //SetUsable(actionButtons[0], SpellBook.MyInstance.GetSpell("Flame Strike"));
        //SetUsable(actionButtons[1], SpellBook.MyInstance.GetSpell("Frost Bite"));
        //SetUsable(actionButtons[2], SpellBook.MyInstance.GetSpell("Flash"));
    }

    // Update is called once per frame
    void Update() {
		if (Input.GetKeyDown(KeyCode.Escape) && !spellBookOpen) {
            keybindOpen = !keybindOpen;
			OpenClose(keybindMenu);
		}
        if (Input.GetKeyDown(KeyCode.L) && !keybindOpen) {
            spellBookOpen = !spellBookOpen;
            OpenClose(spellBookMenu);
        }

        if (Input.GetKeyDown(KeyCode.B) && !keybindOpen && !spellBookOpen) {
            inventoryOpen = !inventoryOpen;
            InventoryScript.MyInstance.OpenClose();
        }

    }

    public void ShowTargetFrame(NPC target) {
        targetFrame.SetActive(true);

        healthStat.Initialize(target.MyHealth.MyCurrentValue, target.MyHealth.MyMaxValue);

        portraitFrame.sprite = target.MyPortrait;

        target.healthChanged += new HealthChanged(UpdateTargetFrame);

        target.characterRemoved += new CharacterRemoved(HideTargetFrame);
    }

    public void HideTargetFrame() {
        targetFrame.SetActive(false);
    }

    public void UpdateTargetFrame(float health) {
        healthStat.MyCurrentValue = health;
    }

	public void UpdateKeyText(string key, KeyCode code){
		Text tmp = Array.Find (keybindButtons, x => x.name == key).GetComponentInChildren<Text>();
		tmp.text = code.ToString ();
	}

    public void ClickActionButton(string buttonName) {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();

    }

    public void OpenClose(CanvasGroup canvasGroup) {
        canvasGroup.alpha = (canvasGroup.alpha > 0) ? 0 : 1;
        canvasGroup.blocksRaycasts = (canvasGroup.blocksRaycasts == true) ? false : true;
        Time.timeScale = (Time.timeScale > 0) ? 0 : 1; //pausar o jogo
    }

    public void UpdateStackSize(IClickable clickable) {

        if (clickable.MyCount > 1) {
            clickable.MyStackSize.text = clickable.MyCount.ToString();
            clickable.MyStackSize.color = Color.white;
            clickable.MyIcon.color = Color.white;
        } else {
            clickable.MyStackSize.color = new Color(0, 0, 0, 0); //se for só um, mostra apenas o ícone do item
            clickable.MyIcon.color = Color.white;
        }

        if (clickable.MyCount == 0) {
            clickable.MyIcon.color = new Color(0, 0, 0, 0);
            clickable.MyStackSize.color = new Color(0, 0, 0, 0);
        }
    }
}
