using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{

    private Image content;

    public float currentFill;

    [SerializeField]
    private float lerpSpeed;

    [SerializeField]
    private Text statValuetext;


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

            currentFill = currentValue / MyMaxValue;

            statValuetext.text = currentValue + "/" + MyMaxValue;
        }
    }

    public float currentValue;


    // Use this for initialization
    void Start()
    {
      //  MyMaxValue = 100;
        content = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentFill != content.fillAmount) {
            content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
        //content.fillAmount = currentFill;
    }

    public void Initialize(float currentValue, float maxValue)
    {
        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
    }
}
