using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    enum levels
    {
        // First Level:     create one object of the First Enemy
        // Second Level:    create one object of the Second Enemy
        // Third Level:     create two objects of the First Enemy and two objects of the Third Enemy
        // Fouth Level:     create one object of the First Enemy, one object of the Second Enemy and three objects of the Third Enemy
        // Fifth Level:     create one object of Boss 1
        firstLevel, secondLevel, thirdLevel, fourthLevel, fifthLevel
    }

    Dictionary<levels, List<int>> levelsDict = new Dictionary<levels, List<int>>();
    levels currLevel;

    int currScore;
    float cameraHeight, cameraWidth;
    public TextMeshProUGUI scoreTxt;
    bool levelUp = false;

    // First Enemy index is 0 and Third Enemy is 3
    [SerializeField] GameObject[] enemyPrefabs;

    void Start()
    {
        GameSetUp();
        Invoke("CreateEnemy", 2f);
    }

    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && levelUp)
        {
            levelUp = false;
            LevelUpFunction();
            Invoke("CreateEnemy", 2f);
            UpdateScoreTxt();
        }

        if (!levelUp && GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            levelUp = true;
    }

    private Vector3 GenerateRandomPosition()
    {
        // Generate random position within the camera's bounds
        float randomX = Random.Range(-cameraWidth + 2f, cameraWidth);
        float randomY = Random.Range(-cameraHeight, cameraHeight);
        return new Vector3(randomX, randomY, 0f);
    }

    private void CreateEnemy()
    {
        Vector3 randomPosition;

        for (int i = 0; i < levelsDict[currLevel].Count; i++)
        {
            for (int j = 0; j < levelsDict[currLevel][i]; j++)
            {
                if (currLevel != levels.fifthLevel)
                    randomPosition = GenerateRandomPosition();
                else
                    randomPosition = new Vector3(cameraWidth + 2f, 0f, 0f);
                Instantiate(enemyPrefabs[i], randomPosition, Quaternion.Euler(0f, 0f, -90f));
            }
        }
    }

    private void LevelUpFunction()
    {
        currScore++;
        currLevel++;

        if (PlayerPrefs.GetInt("highScore") < currScore)
            PlayerPrefs.SetInt("highScore", currScore);
    }

    private void UpdateScoreTxt()
    {
        scoreTxt.text = "WAVE" + (currScore + 1).ToString("D3");
    }

    private void GameSetUp()
    {
        // Set Camera's bounds
        cameraHeight = Camera.main.orthographicSize - 1.5f;
        cameraWidth = cameraHeight * Camera.main.aspect - 2f;

        currScore = 0;
        currLevel = levels.firstLevel;

        levelsDict.Add(levels.firstLevel, new List<int>() { 1, 0, 0, 0, 0 });
        levelsDict.Add(levels.secondLevel, new List<int>() { 0, 1, 0, 0, 0 });
        levelsDict.Add(levels.thirdLevel, new List<int>() { 2, 0, 2, 0, 0 });
        levelsDict.Add(levels.fourthLevel, new List<int>() { 1, 1, 3, 0, 0 });
        levelsDict.Add(levels.fifthLevel, new List<int>() { 0, 0, 0, 0, 1 });
    }
}