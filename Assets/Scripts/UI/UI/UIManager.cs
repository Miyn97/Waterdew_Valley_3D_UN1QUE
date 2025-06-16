using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject optionSection;

    private void Start()
    {
        ResetUI();
    }
    void ResetUI()
    {
        optionSection.SetActive(false);
    }

    public void OptionUISet()
    {
        optionSection.SetActive(!optionSection.activeSelf);
    }
}
