using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuScript : MonoBehaviour
{
    AudioScript audioScript;
    [SerializeField] TextMeshProUGUI infoTxt;
    [SerializeField] TextMeshProUGUI highScoreTxt;

    float zoomSpeed = 0.2f, maxScale = 1.1f, minScale = 0.9f, currentScale = 0.9f;
    bool zoomIn = true;
    int highScore;

    void Start()
    {
        audioScript = GameObject.Find("AudioObject").GetComponent<AudioScript>();

        if (PlayerPrefs.GetInt("highScore") != 0)
            highScore = PlayerPrefs.GetInt("highScore");
        else
        {
            highScore = 0;
            PlayerPrefs.SetInt("highScore", highScore);
        }

        highScoreTxt.text = "High Score: " + highScore.ToString();
    }

    void Update()
    {
        InfoTxtMove();
        
        if (Input.anyKeyDown)
        {
            audioScript.SelectSFX();
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
        
    }

    void InfoTxtMove()
    {
        if (zoomIn)
            currentScale += zoomSpeed * Time.deltaTime;
        else
            currentScale -= zoomSpeed * Time.deltaTime;

        if (currentScale > maxScale)
            zoomIn = false;
        else if (currentScale < minScale)
            zoomIn = true;

        infoTxt.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
    }
}
