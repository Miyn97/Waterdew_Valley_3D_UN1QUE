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

        transform.position = new Vector3(targetPos.x, surfaceTopY + offset, targetPos.z);
    }

    public void RotateClockwise90()
    {
        transform.Rotate(0f, 90f, 0f);
    }
}
