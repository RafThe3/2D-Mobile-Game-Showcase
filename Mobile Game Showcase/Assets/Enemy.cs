using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables

    #region Movement
    [Header("Movement")]
    [Min(0), SerializeField] private float moveSpeed = 1;
    [SerializeField] private bool canMove = true;
    #endregion

    #region Health
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private TMPro.TextMeshProUGUI healthText;

    //Internal Variables
    private int currentHealth = 0;
    private Vector3 textScale;
    #endregion

    #region Damage
    [Header("Damage")]
    [Min(0) ,SerializeField] private int damageToDeal = 1;
    [Min(0) ,SerializeField] private float damageDelay = 1;

    //Internal Variables
    private float damageTimer;
    #endregion

    #region Other
    private GameObject player;
    private Rigidbody2D rb;
    #endregion

    #endregion

    private void Start()
    {
        #region Health
        currentHealth = maxHealth;
        healthText.enabled = false;
        textScale = healthText.transform.localScale;
        #endregion

        #region Damage
        damageTimer = damageDelay;
        #endregion
    }

    private void Awake()
    {
        #region Other
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        #endregion
    }

    private void Update()
    {
        #region Movement
        if (canMove)
        {
            MoveEnemy();
        }
        #endregion

        #region Health
        DisplayEnemyHealth();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            TakeDamage(10);
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        //Code below fixes bugs

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        #endregion

        #region Damage
        damageTimer += Time.deltaTime;
        #endregion
    }

    #region Methods

    #region Movement
    private void MoveEnemy()
    {
        float moveMultiplier = 10 * moveSpeed;
        Vector3 playerPosition = new(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
        playerPosition.Normalize();
        Vector3 moveEnemy = playerPosition * moveMultiplier;
        bool closeToPlayer = playerPosition.magnitude < 0;

        rb.velocity = !closeToPlayer ? moveEnemy : Vector2.zero;
    }
    #endregion

    #region Health
    private void DisplayEnemyHealth()
    {
        Vector3 enemyFacing = gameObject.transform.localScale.x < 0 ? new(-textScale.x, textScale.y, textScale.z) : textScale;
        healthText.transform.localScale = enemyFacing;
        string enemyHealthText = $"Health: {currentHealth}";

        if (currentHealth < 100)
        {
            healthText.enabled = true;
        }

        healthText.text = enemyHealthText;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }
    private void DealDamage()
    {
        player.GetComponent<Player>().TakeDamage(damageToDeal);
        damageTimer = 0;
    }

    private void Die()
    {
        canMove = false;
        healthText.enabled = false;
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && damageTimer >= damageDelay)
        {
            DealDamage();
        }
    }

    #endregion

    #endregion
}
