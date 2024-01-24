using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Credits : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image cardFace;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI roleText;
    void Start()
    {
        cardFace = transform.GetChild(0).GetComponent<Image>();
        nameText = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        roleText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        Debug.Log (nameText.text);
        Debug.Log(cardFace);
        cardFace.enabled = false;
        nameText.enabled = false;
        roleText.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       cardFace.enabled = true;
       nameText.enabled = true;
       roleText.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cardFace.enabled = false;
        nameText.enabled = false;
        roleText.enabled = false;
    }


}
