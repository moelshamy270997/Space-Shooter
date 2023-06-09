using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class PlayerScript : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 moveDirection = Vector2.zero;

    AudioScript audioScript;

    [SerializeField] GameObject explosionEffect;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject tripleLaserPrefab;
    [SerializeField] GameObject firstSuperLaserPrefab;
    [SerializeField] GameObject secondSuperLaserPrefab;
    [SerializeField] GameObject secondSuperLaserPrefab2;

    [SerializeField] InputActionReference playerMovement;
    [SerializeField] InputActionReference playerFire;
    [SerializeField] InputActionReference playerTripleFire;
    [SerializeField] InputActionReference playerFirstSuperFire;
    [SerializeField] InputActionReference playerSecondSuperFire;
    [SerializeField] InputActionReference godMode;

    [SerializeField] TextMeshProUGUI wonTxt;
    [SerializeField] TextMeshProUGUI godModeTxt;

    int currentScore = 0;
    float cooldownDuration = 4f, firstSuperAttackCooldownDuration = 40f, secondSuperAttackCooldownDuration = 40f, shootDelay = 0.2f, speed = 5f;
    private bool isOnCooldown = false, isFirstSuperAttackOnCooldown = false, isSecondSuperAttackOnCooldown = false, godModeFlag = false;
    [SerializeField] Image cooldownImage;
    [SerializeField] Image firstSuperAttackCooldownImage;
    [SerializeField] Image secondSuperAttackCooldownImage;

    bool isPlayerFireHeld = false;
    Coroutine fireCoroutine;

    void Start()
    {
        audioScript = GameObject.Find("AudioObject").GetComponent<AudioScript>();

        rb = GetComponent<Rigidbody2D>();
        cooldownImage.fillAmount = 0f;
        firstSuperAttackCooldownImage.fillAmount = 0f;
        secondSuperAttackCooldownImage.fillAmount = 0f;
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
    }

    private IEnumerator SecondSuperAttack()
    {
        if (!isSecondSuperAttackOnCooldown)
        {
            StartCoroutine(SecondSuperCooldownCoroutine());
            for (int i = 0; i < 100; i++)
            {
                StartCoroutine(CreateLaserOfSecondSuperAttack());
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
    private IEnumerator FirstSuperAttack()
    {
        if (!isFirstSuperAttackOnCooldown)
        {
            StartCoroutine(FirstSuperCooldownCoroutine());
            for (int i = 0; i < 20; i++)
            {
                StartCoroutine(CreateLaserOfFirstSuperAttack());
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
    private IEnumerator CreateLaserOfFirstSuperAttack()
    {
        for (int j = 0; j < 10; j++)
        {
            Instantiate(firstSuperLaserPrefab, transform.position + Vector3.right, Quaternion.Euler(0f, 0f, -90f + j * 5f));
            yield return new WaitForSeconds(0.02f);
            Instantiate(firstSuperLaserPrefab, transform.position + Vector3.right, Quaternion.Euler(0f, 0f, -90f - j * 5f));
            yield return new WaitForSeconds(0.02f);
        }
    }

    private IEnumerator CreateLaserOfSecondSuperAttack()
    {
        float angleIncrement = 360f / 20; // Divide 360 degrees into 20 lasers
        float radius = 2f; // Adjust the radius of the circular pattern

        // Create sweeping lasers
        for (int j = 0; j < 20; j++)
        {
            float angle = j * angleIncrement;
            Vector3 offset = Quaternion.Euler(0f, 0f, angle) * (Vector3.right * radius);

            Instantiate(secondSuperLaserPrefab, transform.position + offset, Quaternion.Euler(0f, 0f, angle));
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(1.0f); // Adjust the pause before burst

        // Create burst of lasers
        int burstSize = 12; // Number of lasers in the burst
        float burstRadius = 1.5f; // Adjust the radius of the burst pattern

        for (int k = 0; k < burstSize; k++)
        {
            float angle = k * (360f / burstSize);
            Vector3 offset = Quaternion.Euler(0f, 0f, angle) * (Vector3.right * burstRadius);

            Instantiate(secondSuperLaserPrefab2, transform.position + offset, Quaternion.Euler(0f, 0f, angle));
            yield return new WaitForSeconds(0.1f); // Adjust the delay between each laser in the burst
        }
    }


    private void OnEnable()
    {
        playerFire.action.started += OnFireStarted;
        playerFire.action.canceled += OnFireCanceled;
        playerFire.action.Enable();

        playerTripleFire.action.performed += OnTripleFire;
        playerTripleFire.action.Enable();

        playerMovement.action.performed += OnMovement;
        playerMovement.action.canceled += OnMovement;
        playerMovement.action.Enable();

        playerFirstSuperFire.action.performed += OnFirstSuperAttackFire;
        playerFirstSuperFire.action.Enable();

        playerSecondSuperFire.action.performed += OnSecondSuperAttackFire;
        playerSecondSuperFire.action.Enable();

        godMode.action.performed += OnGodMode;
        godMode.action.Enable();

    }

    private void OnDisable()
    {
        playerFire.action.Disable();
        playerTripleFire.action.Disable();
        playerMovement.action.Disable();
        playerFirstSuperFire.action.Disable();
        playerSecondSuperFire.action.Disable();
        godMode.action.Disable();
    }

    private void OnFireStarted(InputAction.CallbackContext context)
    {
        isPlayerFireHeld = true;
        fireCoroutine = StartCoroutine(FireCoroutine());
    }

    private void OnFireCanceled(InputAction.CallbackContext context)
    {
        isPlayerFireHeld = false;
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
            fireCoroutine = null;
        }
    }

    private IEnumerator FireCoroutine()
    {
        while (isPlayerFireHeld)
        {
            CreateLaser(laserPrefab);
            yield return new WaitForSeconds(shootDelay);
        }
    }

    private void CreateLaser(GameObject laser)
    {
        Instantiate(laser, transform.position + Vector3.right, Quaternion.Euler(0f, 0f, -90f));
    }

    private void OnFirstSuperAttackFire(InputAction.CallbackContext context)
    {
        StartCoroutine(FirstSuperAttack());
    }

    private void OnSecondSuperAttackFire(InputAction.CallbackContext context)
    {
        StartCoroutine(SecondSuperAttack());
    }

    private void OnGodMode(InputAction.CallbackContext context)
    {
        GodMode();
    }

    private void OnTripleFire(InputAction.CallbackContext context)
    {
        if (!isOnCooldown)
        {
            CreateLaser(tripleLaserPrefab);
            StartCoroutine(CooldownCoroutine());
        }
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    private IEnumerator CooldownCoroutine()
    {
        isOnCooldown = true;
        float timer = cooldownDuration;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            cooldownImage.fillAmount = 1f - (timer / cooldownDuration);
            yield return null;
        }

        isOnCooldown = false;
        cooldownImage.fillAmount = 0f;
    }

    private IEnumerator FirstSuperCooldownCoroutine()
    {
        isFirstSuperAttackOnCooldown = true;
        float timer = firstSuperAttackCooldownDuration;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            firstSuperAttackCooldownImage.fillAmount = 1f - (timer / firstSuperAttackCooldownDuration);
            yield return null;
        }

        isFirstSuperAttackOnCooldown = false;
        firstSuperAttackCooldownImage.fillAmount = 0f;
    }

    private IEnumerator SecondSuperCooldownCoroutine()
    {
        isSecondSuperAttackOnCooldown = true;
        float timer = secondSuperAttackCooldownDuration;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            secondSuperAttackCooldownImage.fillAmount = 1f - (timer / firstSuperAttackCooldownDuration);
            yield return null;
        }

        isSecondSuperAttackOnCooldown = false;
        secondSuperAttackCooldownImage.fillAmount = 0f;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyLaser") && !godModeFlag)
        {
            Destroy(collision.gameObject);
            gameObject.SetActive(false);
            audioScript.PlayerExplosionSFX();

            // Set the highest score
            if (PlayerPrefs.GetInt("highScore") < currentScore)
                PlayerPrefs.SetInt("highScore", currentScore);

            // Explosion Effect
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Invoke("ReturnToMenu", 4f);
        }
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }

    private void GodMode()
    {
        godModeFlag = !godModeFlag;
        godModeTxt.gameObject.SetActive(godModeFlag);
        if (godModeFlag)
            audioScript.GodModeSFX();
    }

    public void GameWonFunction()
    {
        audioScript.WonSFX();
        wonTxt.gameObject.SetActive(true);
        Invoke("ReturnToMenu", 5f);
    }
}
