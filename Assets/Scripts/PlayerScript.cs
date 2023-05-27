using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    [SerializeField] GameObject laserPrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateLaser();
        }
    }

    void CreateLaser()
    {
        Instantiate(laserPrefab, transform.position + Vector3.right, Quaternion.Euler(0f, 0f, -90f));
    }
}
