using UnityEngine;

public class BuildPreview : MonoBehaviour
{
    public MeshRenderer previewRenderer;
    public Material validMat;
    public Material invalidMat;

    public void SetValid(bool isValid)
    {
        previewRenderer.material = isValid ? validMat : invalidMat;
    }

    public void SetPosition(Vector3 targetPos, Collider surfaceCollider)
    {
        if (surfaceCollider == null)
        {
            // fallback: 그냥 중심 위치
            transform.position = targetPos;
            return;
        }

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            transform.position = targetPos;
            return;
        }

        float minY = float.MaxValue;
        foreach (Renderer r in renderers)
        {
            minY = Mathf.Min(minY, r.bounds.min.y);
        }

        float offset = transform.position.y - minY;
        float surfaceTopY = surfaceCollider.bounds.max.y;

        // 변에 있어야하는 건물
        if (gameObject.GetComponent<Building>().data.isEdgeBuilding)
        {
            // '앞 방향'으로 밀어줌 (건물의 정면 방향)
            Vector3 forwardOffset = transform.forward.normalized * 1f; // 또는 2f

            transform.position = new Vector3(
                targetPos.x + forwardOffset.x,
                surfaceTopY + offset,
                targetPos.z + forwardOffset.z
            );
        }
        else
        {
            transform.position = new Vector3(targetPos.x, surfaceTopY + offset, targetPos.z);
        }
    }

    public void RotateClockwise90()
    {
        transform.Rotate(0f, 90f, 0f);
    }
}
