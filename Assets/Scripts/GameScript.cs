using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class GameScript : MonoBehaviour
{
    enum levels
    {
        firstLevel, secondLevel, thirdLevel, fourthLevel, fifthLevel
    }

    levels level = levels.firstLevel;
    int showLevel = 0;
    int highScore;
    int enemyKills = 0;
    int enemiesToCreate = 7;
    int enemiesCreated = 0;
    float creationDelay = 3f, cameraHeight, cameraWidth;
    public TextMeshProUGUI textMeshProComponent;
    [SerializeField] GameObject[] enemyPrefabs;
    bool bossCreated = false;
    private System.Random random = new System.Random();

    void Start()
    {
        // Set Camera's bounds
        cameraHeight = Camera.main.orthographicSize - 1.5f;
        cameraWidth = cameraHeight * Camera.main.aspect - 2f;

        highScore = PlayerPrefs.GetInt("highScore");
        // PlayerPrefs.SetInt("highScore", 0);

        StartCoroutine(CreatePrefabCoroutine());
    }

    private IEnumerator CreatePrefabCoroutine()
    {
        while (true)
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

        // TODO: specify which enemyPrefab will be created, depending on the Level of the Player
        if (level == levels.fifthLevel && !bossCreated)
        {
            int boss = ChooseRandomNumber(4, 5);
            Instantiate(enemyPrefabs[boss], randomPosition, Quaternion.Euler(0f, 0f, -90f));
            bossCreated = true;
            return;
        }

        if (level != levels.fifthLevel && enemiesCreated < enemiesToCreate)
        {
            GameObject enemyPrefabToInstantiate = GetEnemyPrefabForLevel();
            Instantiate(enemyPrefabToInstantiate, randomPosition, Quaternion.Euler(0f, 0f, -90f));
            enemiesCreated++;
            bossCreated = false;
        }
    }

    private int ChooseRandomNumber(int number1, int number2)
    {
        // Generate a random number between 0 and 1
        double randomValue = random.NextDouble();

        if (randomValue < 0.5)
        {
            return number1;
        }
        else
        {
            return number2;
        }
    }

    private GameObject GetEnemyPrefabForLevel()
    {
        int levelIndex = (int)level;
        return enemyPrefabs[levelIndex];
    }

    public void IncreaseLevel()
    {
        if (level == levels.fifthLevel)
        {
            showLevel++;
            level = levels.firstLevel;
            textMeshProComponent.text = "WAVE " + (showLevel + 1).ToString();
            return;
        }
        enemyKills++;
        if (enemyKills == 7)
        {
            // Increase level
            enemiesCreated = 0;
            enemyKills = 0;
            showLevel++;
            level++;
            if (showLevel > highScore) { highScore = showLevel; }

            textMeshProComponent.text = "WAVE " + (showLevel + 1).ToString();
        }
    }
}