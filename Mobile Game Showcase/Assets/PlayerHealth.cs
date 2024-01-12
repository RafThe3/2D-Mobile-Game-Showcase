using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;
    [SerializeField] private float healDelay;
    [SerializeField] private TextMeshProUGUI healthText, healthPacksText;
    [SerializeField] private AudioClip healSFX, healPickupSFX;

    //Internal Variables
    [HideInInspector] public int healthPacks;
    [SerializeField] private int startingHealthPacks = 3;
    private AudioSource audioSource;
    private float healTimer;
    private Animator animator;

    private void Start()
    {
        health = maxHealth;
        healthText.text = $"Health: {health}";
        healthPacksText.text = $"= {healthPacks}";
        audioSource = GetComponent<AudioSource>();
        healthPacks = startingHealthPacks;
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        healTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E) && healTimer >= healDelay && healthPacks > 0)
        {
            Heal(10);
            healTimer = 0;
        }

        if (health <= 0)
        {
            Die();
            Invoke(nameof(ReloadScene), 2);
        }

        healthText.text = $"Health: {health}";
        healthPacksText.text = $"= {healthPacks}";
    }

    public void TakeDamage(int health)
    {
        if (this.health > 0)
        {
            this.health -= health;
        }
    }

    public void Heal(int health)
    {
        if (this.health < maxHealth)
        {
            this.health += health;
            healthPacks--;
            audioSource.PlayOneShot(healSFX);
        }

        if (this.health > maxHealth)
        {
            this.health = maxHealth;
        }
    }

    public void SetHealth(int health)
    {
        this.health = health;
    }

    // Update is called once per frame

    private void Die()
    {
        SetHealth(0);
        animator.SetTrigger("Die");
        GetComponent<PlayerMovement>().canMove = false;
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Health Potion"))
        {
            healthPacks++;
            audioSource.PlayOneShot(healPickupSFX);
            Destroy(collision.gameObject);
        }
    }
}
