using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    float moveSpeed = 5f;
    [SerializeField] GameObject topLimit;
    [SerializeField] GameObject bottomLimit;
    [SerializeField] GameObject laserPrefab;
    bool movingUp = true;
    public int hp;

    void Start()
    {
        StartCoroutine(MoveCoroutine());
        StartCoroutine(PerformActionCoroutine());
    }

    void Update()
    {

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

}
