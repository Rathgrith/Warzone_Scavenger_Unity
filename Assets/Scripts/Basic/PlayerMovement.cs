using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // This is the movement speed
    public static float moveSpeed = 5f;
    // This is the jump speed
    public static float jumpSpeed = 5f;
    // This is the current able jump times
    public static int curJumpTimes = 1;
    // This is the max jump times
    public static int maxJumpTimes = 2;
    // This is the player's x length
    public static float playerXLength = 1.0f;
    // This is the player's z length
    public static float playerZLength = 1.0f;
    // This is the player's height
    public static float playerHeight = 1.0f;

    public bool ifOnGround = true;

    // Fire five ray to determine if the player is standing on the ground or in the air.
    bool IfGrounded()
    {
        float groundCheckDistance = 0.01f;
        float groundCheckRadius = 0.4f;  // The radius of the SphereCast
        // Ensure the SphereCast starts just slightly below the player to avoid clipping with the player's own collider.
        Vector3 sphereCastStartPos = this.transform.position + new Vector3(0, -0.5f + groundCheckRadius, 0);
        return Physics.SphereCast(sphereCastStartPos, groundCheckRadius, Vector3.down, out RaycastHit hit, groundCheckDistance);
    }


    // Use WASD to move the player, and use Space to jump, and use LeftShift to sprint.
    void MovePlayer()
    {
        ifOnGround = IfGrounded();

        // This is the movement part
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            direction += Camera.main.transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction -= Camera.main.transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction -= Camera.main.transform.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Camera.main.transform.right;
        }
        direction.y = 0;
        direction.Normalize();

        // Apply the movement
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Make player always face the direction of movement
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 5f * Time.deltaTime);
        }

        // This is the jump part
        if (Input.GetKeyDown(KeyCode.Space) && (ifOnGround || curJumpTimes > 0))
        {
            if (ifOnGround)
                curJumpTimes = maxJumpTimes - 1;
            else
                curJumpTimes--;
            this.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }
    }




    // This is the initialization part
    void Init()
    {
        curJumpTimes = maxJumpTimes;
        this.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
    }


    void Start()
    {
        Init();
    }


    void Update()
    {
        // Debug.DrawRay(this.transform.position + new Vector3(0, -0.5f * playerHeight, 0), Vector3.down, Color.green);
        MovePlayer();
    }
}
