using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    public CharacterController characterController { get; private set; }

    Vector3 velocity = Vector3.zero;

    float moveSpeed = 2;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
     
    }

    private void FixedUpdate()
    {
        if (characterController.isGrounded)
        {
            velocity.y = 0;
        }
        else
        {
            velocity.y -= Time.fixedDeltaTime * Constants.GRAIVTY;
        }

        {
            Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            characterController.Move(movement * moveSpeed * Time.fixedDeltaTime + velocity * Time.fixedDeltaTime);
        }
        //characterController.Move(velocity * Time.fixedDeltaTime);
    }
}
