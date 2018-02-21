using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSorter : MonoBehaviour {

    private SpriteRenderer parentRenderer;

    private List<Obstacle> obstatles = new List<Obstacle>();

    // Use this for initialization
    void Start() {
        parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Obstacle") {
            Obstacle o = collision.GetComponent<Obstacle>();

            o.FadeOut();

            if (obstatles.Count == 0 || o.MySpriteRenderer.sortingOrder - 1 < parentRenderer.sortingOrder) {
                parentRenderer.sortingOrder = o.MySpriteRenderer.sortingOrder - 1;
            }

            obstatles.Add(o);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {

        if (collision.tag == "Obstacle") {
            Obstacle o = collision.GetComponent<Obstacle>();

            o.FadeIn();

            obstatles.Remove(o);

            if (obstatles.Count == 0) {
                parentRenderer.sortingOrder = 32767;
            } else {
                obstatles.Sort();
                parentRenderer.sortingOrder = obstatles[0].MySpriteRenderer.sortingOrder - 1;
            }
        }

    }
}
