using UnityEngine;

public class BuildInput : MonoBehaviour
{
    [SerializeField] private BuildingData cubeData;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (cubeData != null)
            {
                BuildManager.Instance.StartPlacingBuilding(cubeData);
            }
            else
            {
                Debug.LogWarning("cubeData가 비어 있음!");
            }
        }
    }
}
