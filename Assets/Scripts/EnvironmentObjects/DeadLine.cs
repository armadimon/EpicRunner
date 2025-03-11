using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLine : MonoBehaviour
{
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = FindObjectOfType<Player>().gameObject.transform.position;
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);
        {
            CharacterManager.Instance.Player.condition.TakePhysicalDamage(20);
            CharacterManager.Instance.Player.transform.position = startPosition;
        }
    }
}
