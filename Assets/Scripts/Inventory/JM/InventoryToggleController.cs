using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryToggleController : MonoBehaviour
{
    public GameObject inventoryUI; // 인벤토리 UI 오브젝트 (Canvas)
    private bool isInventoryOpen = false;

    private void Start()
    {
        inventoryUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryUI.SetActive(isInventoryOpen);

        // 마우스 커서 상태도 바꿔주자!
        Cursor.visible = isInventoryOpen;
        Cursor.lockState = isInventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;

        // 혹시 사운드나 애니메이션도 추가하고 싶다면 여기서!
    }
}
