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

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
}
