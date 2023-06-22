using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaserScript : MonoBehaviour
{
    AudioScript audioScript;
    GameObject topLimit;
    GameObject bottomLimit;

    bool movingUp;
    float speed = 10f;

    void Start()
    {
        audioScript = GameObject.Find("AudioObject").GetComponent<AudioScript>();
        audioScript.LaserCreatedSFX();

        topLimit = GameObject.Find("EnemyTopLimit").gameObject;
        bottomLimit = GameObject.Find("EnemyBottomLimit").gameObject;

        StartCoroutine(MoveRightCoroutine(gameObject));
        StartCoroutine(MoveUpDownCoroutine(gameObject));

        movingUp = true ? Mathf.RoundToInt(Random.value) == 1 : false;
    }

    private IEnumerator MoveRightCoroutine(GameObject obj)
    {
        while (true)
        {
            obj.transform.Rotate(Vector3.forward * 100f * Time.deltaTime, Space.Self);
            obj.transform.Translate(Vector3.left * (speed / 3.0f) * Time.deltaTime, Space.World);
            yield return null;
        }
    }

    private IEnumerator MoveUpDownCoroutine(GameObject obj)
    {
        
        while (true)
        {
            Vector3 direction = movingUp ? Vector3.up : Vector3.down;
            obj.transform.Translate(direction * speed / 2f * Time.deltaTime, Space.World);

            if (movingUp && obj.transform.position.y >= topLimit.transform.position.y - 0.5)
                movingUp = false;
            else if (!movingUp && obj.transform.position.y <= bottomLimit.transform.position.y + 0.5)
                movingUp = true;

            yield return null;
        }
        
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerLaser"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
