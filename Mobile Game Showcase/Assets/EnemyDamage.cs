using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float damageDelay = 1;
    [SerializeField] private AudioClip damageAudio;

    //Internal Variables
    private PlayerHealth playerHealth; 
    private AudioSource audioSource;
    private float timer;
    private Animator animator;

    //make serilized field fdawdor dmg

    // Start is called before the first frame update
    private void Start()
    {
        playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        timer = damageDelay;
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && timer >= damageDelay)
        {
            DamagePlayer();
            animator.SetTrigger("Attack");
        }
    }

    private void DamagePlayer()
    {
        playerHealth.TakeDamage(damage);
        audioSource.PlayOneShot(damageAudio, 1.5f);
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }
}
