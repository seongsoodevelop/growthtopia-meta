using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class NetworkPlayerControl : MonoBehaviour
{
    public CharacterController characterController { get; private set; }

    Vector3 velocity = Vector3.zero;

    [SerializeField] TMP_Text nicknameText;

    public string nickname { get; private set; } = "";

    Vector3 position;
    public void Init(string nickname)
    {
        this.nickname = nickname;
        nicknameText.text = nickname;
    }

    public void UpdateTransform(Vector3 position)
    {
        this.position = position;
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        if (!RoomManager.onRoom) return;

        transform.position = Vector3.Lerp(transform.position, position, 10 * Time.deltaTime);
    }
}
