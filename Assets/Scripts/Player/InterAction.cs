using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InterAction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float _lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;
    
    public GameObject curInteractGameObject;
    
    public TextMeshProUGUI promptText;
    private Camera _camera;


    private void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (Time.time - _lastCheckTime > checkRate)
        {
            _lastCheckTime = Time.time;
            Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    SetPromptText();
                }
            }
            else
            {
                curInteractGameObject = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }
    
    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            curInteractGameObject = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
