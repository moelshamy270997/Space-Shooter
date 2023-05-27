using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    float speed = 10f;

    void Start()
    {
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
}
