using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask interactableMask;

    [Header("UI")]
    [SerializeField] private GameObject interactUI;          // UI Panel
    [SerializeField] private TextMeshProUGUI promptText;     // "Press E to interact" 같은 텍스트

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        interactUI.SetActive(false);  // 시작 시 UI 숨김
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }

        HandleInteractPrompt();
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

    void HandleInteractPrompt()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableMask))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                interactUI.SetActive(true);
                promptText.text = interactable.GetInteractPrompt();
                return;
            }
        }

        interactUI.SetActive(false);  // 범위 밖이면 숨김
    }
}
