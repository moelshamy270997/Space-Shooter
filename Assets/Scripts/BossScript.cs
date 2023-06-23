using System.Collections;
using TMPro;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    float moveSpeed = 5f, delayFactor = 5f;
    GameObject topLimit;
    GameObject bottomLimit;
    [SerializeField] GameObject explosionEffect;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject laserPrefab2;
    [SerializeField] GameObject floatingTxt;
    AudioScript audioScript;
    bool movingUp, movingForward = false, movingForwardAttack = false;
    public int hp;

    void Start()
    {
        audioScript = GameObject.Find("AudioObject").GetComponent<AudioScript>();

        topLimit = GameObject.Find("EnemyTopLimit").gameObject;
        bottomLimit = GameObject.Find("EnemyBottomLimit").gameObject;

        movingUp = true ? Mathf.RoundToInt(Random.value) == 1 : false;

        StartCoroutine(MoveCoroutine());
        StartCoroutine(PerformActionCoroutine());
        if (gameObject.tag == "SecondBoss")
            StartCoroutine(PerformMoveForwardCoroutine(GameObject.FindGameObjectsWithTag("SecondBoss")[0]));
    }

    private void CreateLaser(GameObject laser)
    {
        Instantiate(laser, transform.position + Vector3.left, Quaternion.Euler(0f, 0f, 90f));
    }

    private IEnumerator PerformActionCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.value * delayFactor);
            CreateLaser(laserPrefab);

            yield return new WaitForSeconds(Random.value / 2.0f);
            CreateLaser(laserPrefab2);
        }
    }

    private IEnumerator PerformMoveForwardCoroutine(GameObject obj)
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            delayFactor = 3.5f;
            movingForwardAttack = true;
            StartCoroutine(MoveForwardCoroutine(obj));

            yield return new WaitForSeconds(5f);
            delayFactor = 5f;
            movingForwardAttack = false;
            StopCoroutine(MoveForwardCoroutine(obj));
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

    private IEnumerator MoveForwardCoroutine(GameObject obj)
    {
        while (movingForwardAttack)
        {
            Vector3 direction = movingForward ? Vector3.down : Vector3.up;
            obj.transform.Translate(direction * 2f * Time.deltaTime);

            if (movingForward && obj.transform.position.x <= 1f)
                movingForward = false;
            else if (!movingForward && obj.transform.position.x >= 6.2f)
                movingForward = true;

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
