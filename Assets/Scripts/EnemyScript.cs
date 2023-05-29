using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyScript : MonoBehaviour
{
    float moveSpeed = 5f;
    GameObject topLimit;
    GameObject bottomLimit;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject floatingTxt;
    [SerializeField] Canvas canvas;
    AudioScript audioScript;
    bool movingUp = true;
    public int hp;

    void Start()
    {
        audioScript = GameObject.Find("AudioObject").GetComponent<AudioScript>();

        topLimit = GameObject.Find("EnemyTopLimit").gameObject;
        bottomLimit = GameObject.Find("EnemyBottomLimit").gameObject;

        StartCoroutine(MoveCoroutine());
        StartCoroutine(PerformActionCoroutine());
    }

    private void CreateLaser(GameObject laser)
    {
        Instantiate(laser, transform.position + Vector3.left, Quaternion.Euler(0f, 0f, 90f));
    }

    private IEnumerator PerformActionCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            CreateLaser(laserPrefab);
        }
    }

    private IEnumerator MoveCoroutine()
    {
        while (true)
        {
            Vector3 direction = movingUp ? Vector3.left : Vector3.right;

            transform.Translate(direction * moveSpeed * Time.deltaTime);

            if (movingUp && transform.position.y >= topLimit.transform.position.y)
                movingUp = false;
            else if (!movingUp && transform.position.y <= bottomLimit.transform.position.y)
                movingUp = true;

            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerLaser") || collision.CompareTag("PlayerFirstSuperLaser"))
        {
            audioScript.EnemyHitSFX();

            GameObject obj = Instantiate(floatingTxt, transform.position, Quaternion.identity);
            obj.transform.SetParent(GameObject.Find("Canvas").transform, false);
            obj.GetComponent<TextMeshProUGUI>().text = "1";
            obj.transform.position = transform.position;

            hp -= 1;
            CheckHP();

            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("PlayerTripleLaser"))
        {
            audioScript.EnemyHitSFX();
            // TODO: floating number effect
            hp -= 2;
            CheckHP();

            Destroy(collision.gameObject);
        }
    }

    private void CheckHP()
    {
        if (hp <= 0)
        {
            // Enemy is dead
            Destroy(gameObject);
            audioScript.ExplosionSFX();

            // TODO: explosion effect
        }
    }

}
