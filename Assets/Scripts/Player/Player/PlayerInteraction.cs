using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask interactableMask;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }

        ShowInteractPrompt();
    }

    void TryInteract()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableMask))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact();
            }
        }
    }

    void ShowInteractPrompt()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableMask))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                string prompt = interactable.GetInteractPrompt();
                // 여기에 UIManager 연결해서 상호작용 메시지 띄우면 됩니다.
                Debug.Log(prompt);
            }
        }
    }
}
