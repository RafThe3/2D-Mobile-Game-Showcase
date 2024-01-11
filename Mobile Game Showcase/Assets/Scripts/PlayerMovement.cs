using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float jumpForce = 1;

    [Header("Controls")]
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private Canvas mobileControls;
    [Tooltip("Allows the game to be played with keyboard.")] public bool allowKeyControls = true;
    //[Tooltip("Allow gravity if the game is a platformer.") ,SerializeField] private bool allowGravity = false;

    //Internal Variables
    private Collider2D collider;
    private Rigidbody2D rigidbody2D;
    private bool allowGravity = false;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        allowGravity = rigidbody2D.gravityScale > 0;
        mobileControls.enabled = !allowKeyControls;
        joystick.AxisOptions = allowGravity ? AxisOptions.Horizontal : AxisOptions.Both;
        GameObject.Find("Jump").SetActive(allowGravity && !allowKeyControls);
    }

    private void Update()
    {
        //Sets the move variable depending on the controls
        Vector2 move = DetermineControls();
        MovePlayer(move);

        if (Input.GetButtonDown("Jump") && allowKeyControls)
        {
            Jump();
        }
    }

    private void MovePlayer(Vector2 move)
    {
        float moveMultiplier = moveSpeed * 10;
        Vector3 movePlayer = new(x: move.x * moveMultiplier, y: !allowGravity ? move.y * moveMultiplier : rigidbody2D.velocity.y);

        rigidbody2D.velocity = movePlayer;
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
        if (collider.IsTouchingLayers(LayerMask.GetMask("Ground")) && allowGravity)
        {
            Vector2 playerJump = new(0, jumpForce * 100);
            rigidbody2D.AddForce(playerJump);
        }
    }
}
