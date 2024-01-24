using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFade : MonoBehaviour
{
    private Image m_Image;

    public float fadeSpeed;

    private void Awake()
    {
        m_Image = GetComponent<Image>();
    }

    private void Start()
    {
        StartCoroutine(FadeIn(fadeSpeed));
    }

    //teehee
    private IEnumerator FadeIn(float speed)
    {
        Color startColor = new Color (0, 0, 0, 0);
        Color targetImageColor = Color.white;

        m_Image.color = startColor;

        while(m_Image.color != targetImageColor)
        {
            m_Image.color = Color.Lerp(m_Image.color, targetImageColor, speed * Time.deltaTime);

            yield return null;
        }

        print("Faded In");
    }
}
