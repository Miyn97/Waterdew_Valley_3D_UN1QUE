using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class JmInvenInfoManager : MonoBehaviour
{
    public GameObject iconObj;
    Image iconObjImage;

    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI desTxt;

    private void Awake()
    {
        iconObjImage = iconObj.GetComponent<Image>();
    }

    public void UpdateInfoState(TestItem slot)
    {
        if (slot == null)
        {
            //Debug.LogWarning("InfoManager에 전달된 아이템이 null입니다!");
            return;
        }

        iconObjImage.sprite = slot.icon;
        nameTxt.text = slot.name;
        desTxt.text = slot.des;
    }
}
