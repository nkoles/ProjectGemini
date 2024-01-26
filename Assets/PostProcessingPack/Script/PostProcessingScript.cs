using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class PostProcessingScript : MonoBehaviour
{
    [Header("Vignette")]
    private float baseVigValue;

    [Range(0f, 1f)]public float vignetteRange = 0.2f;
    public float deltaRange = 0.02f;

    [Header("Film Grain")] 
    public float filmSpeed = 0.1f;

    private bool filmDone;
    private void Start()
    {
        var volume = gameObject.GetComponent<Volume>();
        if (volume.profile.TryGet<Vignette>(out var vig)) 
        {
            baseVigValue = vig.intensity.value;
        }
    }

    private void Update()
    {
        UpdateVig();
        UpdateFilm();
    }

    private void UpdateVig()
    {
        var volume = gameObject.GetComponent<Volume>();
        if (volume.profile.TryGet<Vignette>(out var vig)) 
        {
            vig.intensity.overrideState = true;
            vig.intensity.value = Mathf.Clamp(vig.intensity.value + Random.Range(-deltaRange, deltaRange), baseVigValue, baseVigValue + vignetteRange);
        }
    }

    private void UpdateFilm()
    {
        var volume = gameObject.GetComponent<Volume>();
        if (volume.profile.TryGet<FilmGrain>(out var film)) 
        {
            film.response.overrideState = true;
            if (film.response.value >= 1f)
            {
                filmDone = true;
            }
            if (film.response.value <= 0f)
            {
                filmDone = false;
            }

            if (filmDone)
            {
                film.response.value -= filmSpeed;
            }
            else
            {
                film.response.value += filmSpeed;
            }
        }
    }
}

      
