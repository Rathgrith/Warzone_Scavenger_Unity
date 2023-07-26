using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RadioManager : MonoBehaviour
{
    public static RadioManager Instance { get; private set; }

    public AudioSource[] audioSources; // Set these in the Inspector
                                       // AudioSource for sound effects
    public AudioSource sfxSource;
    // AudioClip for the switch sound
    public AudioClip switchClip;
    public TextMeshProUGUI nowPlayingText;

    public string[] trackName;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        for (int i = 0; i < audioSources.Length; ++i)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) // Check for number key press
            {
                SwitchTrack(i);
            }
        }
    }

    public void SwitchTrack(int trackIndex)
    {
        if (trackIndex < 0 || trackIndex >= audioSources.Length)
        {
            Debug.LogWarning("Invalid track index.");
            return;
        }

         // Play the switch sound effect
        sfxSource.PlayOneShot(switchClip);
        for (int i = 0; i < audioSources.Length; ++i)
        {
            if (i == trackIndex)
            {
                if (!audioSources[i].isPlaying)
                {
                    audioSources[i].Play();
                    nowPlayingText.text = "Now playing: Track " + (trackName[trackIndex]);
                }
            }
            else
            {
                if (audioSources[i].isPlaying)
                {
                    audioSources[i].Pause();
                }
            }
        }
    }

}

