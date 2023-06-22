using UnityEngine;

public class AudioScript : MonoBehaviour
{
    [SerializeField] AudioClip selectSFX;
    [SerializeField] AudioClip laserCreatedSFX;
    [SerializeField] AudioClip enemyHitSFX;
    [SerializeField] AudioClip explosionSFX;
    [SerializeField] AudioClip godModeSFX;
    [SerializeField] AudioClip wonSFX;
    AudioSource audioSource;

    float musicFadeDuration = 1f;

    void Start()
    {
        audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
    }

    public void SelectSFX()
    {
        audioSource.PlayOneShot(selectSFX, 0.5f);
    }

    public void LaserCreatedSFX()
    {
        audioSource.PlayOneShot(laserCreatedSFX, 0.5f);
    }

    public void EnemyHitSFX()
    {
        audioSource.PlayOneShot(enemyHitSFX, 0.5f);
    }

    public void ExplosionSFX()
    {
        audioSource.PlayOneShot(explosionSFX, 0.5f);
    }

    public void WonSFX()
    {
        audioSource.PlayOneShot(wonSFX, 1f);
        Invoke("FadeMusicOutStart", 0.5f);
    }

    public void GodModeSFX()
    {
        audioSource.PlayOneShot(godModeSFX, 0.5f);
    }

    public void PlayerExplosionSFX()
    {
        audioSource.PlayOneShot(explosionSFX, 0.5f);
        StartCoroutine(FadeOutMusic());
    }

    private void FadeMusicOutStart()
    {
        StartCoroutine(FadeOutMusic());
    }

    private System.Collections.IEnumerator FadeOutMusic()
    {
        float elapsedTime = 0f;
        float startVolume = audioSource.volume;

        while (elapsedTime < musicFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / musicFadeDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

}
