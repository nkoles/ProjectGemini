using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;


public class TitleUIBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   [SerializeField] private float cardHoverPosition;
    private Vector3 cardsStartingPosition;
    public int moveAmount = 100;

    void Start()
    {
        cardsStartingPosition = transform.position;
        
    }

    public void OnPointerEnter(PointerEventData eventData) // move the cards up when hovering over them 
    {
        StopAllCoroutines();
        StartCoroutine(LerpMovement(true)); 
    }

    public void OnPointerExit(PointerEventData eventData) // move the cards down when you stop hovering over them 
    {
        StopAllCoroutines();
        StartCoroutine(LerpMovement(false));
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
