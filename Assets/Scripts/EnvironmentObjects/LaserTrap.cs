using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrap : MonoBehaviour
{
    public Transform firePoint;
    public float range = 10f;
    public LayerMask targetLayer;
    public SpikeTrap[] spikeTraps;
    public bool isLeft = true;

    void Update()
    {
        ShootLaser();
    }

    void ShootLaser()
    {
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, isLeft == true ? -(firePoint.right) : firePoint.right, out hit, range, targetLayer))
        {
            for (int i = 0; i < spikeTraps.Length; i++)
            {
                spikeTraps[i].PopUp();
            }
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(firePoint.position, firePoint.position + (isLeft == true ? -(firePoint.right) : firePoint.right) * range);
    }
}
