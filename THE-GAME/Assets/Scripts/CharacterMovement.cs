using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Player Movement References")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private StaminaBar staminaBar;

    [Header("Player Movement")]
    [SerializeField] private Transform orientation;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float canRunDelayTime; // Stamina 0 olduktan sonra kaç sn sonra tekrar koşabileceğimiz 
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;

    [Header("Player Jumping")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float canJumpDelayTime; // Stamina 0 olduktan sonra kaç sn sonra tekrar koşabileceğimiz 

    private Vector3 velocity;//gravity için hız
    private float speed;// Koşma mı yürüyüş mü olduğunu belirlemek için

    [Header("Stamina Main Parameters")]
    [SerializeField] private float currentStamina;
    [SerializeField] private float maxStamina = 100f;

    [Header("Stamina Regen Parameters")]
    [Range(0, 50)][SerializeField] private float staminaDrain = 20;
    [Range(0, 50)][SerializeField] private float staminaRegen = 5;

    //Bools
    private bool isGrounded;
    private bool canRun;
    private bool canJump;
    private bool isJumping;
    private bool isMoving;
    private bool isWalking;
    private bool isRunning;

    private const float GRAVITY = -9.81f;

    private void Start()
    {
        canRun = true;
        canJump = true;
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
    }
    private void Update()
    {
        Movement();
        StaminaControl();
    }
    /// <summary>
    /// Hareket fonksiyonu. Yürüyüş ve koşma hızını ayarlar, zıplama ve yer çekimini kontrol eder.
    /// </summary>
    private void Movement()
    {
        if (GroundCheck() && velocity.y < 0)
        {
            velocity.y = -2f; // Zemine yapıştır
        }
        else
        {
            velocity.y += GRAVITY * Time.deltaTime;// Zemine hızlı yapıştır
        }

        // Hareket girdisi
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Yönü al
        Vector3 move = orientation.forward * z + orientation.right * x;

        isRunning = Input.GetKey(KeyCode.LeftShift) && (x != 0 || z != 0) && canRun;
        isWalking = !isRunning && (x != 0 || z != 0);
        isMoving = isRunning || isWalking;
        isJumping = Input.GetKeyDown(jumpKey) && isGrounded && canJump;
        // Koşma mı yürüyüş mü?
        speed = isRunning ? runSpeed : walkSpeed;

        controller.Move(move.normalized * speed * Time.deltaTime);
        Jump();

        controller.Move(velocity * Time.deltaTime);
    }
    /// <summary>
    /// Zemin kontrolü. Oyuncunun zeminle temasını kontrol eder.
    /// </summary>
    /// <returns></returns>
    private bool GroundCheck()
    {
        return isGrounded = Physics.CheckSphere(transform.position + Vector3.down * (controller.height / 2), groundDistance, groundMask);
    }
    /// <summary>
    /// Stamina kontrolü. Koşma ve zıplama sırasında stamina'yı azaltır, yürüyüş sırasında ise stamina'yı yeniler.
    /// </summary>
    private void StaminaControl()
    {
        if (!isRunning)
            RegenerateStamina();
        else if (isRunning)
            DrainStamina();

        if (currentStamina <= 0)
        {
            StartCoroutine(nameof(CanRun));
            currentStamina = 0;
        }
    }
    /// <summary>
    /// Stamina'nın sıfır olduktan sonra tekrar koşabilmesi için bekleme süresi ayarlama.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CanRun()
    {
        if (!canRun)
            yield break;
        canRun = false;
        yield return new WaitForSeconds(canRunDelayTime);
        canRun = true;
    }
    /// <summary>
    /// Stamina'yı azaltma fonksiyonu. Koşma sırasında stamina'yı azaltır.
    /// </summary>
    public void DrainStamina()
    {
        currentStamina -= staminaDrain * Time.deltaTime;
        staminaBar.SetStamina(currentStamina);
    }
    /// <summary>
    /// Stamina'yı yenileme fonksiyonu. Yürüyüş sırasında stamina'yı yeniler.
    /// </summary>
    private void RegenerateStamina()
    {
        if (currentStamina <= maxStamina - 0.01)
        {
            currentStamina += staminaRegen * Time.deltaTime;
            if (currentStamina >= maxStamina)
            {
                currentStamina = maxStamina;
            }
            staminaBar.SetStamina(currentStamina);
        }
    }
    /// <summary>
    /// Zıplama fonksiyonu. Oyuncunun zıplamasını kontrol eder.
    /// </summary>
    private void Jump()
    {
        if (Input.GetKeyDown(jumpKey) && isGrounded && canJump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * GRAVITY);
            StartCoroutine(nameof(CanJump));
        }
    }
    /// <summary>
    /// Tekrar zıplama kontrolü için bekleme süresi ayarlama.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CanJump()
    {
        if (!canJump)
            yield break;
        canJump = false;
        yield return new WaitForSeconds(canJumpDelayTime);
        canJump = true;
    }
}