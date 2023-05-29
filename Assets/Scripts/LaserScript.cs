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

    void FixedUpdate()
    {
        // First Super Attack Effect
        if (transform.position.y > 4f)
            transform.rotation = Quaternion.Euler(0f, 0f, -135f);
        if (transform.position.y < -4f)
            transform.rotation = Quaternion.Euler(0f, 0f, -45);
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        // If the super Attack is on, if the Super Laser hits the enemy's laser, it will be destroyed
        if (collision.CompareTag("EnemyLaser") && gameObject.CompareTag("PlayerFirstSuperLaser"))
        {
            Destroy(collision.gameObject);
        }
    }

}
