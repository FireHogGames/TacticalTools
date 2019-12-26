using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement properties")]
    //Movement properties
    public float hurtMoveSpeed;
    public float runSpeed;
    public float sprintSpeed;

    public bool canSprint;
    public bool isHurt;

    private float speed;
    private Vector3 movement;

    [Header("Jumping properties")]
    //Jumping properties
    public float jumpForce;

    public bool canJump;
    private float gravity;

    [Header("Rotation properties")]
    //rotation properties
    public Camera playerCamera;
    public float ySensitivity;
    public float xSensitivity;

    private float _xRot = 0f;

    

    //Character controller
    private CharacterController cc;

   

    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        //Check if the player is hurt
        //If is hurt. Make sure the player can't jump
        if (isHurt)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }

        //Check if the player is in the air or on the ground
        //If in the air. Then increase the gravity value to make the player fall faster
        if (cc.isGrounded)
        {
            gravity = Physics.gravity.y;
        }
        else
        {
            gravity += Physics.gravity.y * Time.deltaTime;
        }

        //Get the movement axis
        float _xMov = Input.GetAxis("Horizontal") * speed;
        float _zMov = Input.GetAxis("Vertical") * speed;

        //For sprinting
        if (Input.GetKey(KeyCode.LeftShift) && canSprint && Input.GetKey(KeyCode.W))
        {
            speed = sprintSpeed;
        }
        else if(!isHurt)
        {
            speed = runSpeed;
        }
        else if(isHurt)
        {
            speed = hurtMoveSpeed;
        }

        if(cc.isGrounded && canJump && Input.GetButtonDown("Jump"))
        {
            gravity = jumpForce;
        }

        //combine all movement vectors into a vector 3
        movement = new Vector3(_xMov, gravity, _zMov);

        float _yRot = Input.GetAxis("Mouse X") * ySensitivity;
        transform.Rotate(0f, _yRot, 0f);

        _xRot -= Input.GetAxis("Mouse Y") * xSensitivity;
        _xRot = Mathf.Clamp(_xRot, -80f, 80f);
        playerCamera.transform.localRotation = Quaternion.Euler(_xRot, 0f, 0f);

        //Make the movement vector to move into the direction your looking
        movement = transform.rotation * movement;

        //move the character controller
        cc.Move(movement * Time.deltaTime);
    }
}
