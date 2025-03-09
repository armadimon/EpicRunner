using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Transform player;  // 플레이어 본체
    public float cameraOffset = 0.2f;  // 벽과의 최소 거리
    public LayerMask obstacleLayer;  // 충돌 감지할 레이어

    void LateUpdate()
    {
        Vector3 cameraPosition = player.position + new Vector3(0, 1.6f, 0); // 플레이어 머리 높이
        Vector3 desiredPosition = transform.position; // 카메라 원래 위치

        if (Physics.Raycast(cameraPosition, (desiredPosition - cameraPosition).normalized, out RaycastHit hit, Vector3.Distance(cameraPosition, desiredPosition), obstacleLayer))
        {
            transform.position = hit.point + hit.normal * cameraOffset;  // 벽에서 살짝 띄우기
        }
        else
        {
            transform.position = desiredPosition;
        }
    }
}