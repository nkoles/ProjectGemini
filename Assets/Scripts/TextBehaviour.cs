using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TextBehaviour : MonoBehaviour
{
    private TextMeshProUGUI _textComponent;
    
    public float typingSpeed;
    //
    //Set Text from other scripts by accessing the text string in tmp
    public string TextContent
    {
        get { return _textComponent.text; } 
        set {
            _textComponent.text = value;
        }
    }

    private void Awake()
    {
        _textComponent = GetComponent<TextMeshProUGUI>();
    }

    //Speed in Seconds
    private IEnumerator TypeText(float speed)
    {
        //Splitting TMP text string into chars
        char[] textChar = _textComponent.text.ToCharArray();
        _textComponent.text = "";

        for (int i =0; i < textChar.Length; ++i)
        {
            _textComponent.text += textChar[i];

            yield return new WaitForSeconds(1/speed);
        }

       
    }

    private void OnEnable()
    {
        StartCoroutine(TypeText(typingSpeed));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
