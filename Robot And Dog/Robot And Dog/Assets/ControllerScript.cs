using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour{

    public GameObject target;
    Animator targetAnimator;
    Rigidbody body;
    private float walkSpeed = 0.05f;
    private bool facingRight = true;

    
    void Awake(){
        SetTargetCharacter();
    }
    
    void Update(){
        if (Input.GetButton("WalkRight")) {
            if (!facingRight) {
                TurnCharacterAround();
            }
            targetAnimator.SetBool("Walking", true);
            target.transform.position += Vector3.right * walkSpeed;
            targetAnimator.SetFloat("charSpeed", body.velocity.magnitude); 
        } else if (Input.GetButton("WalkLeft")) {
            if (facingRight){
                TurnCharacterAround();
            }
            targetAnimator.SetBool("Walking", true);
            target.transform.position -= Vector3.right * walkSpeed;
        }
        if (body.velocity.magnitude <= 0.001f) {
            targetAnimator.SetBool("Walking", false);
        }
    }

    void SetTargetCharacter() {
        targetAnimator = target.GetComponent<Animator>();
        body = target.GetComponentInChildren<Rigidbody>();
    }

    void TurnCharacterAround() {

        if (facingRight) {
            target.transform.rotation = Quaternion.Euler(0, -90, 0);
        } else {
            target.transform.rotation = Quaternion.Euler(0, 90, 0);

        }
        facingRight = !facingRight;
    }
}