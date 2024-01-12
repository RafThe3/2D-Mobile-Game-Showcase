using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementExample : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f, chaseDistance = 1;
    public bool canMove = true;

    //Internal Variables
    private Rigidbody2D rb;
    private GameObject player;
    private bool isMoving;
    //private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        //animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            MoveEnemy();
            FlipEnemyFacing();
        }
        else
        {
            rb.velocity = new(0, rb.velocity.y);
            return;
        }
    }

    private void Update()
    {
        isMoving = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        //animator.SetBool("isMoving", isMoving);
    }

    private void MoveEnemy()
    {
        float moveMultplier = 10 * moveSpeed;
        Vector3 playerPosition = player.transform.position - transform.position;
        playerPosition.Normalize();
        Vector2 moveEnemy = new(x: moveMultplier * playerPosition.x, y: moveMultplier * playerPosition.y);

        rb.velocity = moveEnemy;
    }

    private void FlipEnemyFacing()
    {
        bool horizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        if (horizontalSpeed)
        {
            transform.localScale = new Vector3(Mathf.Sign(rb.velocity.x), 1, 1);
        }
    }
}
