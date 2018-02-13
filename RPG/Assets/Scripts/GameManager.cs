using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {


    [SerializeField]
    private Player player;

    private NPC currentTarget;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        ClickTarget();
    }

    private void ClickTarget() {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
            //setar o alvo
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);

            if (hit.collider != null) {
                /*   //ataca apenas tags que contenham inimigos
                   //FUTURO: colocar tags de objetos para destruir e achar itens
                   if (hit.collider.tag == "Enemy") {
                       player.MyTarget = hit.transform.GetChild(0);
                   }
               } else {
                   //tirar o alvo
                   player.MyTarget = null;*/
                if (currentTarget != null) {
                    currentTarget.DeSelect();
                }

                currentTarget = hit.collider.GetComponent<NPC>(); //seleciona o alvo

                player.MyTarget = currentTarget.Select(); //diz ao player qual o alvo


                UIManager.MyInstance.ShowTargetFrame(currentTarget);


            } else {

                UIManager.MyInstance.HideTargetFrame();

                if (currentTarget != null) {
                    currentTarget.DeSelect();
                }

                currentTarget = null;
                player.MyTarget = null;
            }
        }
    }
}
