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
    
    public float noHungerHealthDecay;

    public event Action OnTakeDamage;
    void Update()
    {
    }

    public void Heal(float value)
    {
    }

    public void Eat(float value)
    {
    }
    
    public void Death()
    {
        Debug.Log("Death");
    }

    public void TakePhysicalDamage(int damage)
    {
    }

}
