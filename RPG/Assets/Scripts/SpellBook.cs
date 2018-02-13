using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour
{
    [SerializeField]
    private Image castingBar;

    [SerializeField]
    private Text spellName;

    [SerializeField]
    private Text castTime;

    [SerializeField]
    private Spell[] spells;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private Coroutine spellRoutine;

    private Coroutine fadeRoutine;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Spell CastSpell(int index)
    {
        spellName.text = spells[index].MyName;

        icon.sprite = spells[index].MyIcon;

        castingBar.color = spells[index].MyBarColor;
        castingBar.fillAmount = 0;

        spellRoutine = StartCoroutine(Progress(index));

        fadeRoutine = StartCoroutine(FadeBar());

        return spells[index];
    }

    private IEnumerator Progress(int index)
    {
        float timePassed = Time.deltaTime;

        float rate = 1.0f / spells[index].MyCastTime;

        float progress = 0.0f;

        while (progress <= 1.0) {
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            timePassed += Time.deltaTime;

            castTime.text = (spells[index].MyCastTime - timePassed).ToString("F1");
            if(spells[index].MyCastTime - timePassed < 0) {
                castTime.text = "0.0";
            }

            yield return null;
        }
        StopCasting();
    }

    private IEnumerator FadeBar()
    {
        float rate = 1.0f / 0.45f;

        float progress = 0.0f;

        while (progress <= 1.0) {

            canvasGroup.alpha = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            yield return null;
        }
    }

    public void StopCasting()
    {

        if(fadeRoutine != null) {
            StopCoroutine(fadeRoutine);
            canvasGroup.alpha = 0;
            fadeRoutine = null;
        }
        if(spellRoutine != null) {
            StopCoroutine(spellRoutine);
            spellRoutine = null;
        }
    }
}
