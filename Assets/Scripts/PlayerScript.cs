using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 moveDirection = Vector2.zero;

    AudioScript audioScript;

    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject tripleLaserPrefab;
    [SerializeField] GameObject firstSuperLaserPrefab;
    [SerializeField] InputActionReference playerMovement;
    [SerializeField] InputActionReference playerFire;
    [SerializeField] InputActionReference playerTripleFire;

    [SerializeField] TextMeshProUGUI godModeTxt;


    float cooldownDuration = 2f;
    private bool isOnCooldown = false, godMode = false;
    [SerializeField] Image cooldownImage;
        
    float speed = 5f;


    void Start()
    {
        audioScript = GameObject.Find("AudioObject").GetComponent<AudioScript>();

        rb = GetComponent<Rigidbody2D>();
        cooldownImage.fillAmount = 0f;
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GodMode();
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            StartCoroutine(FirstSuperAttack());
            
        }
    }

    private IEnumerator FirstSuperAttack()
    {
        for (int i = 0; i < 100; i++)
        {
            StartCoroutine(CreateLaserOfFirstSuperAttack());
            yield return new WaitForSeconds(0.01f);
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

    private void OnEnable()
    {
        playerFire.action.performed += OnFire;
        playerFire.action.Enable();

        playerTripleFire.action.performed += OnTripleFire;
        playerTripleFire.action.Enable();

        playerMovement.action.performed += OnMovement;
        playerMovement.action.canceled += OnMovement;
        playerMovement.action.Enable();
    }

    private void OnDisable()
    {
        playerFire.action.Disable();
        playerTripleFire.action.Disable();
        playerMovement.action.Disable();
    }

    private void CreateLaser(GameObject laser)
    {
        Instantiate(laser, transform.position + Vector3.right, Quaternion.Euler(0f, 0f, -90f));
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        CreateLaser(laserPrefab);
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
        cooldownImage.fillAmount = 1f;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyLaser") && !godMode)
        {
            Destroy(collision.gameObject);
            gameObject.SetActive(false);
            audioScript.ExplosionSFX();
            // TODO: explosion particle effect
            Invoke("ReturnToMenu", 3f);
        }
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }

    private void GodMode()
    {
        godMode = !godMode;
        godModeTxt.gameObject.SetActive(godMode);
        if (godMode)
            audioScript.GodModeSFX();
    }
}
