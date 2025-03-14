using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 10f;
    private float startX;
    private float timeOffset;

    void Start()
    {
        startX = transform.position.x;
        timeOffset = Random.Range(0f, Mathf.PI * 2);
    }

    void Update()
    {
        float x = startX + Mathf.Sin(Time.time * speed + timeOffset) * distance;
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}
