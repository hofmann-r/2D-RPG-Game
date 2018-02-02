using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{

	private Image content;

	public float currentFill;


	public float MyMaxValue { get; set; }

	public float MyCurrentValue {
		get {
			return currentValue;
		}
		set {

			if (value > MyMaxValue) {
				currentValue = MyMaxValue;
			} else if (value < 0) {
				currentValue = 0;
			} else {
				currentValue = value;
			}
		}
	}

	public float currentValue;


	// Use this for initialization
	void Start ()
	{
		MyMaxValue = 100;
		content = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
