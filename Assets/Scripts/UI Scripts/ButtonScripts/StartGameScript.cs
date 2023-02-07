using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartGameScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Currently inactive
    [SerializeField]
    private GameObject selectionEffect;

    public void OnPointerEnter(PointerEventData eventData)
    {
        selectionEffect.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectionEffect.SetActive(false);
    }
}
