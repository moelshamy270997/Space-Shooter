using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    enum levels
    {
        // First Level:         create one object of the First Enemy
        // Second Level:        create one object of the Second Enemy
        // Third Level:         create two objects of the First Enemy and two objects of the Third Enemy
        // Fouth Level:         create one object of the First Enemy, one object of the Second Enemy and three objects of the Third Enemy
        // Fifth Level:         create one object of Boss 1
        // Sixth Level:         create eight objects of the First Enemy and two objects of the Second Enemy
        // Seventh Level:       create three objects of the Second Enemy and three objects of the Fourth Enemy
        // Eighth Level:        create Five objects of the Third Enemy and Two objects of the Fourth Enemy
        // Ninth Level:         create Three objects of the Seoncd Enemy, Three objects of the Third Enemy and Three objects of the Fourth Enemy
        // Tenth & Final Level: create two objects of Boss 2
        firstLevel, secondLevel, thirdLevel, fourthLevel, fifthLevel, sixthLevel, seventhLevel, eighthLevel, ninthLevel, tenthLevel, gameWonLevel
    }

    Dictionary<levels, List<int>> levelsDict = new Dictionary<levels, List<int>>();
    levels currLevel;

    int currScore;
    float cameraHeight, cameraWidth;
    public TextMeshProUGUI scoreTxt;
    bool levelUp = false, gameWonFlag = false;

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
            if (currLevel != levels.gameWonLevel)
            {
                Invoke("CreateEnemy", 2f);
                UpdateScoreTxt();
            }
            else
            {
                gameWonFlag = true;
            }
        }

        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && currLevel == levels.gameWonLevel && gameWonFlag)
        {
            gameWonFlag = false;
            Invoke("GameWonFunction", 1f);
        }
    }

    private void GameWonFunction()
    {
        FindObjectOfType<PlayerScript>().GameWonFunction();
    }

    public void SetLevelUp(bool value)
    {
        levelUp = value;
    }

    private Vector3 GenerateRandomPosition()
    {
        // Generate random position within the camera's bounds
        float randomX = Random.Range(-cameraWidth + 3f, cameraWidth);
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
                if (currLevel == levels.fifthLevel) // Last Level after that the game will be reset
                    randomPosition = new Vector3(cameraWidth + 2f, Random.value * 2, 0f);
                else if (currLevel == levels.tenthLevel)
                    randomPosition = new Vector3(cameraWidth + 2f, Random.value * 4f, 0f);
                else
                    randomPosition = GenerateRandomPosition();

                Instantiate(enemyPrefabs[i], randomPosition, Quaternion.Euler(0f, 0f, -90f));
            }
        }
    }

    private void LevelUpFunction()
    {
        currScore++;
        currLevel++;

        if (PlayerPrefs.GetInt("highScore") < currScore && currScore != 11)
            PlayerPrefs.SetInt("highScore", currScore);
    }

    private void UpdateScoreTxt()
    {
        scoreTxt.text = "WAVE" + currScore.ToString("D3");
    }

    private void GameSetUp()
    {
        // Set Camera's bounds
        cameraHeight = Camera.main.orthographicSize - 1.5f;
        cameraWidth = cameraHeight * Camera.main.aspect - 2f;

        currScore = 1;
        currLevel = levels.firstLevel;

        // First Level:         create one object of the First Enemy
        // Second Level:        create one object of the Second Enemy
        // Third Level:         create two objects of the First Enemy and two objects of the Third Enemy
        // Fouth Level:         create one object of the First Enemy, one object of the Second Enemy and three objects of the Third Enemy
        // Fifth Level:         create one object of Boss 1
        // Sixth Level:         create eight objects of the First Enemy and two objects of the Second Enemy
        // Seventh Level:       create three objects of the Second Enemy and three objects of the Fourth Enemy
        // Eighth Level:        create Five objects of the Third Enemy and Two objects of the Fourth Enemy
        // Ninth Level:         create Three objects of the Seoncd Enemy, Three objects of the Third Enemy and Four objects of the Fourth Enemy
        // Tenth & Final Level: create two objects of Boss 2

        levelsDict.Add(levels.firstLevel, new List<int>() { 1, 0, 0, 0, 0, 0 });
        levelsDict.Add(levels.secondLevel, new List<int>() { 0, 1, 0, 0, 0, 0 });
        levelsDict.Add(levels.thirdLevel, new List<int>() { 2, 0, 2, 0, 0, 0 });
        levelsDict.Add(levels.fourthLevel, new List<int>() { 1, 1, 3, 0, 0, 0 });
        levelsDict.Add(levels.fifthLevel, new List<int>() { 0, 0, 0, 0, 1, 0 });

        levelsDict.Add(levels.sixthLevel, new List<int>() { 8, 2, 0, 0, 0, 0});
        levelsDict.Add(levels.seventhLevel, new List<int>() { 0, 3, 0, 3, 0, 0 });
        levelsDict.Add(levels.eighthLevel, new List<int>() { 0, 0, 5, 2, 0, 0 });
        levelsDict.Add(levels.ninthLevel, new List<int>() { 0, 3, 3, 4, 0, 0 });
        levelsDict.Add(levels.tenthLevel, new List<int>() { 0, 0, 0, 0, 0, 2 });
    }
}