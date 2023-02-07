using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Currently active
    [SerializeField]
    private GameObject selectionEffect;

    [SerializeField]
    private GameObject textField;

    public void OnPointerEnter(PointerEventData eventData)
    {
        selectionEffect.SetActive(false);
        textField.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectionEffect.SetActive(true);
        textField.SetActive(false);
    }
}
