using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapButton  : MonoBehaviour, IInteractable
{
    public string objectDescription = "F를 눌러 상호작용";
    [SerializeField]
    private List<GameObject> trapObjects;
    private List<ITrap> traps = new List<ITrap>();
    
    public GameObject textObject;
    private TextMesh textMesh;
    private bool isNearObject = false;

    void Start()
    {
        foreach (var trapObject in trapObjects)
        {
            ITrap trap = trapObject.GetComponent<ITrap>();
            if (trap != null)
            {
                traps.Add(trap);
            }
        }
        
        textObject = new GameObject("ObjectDescriptionText");
        textObject.transform.SetParent(transform);
        textObject.transform.localPosition = new Vector3(0, 1, 0);
        textObject.SetActive(false);

        textMesh = textObject.AddComponent<TextMesh>();
        textMesh.text = objectDescription;
        textMesh.fontSize = 24;
        textMesh.characterSize = 0.05f;
        textMesh.alignment = TextAlignment.Center;
        textMesh.anchor = TextAnchor.MiddleCenter;
    }
    
    public string GetInterfacePrompt()
    {
        textObject.SetActive(true);
        return textMesh.text;
    }

    public void DeactivatePrompt()
    {
        textObject.SetActive(false);
    }
    
    public void OnInteract()
    {
        for (int i = 0; i < traps.Count; i++)
        {
            if (traps[i].isActive == true)
                traps[i].Deactivate();
            else if (traps[i].isActive == false)
                traps[i].Activate();
            
        }
    }
}
