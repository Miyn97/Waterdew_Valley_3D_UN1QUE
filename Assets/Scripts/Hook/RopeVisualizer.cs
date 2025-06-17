using UnityEngine;

public class RopeVisualizer : MonoBehaviour
{
    public Transform startPoint; // Hand
    public Transform endPoint;   // Hook
    public Transform ropeMesh;   // Cylinder mesh

    void LateUpdate()
    {
        if (startPoint == null || endPoint == null || ropeMesh == null)
            return;

        Vector3 dir = endPoint.position - startPoint.position;
        float distance = dir.magnitude;

        if (distance < 0.001f)
            return;

        // 중간 위치에 배치
        transform.position = startPoint.position + dir / 2f;

        // 방향 설정 (Y축을 기준으로)
        transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        // 길이 설정 (Cylinder 기본 길이 = 2이므로 0.5 곱)
        ropeMesh.localScale = new Vector3(0.05f, distance / 2f, 0.05f);
    }
}
