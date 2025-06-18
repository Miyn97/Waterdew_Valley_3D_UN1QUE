using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JmInvenHelpUI : MonoBehaviour
{
    public GameObject helpUI;

    void Start()
    {
        helpUI.SetActive(false);
    }

    public void ToggleUI()
    {
        helpUI.SetActive(!helpUI.activeSelf);
    }
}
