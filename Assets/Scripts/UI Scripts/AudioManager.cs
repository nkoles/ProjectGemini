using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{
    // audio playing script - Add sounds to the Resources folder and then name them 
    // call the play function to play a clip by name
    public AudioSource SFXPlayer;
    private void Start()
    {
        SFXPlayer = GetComponent<AudioSource>();
    }
    public void play(string clipName)
    {
        var clip = Resources.Load<AudioClip>(clipName);
        SFXPlayer.clip = clip;
        SFXPlayer.Play();
    }
}