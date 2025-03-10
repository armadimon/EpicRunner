using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    
    public float upHeight = 1.5f;
    public float speed = 5f;
    public float resetDelay = 2f;
    private Vector3 initialPosition;
    private bool isUp = false;
    
    public float knockBackForce = 5f;
    
    private void Start()
    {
        initialPosition = transform.position;
        PopUp();
    }
    
        public void PopUp()
        {
            if (!isUp)
            {
                isUp = true;
                StopAllCoroutines();
                StartCoroutine(MoveSpike(initialPosition + Vector3.up * upHeight, true));
            }
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                CharacterManager.Instance.Player.condition.TakePhysicalDamage(20);
                CharacterManager.Instance.Player.controller.KnockBack(knockBackForce, (other.transform.position - transform.position).normalized);
            }
        }
        
        private IEnumerator MoveSpike(Vector3 targetPosition, bool goingUp)
        {
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
                yield return null;
            }

            if (goingUp)
            {
                yield return new WaitForSeconds(resetDelay);
                StartCoroutine(MoveSpike(initialPosition, false));
            }
            else
            {
                isUp = false;
            }
        }

    
}
