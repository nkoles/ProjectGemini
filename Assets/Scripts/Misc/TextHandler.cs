using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextHandler : MonoBehaviour
{
    public TMP_Text TextHolder;

    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
