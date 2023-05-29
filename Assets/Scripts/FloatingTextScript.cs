using System.Collections;
using UnityEngine;

public class FloatingTextScript : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(FloatingTextAnimation());
    }

    private IEnumerator FloatingTextAnimation()
    {
        Vector3 startPosition = transform.position;
        float moveSpeed = 2f;

        while (transform.position.y < startPosition.y + 1.0f)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

}
