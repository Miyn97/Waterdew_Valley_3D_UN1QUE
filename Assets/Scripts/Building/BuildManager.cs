using UnityEngine;

public class BuildManager : Singleton<BuildManager>
{
    [Header("현재 설치 중인 건물")]
    public BuildingData currentBuildingData;
    public GameObject previewInstance;
    public IBuildableObject previewScript;

    [Header("설치 가능한 바닥 레이어")]
    [SerializeField] private LayerMask buildableSurfaceMask;

    private bool isBuilding = false;
    private int currentRotation = 0;

    protected override void Awake()
    {
        base.Awake();
        previewInstance = currentBuildingData.prefabPreview;
    }

    private void Update()
    {
        if (!isBuilding || previewInstance == null) return;

        UpdatePreviewPosition();

        if (Input.GetKeyDown(KeyCode.R))
        {
            RotatePreview();
        }

        if (Input.GetMouseButtonDown(0))
        {
            TryPlaceBuilding();
        }
    }

    public void StartPlacingBuilding(BuildingData data)
    {
        if (previewInstance != null) Destroy(previewInstance);

        currentBuildingData = data;
        previewInstance = Instantiate(data.prefabPreview);
        previewScript = previewInstance.GetComponent<IBuildableObject>();
        isBuilding = true;
        currentRotation = 0;
    }

    private void UpdatePreviewPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, buildableSurfaceMask))
        {
            IBuildableSurface surface = hit.collider.GetComponent<IBuildableSurface>();
            if (surface != null)
            {
                Vector3 snapPos = surface.GetSnappedPosition(hit.point);
                previewInstance.transform.position = snapPos;

                // 설치 가능 여부에 따라 색상 변경
                bool canBuild = surface.CanBuildHere(snapPos);
                SetPreviewColor(canBuild ? Color.green : Color.red);
            }
        }
    }

    private void RotatePreview()
    {
        previewInstance.transform.Rotate(0, 90, 0);
        currentRotation = (currentRotation + 90) % 360;
        previewScript.Rotate();
    }

    private void TryPlaceBuilding()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, buildableSurfaceMask))
        {
            IBuildableSurface surface = hit.collider.GetComponent<IBuildableSurface>();
            if (surface != null)
            {
                Vector3 snapPos = surface.GetSnappedPosition(hit.point);
                if (surface.CanBuildHere(snapPos))
                {
                    GameObject placed = Instantiate(currentBuildingData.prefab, snapPos, Quaternion.Euler(0, currentRotation, 0));
                    surface.RegisterBuild(snapPos);
                    StopPlacing();
                }
            }
        }
    }

    private void SetPreviewColor(Color color)
    {
        foreach (var renderer in previewInstance.GetComponentsInChildren<Renderer>())
        {
            foreach (var mat in renderer.materials)
            {
                if (mat.HasProperty("_Color"))
                {
                    mat.color = color;
                }
            }
        }
    }

    public void StopPlacing()
    {
        if (previewInstance != null) Destroy(previewInstance);
        previewInstance = null;
        previewScript = null;
        isBuilding = false;
    }
}
