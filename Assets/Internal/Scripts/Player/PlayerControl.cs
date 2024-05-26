using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    public CharacterController characterController { get; private set; }

    Vector3 velocity = Vector3.zero;

    float moveSpeed = 2;

    [SerializeField] TMP_Text nicknameText;

    Vector3 prevPosition;

    public void Init(string  nickname)
    {
        nicknameText.text = nickname;
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        InvokeRepeating("SendPlayerData", 0, 0.1f);
    }

    private void Update()
    {
     
    }

    private void FixedUpdate()
    {
        if (!RoomManager.onRoom) return;

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

            if (EventSystem.current
         && EventSystem.current.currentSelectedGameObject
         && EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMP_InputField>())
            {
                movement = Vector3.zero;
            }

            characterController.Move(movement * moveSpeed * Time.fixedDeltaTime + velocity * Time.fixedDeltaTime);
        }
        //characterController.Move(velocity * Time.fixedDeltaTime);
    }

    private void SendPlayerData()
    {
        bool flag = false;
        if (prevPosition != transform.position)
        {
            prevPosition = transform.position;
            flag = true;
        }

        if (flag)
        {
            NetworkPacketSender.inst.SendRoomPlayerDataPacket(transform.position);
        }
    }
}
