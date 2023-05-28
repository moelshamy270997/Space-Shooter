using UnityEngine;

public class AudioScript : MonoBehaviour
{
    [SerializeField] AudioClip selectSFX;
    [SerializeField] AudioClip laserCreatedSFX;
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

    public void LaserCreatedSFX()
    {
        audioSource.PlayOneShot(laserCreatedSFX, 0.5f);
    }
}
