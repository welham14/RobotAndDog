using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour {


    //Controller Vars
    public GameObject target;
    Animator targetAnimator;
    Rigidbody body;
    public float walkSpeed = 100;
    public float gravity = 20;
    public float jumpForce = 8;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private bool facingRight = true;
    
    //Camera Vars
    private Vector3 targetCameraPos;
    public float followSpeed = 1f;
    public float closeZoom, mediumZoom, farZoom;
    public float cameraHeight = 0;
    private float targetZoom;
    

    void Awake() {
        targetZoom = mediumZoom;
        SetTargetCharacter();
        targetCameraPos = new Vector3(target.transform.position.x, transform.position.y, targetZoom);
        body = target.GetComponent<Rigidbody>();
        controller = target.GetComponent<CharacterController>();
    }

    void FixedUpdate() {
        if (controller.isGrounded) {
            //print("Grounded");
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection = moveDirection * walkSpeed;
            if (Input.GetButton("Jump")) {
                moveDirection.y = jumpForce;
            }
        } else {
            //print("In Air");
        }

        //Apply Gravity
        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

        //Move the Controller
        controller.Move(moveDirection * Time.deltaTime);

        if (controller.velocity.x > 0) {
        if (!facingRight) {
            TurnCharacterAround();
        }
        targetAnimator.SetBool("Walking", true);
        } else if (controller.velocity.x < 0){
            if (facingRight) {
                TurnCharacterAround();
            }
            targetAnimator.SetBool("Walking", true);
        } else {
            targetAnimator.SetBool("Walking", false);
        }
        
        //Zoom Camera In
        if (Input.GetButtonDown("ZoomIn")){
            if (targetZoom == mediumZoom) {
                targetZoom = closeZoom;
            } else if (targetZoom == farZoom){
                targetZoom = mediumZoom;
            }
        }

        //Zoom Camera Out
        if (Input.GetButtonDown("ZoomOut")){
            if (targetZoom == mediumZoom) {
                targetZoom = farZoom;
            } else if (targetZoom == closeZoom) {
                targetZoom = mediumZoom;
            }
        }

        //Update Camera Position
        targetCameraPos = new Vector3(target.transform.position.x, transform.position.y, targetZoom);
        transform.position = Vector3.Lerp(transform.position, targetCameraPos, 1 * followSpeed);
        transform.LookAt(target.transform.position + (Vector3.up*cameraHeight));
    }

    void SetTargetCharacter() {
        targetAnimator = target.GetComponent<Animator>();
        body = target.GetComponentInChildren<Rigidbody>();
    }

    void TurnCharacterAround() {
        if (facingRight) {
            target.transform.rotation = Quaternion.Euler(0, -90, 0);
        } else if (!facingRight) {
            target.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        facingRight = !facingRight;
    }
}