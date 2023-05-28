using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    int highScore;
    float creationDelay = 3f, cameraHeight, cameraWidth;
    [SerializeField] GameObject[] enemyPrefabs;

    void Start()
    {
        // Set Camera's bounds
        cameraHeight = Camera.main.orthographicSize - 1.5f;
        cameraWidth = cameraHeight * Camera.main.aspect - 2f;


        highScore = PlayerPrefs.GetInt("highScore");
        // highScore += 10;
        PlayerPrefs.SetInt("highScore", highScore);

        StartCoroutine(CreatePrefabCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator CreatePrefabCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(creationDelay);
            CreateEnemy();
        }
        
    }

    private void CreateEnemy()
    { 
        // Generate random position within the camera's bounds
        float randomX = Random.Range(-cameraWidth + 2f, cameraWidth);
        float randomY = Random.Range(-cameraHeight, cameraHeight);
        Vector3 randomPosition = new Vector3(randomX, randomY, 0f);

        // Instantiate the enemy prefab at the random position
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        Instantiate(enemyPrefabs[randomIndex], randomPosition, Quaternion.Euler(0f, 0f, -90f));

        // TODO: specify which enemyPrefab will be created, depending on the Level of the Player
    }
}
