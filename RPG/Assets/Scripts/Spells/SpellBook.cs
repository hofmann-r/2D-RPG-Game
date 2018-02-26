using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour
{
	[SerializeField]
	private Image castingBar;

	[SerializeField]
	private Text currentSpell;

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

	private static SpellBook instance;

	public static SpellBook MyInstance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<SpellBook> ();
			}

			return instance;
		}
	}

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{

	}

	public Spell CastSpell (string spellName, Player player)
	{

		Spell spell = Array.Find (spells, x => x.MyName == spellName);

		if (player.MyMana.MyCurrentValue >= spell.MyManaCost) {

			currentSpell.text = spell.MyName;

			icon.sprite = spell.MyIcon;

			castingBar.color = spell.MyBarColor;
			castingBar.fillAmount = 0;

			spellRoutine = StartCoroutine (Progress (spell));

			fadeRoutine = StartCoroutine (FadeBar ());
		}

		return spell;
	}

	private IEnumerator Progress (Spell spell)
	{
		float timePassed = Time.deltaTime;

		float rate = 1.0f / spell.MyCastTime;

		float progress = 0.0f;

		while (progress <= 1.0) {
			castingBar.fillAmount = Mathf.Lerp (0, 1, progress);

			progress += rate * Time.deltaTime;

			timePassed += Time.deltaTime;

			castTime.text = (spell.MyCastTime - timePassed).ToString ("F1");
			if (spell.MyCastTime - timePassed < 0) {
				castTime.text = "0.0";
			}

			yield return null;
		}
		StopCasting ();
	}

	private IEnumerator FadeBar ()
	{
		float rate = 1.0f / 0.45f;

		float progress = 0.0f;

		while (progress <= 1.0) {

			canvasGroup.alpha = Mathf.Lerp (0, 1, progress);

			progress += rate * Time.deltaTime;

			yield return null;
		}
	}

	public void StopCasting ()
	{

		if (fadeRoutine != null) {
			StopCoroutine (fadeRoutine);
			canvasGroup.alpha = 0;
			fadeRoutine = null;
		}
		if (spellRoutine != null) {
			StopCoroutine (spellRoutine);
			spellRoutine = null;
		}
	}

	public Spell GetSpell (string spellName)
	{
		return Array.Find (spells, x => x.MyName == spellName);
	}
}
