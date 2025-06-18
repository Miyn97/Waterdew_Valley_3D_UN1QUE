using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interact();
    string GetInteractPrompt();  // UI 표시용 텍스트
}
