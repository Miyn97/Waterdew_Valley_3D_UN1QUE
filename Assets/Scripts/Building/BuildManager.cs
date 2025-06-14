using System.Collections.Generic;
using System.Linq;

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
        HandleRotate();
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
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetBuildItem(datas[2]);
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

        if (datas.IndexOf(currentData) == 0) // 타일 관련
        {
            Vector2Int gridPos = WorldToGrid(mousePos);
            bool valid = ship.GetBuildablePositions().Contains(gridPos);

            if (valid)
            {
                currentPreview.SetPosition(GridToWorld(gridPos), ship.GetComponent<Collider>());
            }
            else
            {
                currentPreview.SetPosition(mousePos, null);
            }

            currentPreview.SetValid(valid);
        }
        else    // 건물 오브젝트 관련
        {
            IBuildableSurface surface = GetSurfaceUnderMouse();
            if (surface != null && surface.CanBuildHere(mousePos))
            {
                Vector3 snappedPos = surface.GetSnappedPosition(mousePos);
                Collider surfaceCol = (surface as MonoBehaviour)?.GetComponent<Collider>(); // 안전하게 캐스팅
                currentPreview.SetPosition(snappedPos, surfaceCol);
                currentPreview.SetValid(true);
            }
            else
            {
                currentPreview.SetPosition(mousePos, null);
                currentPreview.SetValid(false);
            }
        }
    }

    void HandlePlacement()
    {
        if (currentPreview == null || currentData == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = GetMouseWorldPosition();

            if (datas.IndexOf(currentData) == 0)    // 타일 설치
            {
                Vector2Int gridPos = WorldToGrid(mousePos);
                if (ship.GetBuildablePositions().Contains(gridPos))
                {
                    Vector3 spawnPos = GridToWorld(gridPos);
                    GameObject tile = Instantiate(currentData.prefab, spawnPos, Quaternion.identity, ship.transform);
                    Tile tileScript = tile.GetComponent<Tile>();
                    tileScript.gridPosition = gridPos;
                    ship.RegisterTile(gridPos, tileScript);
                }
            }
            else    // 건물 오브젝트 설치
            {
                IBuildableSurface surface = GetSurfaceUnderMouse();
                if (surface != null && surface.CanBuildHere(mousePos))
                {
                    Vector3 snapped = surface.GetSnappedPosition(mousePos);
                    Instantiate(currentData.prefab, currentPreview.transform.position, currentPreview.transform.rotation);
                    surface.RegisterBuild(snapped);
                }
            }
        }
    }

    // 오른쪽으로 90도 회전
    void HandleRotate()
    {
        if (datas.IndexOf(currentData) != 0 && currentPreview != null && Input.GetKeyDown(KeyCode.R))
        {
            currentPreview.RotateClockwise90();
        }
    }

    // 타일 전용
    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    // 빌딩 전용
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
