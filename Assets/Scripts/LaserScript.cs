using System.Collections;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    AudioScript audioScript;

    float speed = 10f;

    void Start()
    {
        audioScript = GameObject.Find("AudioObject").GetComponent<AudioScript>();
        audioScript.LaserCreatedSFX();
        StartCoroutine(MoveRightCoroutine(gameObject));
    }

    private IEnumerator MoveRightCoroutine(GameObject obj)
    {
        while (true)
        {
            obj.transform.Translate(Vector3.up * speed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
