using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    public GameObject inven;

    private void Start()
    {
        inven.SetActive(false);
    }

    public void InvenUISet()
    {
        inven.SetActive(!inven.activeSelf);
    }
}
