using System.Collections;
using TMPro;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    float moveSpeed = 5f;
    GameObject topLimit;
    GameObject bottomLimit;
    [SerializeField] GameObject explosionEffect;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject laserPrefab2;
    [SerializeField] GameObject floatingTxt;
    AudioScript audioScript;
    bool movingUp;
    public int hp;

    void Start()
    {
        audioScript = GameObject.Find("AudioObject").GetComponent<AudioScript>();

        topLimit = GameObject.Find("EnemyTopLimit").gameObject;
        bottomLimit = GameObject.Find("EnemyBottomLimit").gameObject;

        movingUp = true ? Mathf.RoundToInt(Random.value) == 1 : false;

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
            yield return new WaitForSeconds(Random.value * 5);
            CreateLaser(laserPrefab);

            yield return new WaitForSeconds(1f);
            CreateLaser(laserPrefab2);
        }
    }

    private IEnumerator MoveCoroutine()
    {
        while (true)
        {
            Vector3 direction = movingUp ? Vector3.left : Vector3.right;
            transform.Translate(direction * moveSpeed * Time.deltaTime);

            if (movingUp && transform.position.y >= topLimit.transform.position.y - 0.5)
                movingUp = false;
            else if (!movingUp && transform.position.y <= bottomLimit.transform.position.y + 0.5)
                movingUp = true;

            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerLaser") || collision.CompareTag("PlayerFirstSuperLaser") || collision.CompareTag("PlayerSecondSuperLaser"))
        {
            FindObjectOfType<GameScript>().SetLevelUp(true);
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
            FindObjectOfType<GameScript>().SetLevelUp(true);
            audioScript.EnemyHitSFX();

            GameObject obj = Instantiate(floatingTxt, transform.position, Quaternion.identity);
            obj.transform.SetParent(GameObject.Find("Canvas").transform, false);
            obj.GetComponent<TextMeshProUGUI>().text = "2";
            obj.transform.position = transform.position;

            hp -= 2;
            CheckHP();

            Destroy(collision.gameObject);
        }
    }

    private void CheckHP()
    {
        // Enemy is dead
        if (hp <= 0)
        {
            Destroy(gameObject);
            audioScript.ExplosionSFX();

            // Explosion Effect
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosion, 2f);
        }
    }
}
