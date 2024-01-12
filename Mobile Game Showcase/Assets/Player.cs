using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    #region Variables

    #region Health
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float healDelay;
    [SerializeField] private TextMeshProUGUI healthText, healthPacksText;
    [SerializeField] private AudioClip healSFX, healPickupSFX;

    //Internal Variables
    private int currentHealth;
    private int healthPacks;
    [SerializeField] private int startingHealthPacks = 3;
    private AudioSource audioSource;
    private float healTimer;
    #endregion

    #region Movement
    [Header("Movement")]
    public bool canMove = true;
    [Min(0), SerializeField] private float moveSpeed = 1;
    [Min(0), SerializeField] private float jumpForce = 1;
    #endregion

    #region Controls
    [Header("Controls")]
    [Tooltip("Allows the game to be played with keyboard if set to true.")] public bool allowKeyControls = true;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private Canvas mobileControls;
    #endregion

    #region Other
    private Animator animator;
    private Collider2D cldr;
    private Rigidbody2D rb;
    private bool allowGravity = false;
    #endregion

    #endregion


    private void Awake()
    {
        #region Other

        rb = GetComponent<Rigidbody2D>();
        cldr = GetComponent<Collider2D>();

        #endregion
    }

    // Start is called before the first frame update
    private void Start()
    {
        #region Movement and Controls
        allowGravity = rb.gravityScale > 0;
        mobileControls.enabled = !allowKeyControls;
        joystick.AxisOptions = allowGravity ? AxisOptions.Horizontal : AxisOptions.Both;
        #endregion

        #region Health
        currentHealth = maxHealth;
        healthText.text = $"Health: {currentHealth}";
        healthPacksText.text = $"= {healthPacks}";
        audioSource = GetComponent<AudioSource>();
        healthPacks = startingHealthPacks;
        animator = GetComponent<Animator>();
        #endregion
    }

    // Update is called once per frame
    private void Update()
    {
        #region Movement and Controls

        if (canMove)
        {
            //Sets the move variable depending on the controls
            Vector2 move = DetermineControls();
            MovePlayer(move);
        }

        #endregion

        #region Health
        if (currentHealth <= 0)
        {
            Die();
        }

        Debug.Log(currentHealth);

        //Code below fixes bugs
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        #endregion
    }

    #region Methods

    #region Movement and Controls

    private void MovePlayer(Vector2 move)
    {
        float moveMultiplier = moveSpeed * 10;
        Vector3 movePlayer = new(x: move.x * moveMultiplier, y: !allowGravity ? move.y * moveMultiplier : rb.velocity.y);

        rb.velocity = movePlayer;
    }

    private Vector2 DetermineControls()
    {
        Vector2 move = Vector2.zero;

        if (allowKeyControls)
        {
            move.x = Input.GetAxis("Horizontal");
            move.y = Input.GetAxis("Vertical");
        }
        else
        {
            move.x = joystick.Horizontal;
            move.y = joystick.Vertical;
        }

        return move;
    }

    public void Jump()
    {
        if (cldr.IsTouchingLayers(LayerMask.GetMask("Ground")) && allowGravity)
        {
            Vector2 playerJump = new(0, jumpForce * 100);
            rb.AddForce(playerJump);
        }
    }

    #endregion

    #region Health

    public void TakeDamage(int health)
    {
        if (currentHealth > 0)
        {
            currentHealth -= health;
        }
    }

    public void Heal(int health)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += health;
            healthPacks--;
            audioSource.PlayOneShot(healSFX);
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private void Die()
    {
        canMove = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Health Pack"))
        {
            CollectHealthPack(collision);
        }
    }

    private void CollectHealthPack(Collision2D collision)
    {
        healthPacks++;
        audioSource.PlayOneShot(healPickupSFX);
        Destroy(collision.gameObject);
    }

    #endregion

    #endregion
}
