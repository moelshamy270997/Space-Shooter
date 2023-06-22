using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class MenuScript : MonoBehaviour
{
    AudioScript audioScript;
    [SerializeField] TextMeshProUGUI highScoreTxt;
    [SerializeField] InputActionReference gameStart;

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
    private void OnEnable()
    {
        gameStart.action.performed += OnGameStart;
        gameStart.action.Enable();
    }

    private void OnDisable()
    {
        gameStart.action.Disable();
    }

    private void OnGameStart(InputAction.CallbackContext context)
    {
        audioScript.SelectSFX();
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
}
