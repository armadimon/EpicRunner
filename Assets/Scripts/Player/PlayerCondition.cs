using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface IDamageable
{
    void TakePhysicalDamage(int damage);
}

public class PlayerCondition : MonoBehaviour, IDamageable
{
    

    public UICondition uiCondition;

    Condition Health {get {return uiCondition.health;}}
    Condition Stamina {get {return uiCondition.stamina;}}

    public event Action OnTakeDamage;

    void Update()
    {
        Stamina.Add(Stamina.passiveValue * Time.deltaTime);
        if (Health.curValue <= 0f)
        {
            Death();
        }
    }
    
    public void Heal(float value)
    {
        Health.Add(value);
    }
    
    public void Death()
    {
        Debug.Log("Death");
    }

    public void TakePhysicalDamage(int damage)
    {
        Health.Subtract(damage);
        OnTakeDamage?.Invoke();
    }
    
    public bool UseStamina(float value)
    {
        if (Stamina.curValue - value < 0f)
        {
            return false;
        }
        
        Stamina.Subtract(value);
        return true;
    }
}
