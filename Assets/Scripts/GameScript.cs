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
        firstLevel,secondLevel, thirdLevel, fourthLevel, fifthLevel
    }
    levels level;
    int showLevel = 0;
    int highScore;
    int enemyKills = 0;
    float creationDelay = 3f, cameraHeight, cameraWidth;
    public TextMeshProUGUI textMeshProComponent;
    [SerializeField] GameObject[] enemyPrefabs;

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
        

        // TODO: specify which enemyPrefab will be created, depending on the Level of the Player
        if(level == levels.fifthLevel)
        {
            // Create Boss Enenmy
            return;
        }
        GameObject enemyPrefabToInstantiate = GetEnemyPrefabForLevel();
        Instantiate(enemyPrefabToInstantiate, randomPosition, Quaternion.Euler(0f, 0f, -90f));

    }

    private GameObject GetEnemyPrefabForLevel()
    {
        int levelIndex = (int)level;
        //Debug.Log(levelIndex);
        return enemyPrefabs[levelIndex];
    }
    public void IncreaseLevel()
    {

        enemyKills++;
        if ((enemyKills % 5) == 0)
        {
            // Increase level
            showLevel++;
            level++;
            if (level > levels.fifthLevel)
            {
                level = levels.firstLevel;
            }

            textMeshProComponent.text = "WAVE " + (showLevel + 1).ToString();
        }

    }
}
