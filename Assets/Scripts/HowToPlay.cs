using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HowToPlay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string stringforThisCard;
    [SerializeField] private TextMeshProUGUI HTPTextBox;
    [SerializeField] private float cardHoverPosition;
    private Vector3 cardsStartingPosition;
    private string currentText;
    public int moveAmount;
    void Start()
    {
        currentText = HTPTextBox.text;
        cardsStartingPosition = transform.position;
    }

    public void OnPointerEnter(PointerEventData eventData) // move the cards up when hovering over them 
    {
        StartCoroutine(waitToChange());
        StartCoroutine(LerpMovement(true));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HTPTextBox.text = currentText;
        StartCoroutine(LerpMovement(false));
    }

    IEnumerator waitToChange()
    {
        //yield on a new YieldInstruction that waits
        yield return new WaitForSeconds(0.2f);
        HTPTextBox.text = stringforThisCard;
    }

    private IEnumerator LerpMovement(bool isUp)
    {
        int movement = (isUp) ? moveAmount : 0;
        Vector3 goalPosition = cardsStartingPosition + new Vector3(0, movement, 0);
        for (float i = 0; i < 1.1; i += 0.17f)
        {
            transform.position = Vector3.Slerp(transform.position, goalPosition, i);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
