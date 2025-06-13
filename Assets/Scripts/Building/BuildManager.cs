using System.Collections.Generic;

using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private List<BuildingData> datas;
    [SerializeField] private Ship ship;

    private BuildingData currentData;
    private BuildPreview currentPreview;

    void Update()
    {
        HandleNumberKeyInput();
        UpdatePreview();
        HandlePlacement();
    }

    void HandleNumberKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetBuildItem(datas[0]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetBuildItem(datas[1]);
        }
    }

    void SetBuildItem(BuildingData data)
    {
        if (currentPreview) Destroy(currentPreview.gameObject);

        currentData = data;
        GameObject previewObj = Instantiate(data.preview);
        currentPreview = previewObj.GetComponent<BuildPreview>();

        if (!currentPreview)
        {
            Debug.LogWarning($"BuildPreview가 {data.prefab.name}에 없습니다. 기본 컴포넌트 생성 시도합니다.");
        }
    }

    void UpdatePreview()
    {
        if (currentPreview == null || currentData == null) return;

        Vector3 mousePos = GetMouseWorldPosition();

        if (datas.IndexOf(currentData) == 0) // 타일 설치 모드
        {
            Vector2Int gridPos = WorldToGrid(mousePos);
            bool valid = ship.GetBuildablePositions().Contains(gridPos);

            currentPreview.SetPosition(GridToWorld(gridPos));
            currentPreview.SetValid(valid);
        }
        else // 빌딩 설치 모드
        {
            IBuildableSurface surface = GetSurfaceUnderMouse();
            if (surface != null && surface.CanBuildHere(mousePos))
            {
                currentPreview.SetPosition(surface.GetSnappedPosition(mousePos));
                currentPreview.SetValid(true);
            }
            else
            {
                currentPreview.SetPosition(mousePos);
                currentPreview.SetValid(false);
            }
        }
    }

    void HandlePlacement()
    {
        if (currentPreview == null || currentData == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 placePos = currentPreview.transform.position;

            if (datas.IndexOf(currentData) == 0) // 타일
            {
                Vector2Int gridPos = WorldToGrid(placePos);
                if (ship.GetBuildablePositions().Contains(gridPos))
                {
                    GameObject tile = Instantiate(currentData.prefab, placePos, Quaternion.identity, ship.transform);
                    Tile tileScript = tile.GetComponent<Tile>();
                    tileScript.gridPosition = gridPos;
                    ship.RegisterTile(gridPos, tileScript);
                }
            }
            else // 빌딩
            {
                IBuildableSurface surface = GetSurfaceUnderMouse();
                if (surface != null && surface.CanBuildHere(placePos))
                {
                    Instantiate(currentData.prefab, surface.GetSnappedPosition(placePos), Quaternion.identity);
                    surface.RegisterBuild(placePos);
                }
            }
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    IBuildableSurface GetSurfaceUnderMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            return hit.collider.GetComponent<IBuildableSurface>();
        }
        return null;
    }

    Vector2Int WorldToGrid(Vector3 world)
    {
        return new Vector2Int(Mathf.RoundToInt(world.x), Mathf.RoundToInt(world.z));
    }

    Vector3 GridToWorld(Vector2Int grid)
    {
        return new Vector3(grid.x, 0, grid.y);
    }
}
