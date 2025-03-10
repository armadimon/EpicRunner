using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;
    
    public Transform slotPanel;
    public Transform dropPostion;
    
    // [Header("Select Item")]
    // public TextMeshProUGUI selectItemName;
    // public TextMeshProUGUI selectItemDescription;
    // public TextMeshProUGUI selectStatName;
    // public TextMeshProUGUI selectStatValue;
    // public GameObject useButton;
    // public GameObject equipButton;
    // public GameObject unEquipButton;
    // public GameObject dropButton;

    private PlayerController _controller;
    private PlayerCondition _condition;

    private ItemData _selectedItem;
    private int _selectedItemIndex = 0;
    
    int curEquipIndex;
    private void Start()
    {
        _controller = CharacterManager.Instance.Player.controller;
        _condition = CharacterManager.Instance.Player.condition;
        dropPostion = CharacterManager.Instance.Player.dropPosition;

        CharacterManager.Instance.Player.addItem += AddItem;
        
        // inventoryWindow.SetActive(false);
        _controller.itemQuckSlot += SelectItemSlot;
        slots = new ItemSlot[gameObject.transform.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = gameObject.transform.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }
        
        // ClearSelecteditemWindow();
        UpdateUI();
    }

    // void ClearSelecteditemWindow()
    // {
    //     selectItemName.text = String.Empty;
    //     selectItemDescription.text = String.Empty;
    //     selectStatName.text = String.Empty;
    //     selectStatValue.text = String.Empty;
    //     
    //     useButton.SetActive(false);
    //     equipButton.SetActive(false);
    //     unEquipButton.SetActive(false);
    //     dropButton.SetActive(false);
    // }

    void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.interactItem;

        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.interactItem = null;
                return ;
            }

        }
        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.interactItem = null;
            return ;
        }
            
        ThrowItem(data);
        CharacterManager.Instance.Player.interactItem = null;
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    ItemSlot GetItemStack(ItemData itemData)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == itemData && slots[i].quantity < itemData.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    void ThrowItem(ItemData itemData)
    {
        Instantiate(itemData.dropPrefab, dropPostion.position, Quaternion.Euler(Vector3.one * UnityEngine.Random.value * 360 ));
    }

    public void SelectItem(int index)
    {
        Debug.Log("slots[index].item" + slots[index].item);
        if (slots[index].item == null)
        {
            return;
        }
        _selectedItem = slots[index].item;
        _selectedItemIndex = index;
    }

    public void SelectItemSlot(string key)
    {
        Debug.Log("Key : " + key);
        if (key == "q")
        {
            SelectItem(0);
        }
        else if (key == "e")
        {
            SelectItem(1);
        }

        if (_selectedItem == null)
            return ;
        UseItem();
    }
    
    public void UseItem()
    {
        if (_selectedItem.type == ItemType.Consumable)
        {
            for (int i = 0; i < _selectedItem.consumables.Length; i++)
            {
                switch (_selectedItem.consumables[i].type)
                {
                    case ConsumableType.Health:
                        _condition.Heal(_selectedItem.consumables[i].value);
                        break;
                    case ConsumableType.Speed:
                        _controller.ActiveBoost(_selectedItem.consumables[i].type, _selectedItem.consumables[i].value);
                        break;
                    case ConsumableType.Jump:
                        _controller.ActiveBoost(_selectedItem.consumables[i].type, _selectedItem.consumables[i].value);
                        break;
                }
            }
        }
        RemoveSelectedItem();
    }
    //
    // public void OnDropButton()
    // {
    //     ThrowItem(_selectedItem);
    //     RemoveSelectedItem();
    // }

    void RemoveSelectedItem()
    {
        slots[_selectedItemIndex].quantity--;
    
        if (slots[_selectedItemIndex].quantity <= 0)
        {
            _selectedItem = null;
            slots[_selectedItemIndex].item = null;
            _selectedItemIndex = -1;
        }
        UpdateUI();
    }

    // public void OnEquipButton()
    // {
    //     if (slots[curEquipIndex].equipped)
    //     {
    //         UnEquip(curEquipIndex);
    //     }
    //     
    //     slots[_selectedItemIndex].equipped = true;
    //     curEquipIndex = _selectedItemIndex;
    //     CharacterManager.Instance.Player.equip.EquipNew(_selectedItem);
    //     UpdateUI();
    //     
    //     SelectItem(_selectedItemIndex);
    // }

    // void UnEquip(int index)
    // {
    //     slots[index].equipped = false;
    //     CharacterManager.Instance.Player.equip.UnEquip();
    //     UpdateUI();
    //
    //     if (_selectedItemIndex == index)
    //     {
    //         SelectItem(index);
    //     }
    // }

    // public void OnUnEquipButton()
    // {
    //     UnEquip(_selectedItemIndex);
    // }
}