using System.Collections;
using System.Collections.Generic;
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

        // Move the text upwards
        while (transform.position.y < startPosition.y + 1.0f)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            yield return null;
        }

        // transform.position = startPosition;
        // GetComponent<TextMeshProUGUI>().text = string.Empty;
        Destroy(gameObject);
    }

}
