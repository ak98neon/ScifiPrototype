using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FPSController : MonoBehaviour
{
    #region PUBLIC FIELDS

    [Header("Walk / Run Setting")] public float walkSpeed;
    public float runSpeed;

    [Header("Rotation Speed")] public float rotationSpeed = 1f;

    [Header("Jump Settings")] public float playerJumpForce;
    public ForceMode appliedForceMode;

    [Header("Jumping State")] public bool playerIsJumping;

    [Header("Current Player Speed")] public float currentSpeed;

    private Animator animator;

    #endregion


    #region PRIVATE FIELDS

    private float _xAxis;
    private float _zAxis;
    private CharacterController ch;
    private RaycastHit _hit;
    private Vector3 _groundLocation;
    private bool _isCapslockPressedDown;

    #endregion

    #region MONODEVELOP ROUTINES

    private void Start()
    {
        #region Initializing Components

        ch = GetComponent<CharacterController>();

        animator = GetComponent<Animator>();

        #endregion
    }

    private void Update()
    {
        CheckDoor();
        #region Controller Input

        _xAxis = Input.GetAxis("Horizontal");
        _zAxis = Input.GetAxis("Vertical");

        animator.SetBool("NonCombat", true);

        if (_xAxis != 0 || _zAxis != 0)
        {
            animator.SetBool("Idling", false);
            Vector3 moveDirection = new Vector3(_xAxis, 0, _zAxis);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= walkSpeed;

            //moveDirection.y -= gravity * Time.deltaTime;
            ch.Move(moveDirection * Time.deltaTime);
        } else
        {
            animator.SetBool("Idling", true);
        }

        #endregion

        #region Rotate Player

        this.transform.Rotate(new Vector3(0f, rotationSpeed * Input.GetAxis("Mouse X"), 0f));

        #endregion

        #region Adjust Player Speed

        currentSpeed = _isCapslockPressedDown ? runSpeed : walkSpeed;

        #endregion

        #region Player Jump Status

        playerIsJumping = Input.GetButton("Jump");

        #endregion

        #region Disable Multiple Jumps

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out _hit,
            Mathf.Infinity))
        {
            if (String.Compare(_hit.collider.tag, "ground", StringComparison.Ordinal) == 0)
            {
                _groundLocation = _hit.point;
            }

            var distanceFromPlayerToGround = Vector3.Distance(transform.position, _groundLocation);
            if (distanceFromPlayerToGround > 1f)
                playerIsJumping = false;
        }

        #endregion
    }

    private void CheckDoor()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out _hit, 2))
        {
            if (_hit.transform.tag == "Door")
            {
                Debug.Log("Open door");
                if (Input.GetKeyDown(KeyCode.F))
                {
                    _hit.collider.gameObject.GetComponent<OpenDoor>().OpenCloseDoor();
                }
            }
        }
    }

    #endregion
}
