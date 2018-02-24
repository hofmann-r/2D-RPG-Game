using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellButton : MonoBehaviour, IPointerClickHandler {

    [SerializeField]
    private string spellName;

    public void OnPointerClick(PointerEventData eventData) {
        if(eventData.button == PointerEventData.InputButton.Left) { //verifica se o botão esquerdo do mouse foi clicado
            //pegar spell selecionado
            HandScript.MyInstance.TakeMoveable(SpellBook.MyInstance.GetSpell(spellName));

        }

    }
}
