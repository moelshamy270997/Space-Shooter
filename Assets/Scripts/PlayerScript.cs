using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 moveDirection = Vector2.zero;

    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject tripleLaserPrefab;
    [SerializeField] InputActionReference playerMovement;
    [SerializeField] InputActionReference playerFire;
    [SerializeField] InputActionReference playerTripleFire;


    float cooldownDuration = 2f;
    private bool isOnCooldown = false;
    [SerializeField] Image cooldownImage;
        
    float speed = 5f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cooldownImage.fillAmount = 0f;
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
    }

    void Update()
    {

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

}
