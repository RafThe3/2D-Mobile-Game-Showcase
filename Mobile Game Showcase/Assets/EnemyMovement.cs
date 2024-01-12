using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Min(0) ,SerializeField] private float moveSpeed = 1;
    [SerializeField] private bool canMove = true;

    private GameObject player;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (canMove)
        {
            MoveEnemy();
        }
    }

    private void MoveEnemy()
    {
        float moveMultiplier = 10 * moveSpeed;
        Vector3 playerPosition = new(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
        playerPosition.Normalize();

        rb.velocity = playerPosition * moveMultiplier;
    }
}
