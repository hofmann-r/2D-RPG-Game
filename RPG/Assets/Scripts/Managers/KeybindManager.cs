using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class KeybindManager : MonoBehaviour
{

	private static KeybindManager instance;

	public static KeybindManager MyInstance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<KeybindManager> ();
			}
			return instance;
		}
	}

	public Dictionary<string, KeyCode> Keybinds { get; private set; }

	public Dictionary<string, KeyCode> Actionbinds { get; private set; }

	private string bindName;

	// Use this for initialization
	void Start ()
	{
		Keybinds = new Dictionary<string, KeyCode> ();

		Actionbinds = new Dictionary<string, KeyCode> ();

		BindKey ("UP", KeyCode.W);
		BindKey ("DOWN", KeyCode.S);
		BindKey ("LEFT", KeyCode.A);
		BindKey ("RIGHT", KeyCode.D);

		BindKey ("ACT1", KeyCode.Alpha1);
		BindKey ("ACT2", KeyCode.Alpha2);
		BindKey ("ACT3", KeyCode.Alpha3);
		BindKey ("SWRD", KeyCode.Space);
		
	}
	
	public void BindKey(string key, KeyCode keybind){
		Dictionary<string, KeyCode> currentDictionary = Keybinds;
		if (key.Contains ("ACT")) { //if key contain action button
			currentDictionary = Actionbinds;
		} 
		if (!currentDictionary.ContainsKey (key)) {
			currentDictionary.Add (key, keybind);
			UIManager.MyInstance.UpdateKeyText (key, keybind); //redundante
		} else if (currentDictionary.ContainsValue (keybind)) {
			string myKey = currentDictionary.FirstOrDefault (x => x.Value == keybind).Key;
			currentDictionary [myKey] = KeyCode.None;
			UIManager.MyInstance.UpdateKeyText (key, KeyCode.None);
		}

		currentDictionary [key] = keybind;

		bindName = string.Empty;
		UIManager.MyInstance.UpdateKeyText (key, keybind);
	}

    public void KeyBindOnClick(string bindName) {
        this.bindName = bindName;
    }

    private void OnGUI() {
        if(bindName != string.Empty) {
            Event e = Event.current;

            if (e.isKey) {
                BindKey(bindName, e.keyCode);
            }
        }

        
    }
}
