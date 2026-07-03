using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Animator animator;
    public float walkSeep;
    public float runSeep;
    private CharacterController characterController;
    public GameObject _camera;
    public float gravity;
    public float Max_fall_speed;
    private float verticalSpeed;
    private bool cut = false;
    public float scanRadius = 5f;
    public string targetTag;
    public Container container;
    public GameObject Null;
    public GameObject GameUI;
    public void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        characterController = gameObject.GetComponent<CharacterController>();
        container = gameObject.GetComponent<Container>();
    }

    public void Update()
    {
        if(GameUI.activeInHierarchy)
        {
            HorizontalMove();
        }
        VerticalMove();
    }

    public void VerticalMove()
    {
        
        if (characterController.isGrounded && verticalSpeed < 0)        
            verticalSpeed = 0.5f;       
        
        if(verticalSpeed <= Max_fall_speed)
            verticalSpeed += gravity * Time.deltaTime;     

        characterController.Move(new Vector3(0, -verticalSpeed, 0) * Time.deltaTime);
    }


    public void HorizontalMove()
    {
        float z = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");
        Vector3 vector = new Vector3(x, 0, z);
        if (vector == Vector3.zero)
        {
            if (cut)
            {
                animator.SetBool("Jump", false);
                animator.SetBool("Walk", false);
                animator.SetBool("Run", false);
                animator.SetBool("Idle", true);
                cut = false;
             
            }
            if (Input.GetButtonDown("Rest"))
            {
                animator.SetBool("Idle", false);
                animator.SetTrigger("Rest");
            }
        }
        else
        {
            cut = true;
            animator.SetBool("Idle", false);
            animator.SetBool("Jump", false);
            animator.SetBool("Rest", false);
            Vector3 camX = _camera.GetComponent<Transform>().right;
            Vector3 camZ = _camera.GetComponent<Transform>().forward;
            camX.y = 0;
            camZ.y = 0;
            Vector3 moveVector = (camX * x) + (camZ * z);
            if (Input.GetButton("left shift"))
            {

                animator.SetBool("Walk", false);
                animator.SetBool("Run", true);
                characterController.Move(moveVector * runSeep * Time.deltaTime);
                transform.rotation = Quaternion.LookRotation(moveVector);
            }
            else
            {
                animator.SetBool("Walk", true);
                animator.SetBool("Run", false);
                characterController.Move(moveVector * walkSeep * Time.deltaTime);
                transform.rotation = Quaternion.LookRotation(moveVector);
            }
            
        }
    }

    public void Pickup()
    {
        if (Input.GetButtonDown("Pickup")){
            Collider[] cols = Physics.OverlapSphere(transform.position, scanRadius);
            foreach (var col in cols)
            {
                if (col.CompareTag(targetTag))
                {
                    container.Add((int)col.GetComponent<DropItems>().ID, 1);
                    Destroy(col);
                }
            }
        }
    }
}
