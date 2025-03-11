using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITrap
{
    public bool isActive { get; set;}
    
    void Activate();
    void Deactivate();
}
