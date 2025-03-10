using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrap : MonoBehaviour
{
    public Transform firePoint; // 레이저를 발사할 위치
    public float range = 10f;   // 레이저 감지 거리
    public LayerMask targetLayer; // 감지할 대상 레이어
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
        Gizmos.DrawLine(firePoint.position, firePoint.position + firePoint.right * range);
    }
}
