using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    [SerializeField] AudioClip selectSFX;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
    }

    
    void Update()
    {
        
    }

    public void SelectSFX()
    {
        audioSource.PlayOneShot(selectSFX, 0.5f);
    }
}
