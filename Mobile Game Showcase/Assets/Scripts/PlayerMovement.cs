using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private bool canMove = true;
    [Min(0) ,SerializeField] private float moveSpeed = 1;
    [Min(0) ,SerializeField] private float jumpForce = 1;

    [Header("Controls")]
    [Tooltip("Allows the game to be played with keyboard if set to true.")] public bool allowKeyControls = true;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private Canvas mobileControls;

    //Internal Variables
    private Collider2D cldr;
    private Rigidbody2D rb;
    private bool allowGravity = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cldr = GetComponent<Collider2D>();
    }

    private void Start()
    {
        allowGravity = rb.gravityScale > 0;
        mobileControls.enabled = !allowKeyControls;
        joystick.AxisOptions = allowGravity ? AxisOptions.Horizontal : AxisOptions.Both;
        //GameObject.Find("Jump").SetActive(allowGravity && !allowKeyControls);
    }

    private void Update()
    {
        if (canMove)
        {
            //Sets the move variable depending on the controls
            Vector2 move = DetermineControls();
            MovePlayer(move);
        }

        if (Input.GetButtonDown("Jump") && allowKeyControls)
        {
            Jump();
        }
    }

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
}
