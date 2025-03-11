using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string GetInterfacePrompt();
    public void DeactivatePrompt();
    public void OnInteract();

}
public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public virtual string GetInterfacePrompt()
    {
        string str = $"[ {data.displayName} ]\n{data.description}";
        return str;
    }

    public void DeactivatePrompt()
    {

    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.interactItem = data;
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }
}
