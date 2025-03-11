using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrap : MonoBehaviour, ITrap
{
    public bool isActive { get; set;}
    public Transform firePoint;
    public float range = 10f;
    public LayerMask targetLayer;
    [SerializeField] private List<GameObject> trapObjects;
    private List<ITrap> traps = new List<ITrap>();
    public bool isLeft = true;

    void Start()
    {
        isActive = true;
        foreach (var trapObject in trapObjects)
        {
            ITrap trap = trapObject.GetComponent<ITrap>();
            if (trap != null)
            {
                traps.Add(trap);
            }
        }
    }

    public void Activate()
    {
        Debug.Log("Activate Laser Trap");
        isActive = true;
    }

    public void Deactivate()
    {
        Debug.Log("DeActivate Laser Trap");
        isActive = false;
    }
    
    void Update()
    {
        if (isActive)
            ShootLaser();
    }

    void ShootLaser()
    {
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, isLeft == true ? -(firePoint.right) : firePoint.right, out hit, range, targetLayer))
        {
            for (int i = 0; i < traps.Count; i++)
            {
                if (traps[i].isActive == true)
                    traps[i].Activate();
            }
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(firePoint.position, firePoint.position + (isLeft == true ? -(firePoint.right) : firePoint.right) * range);
    }
}
