using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpPower;
    public float cooldown;
    public bool isActivated = true;
    
    private MeshRenderer _meshRenderer;


    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.material.color = Color.green;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && isActivated)
        {
            Debug.Log(other.gameObject.name);
            isActivated = false;
            _meshRenderer.material.color = Color.red;
            StartSuperJumpCorutine();
        }
    }

    private void StartSuperJumpCorutine()
    {
        StartCoroutine(StartSuperJump());
    }
    
    private IEnumerator StartSuperJump()
    {
        CharacterManager.Instance.Player.controller.SuperJump(jumpPower);
        yield return new WaitForSeconds(cooldown);
        isActivated = true;
        _meshRenderer.material.color = Color.green;
    }
}
