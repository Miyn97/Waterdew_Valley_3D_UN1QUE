using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class JmSlot : MonoBehaviour
{
    public ItemBaseData currentItem;
    public Image iconImage;

    public TextMeshProUGUI stackText;
    public GameObject quickSlotHighlight;

    public int slotIndex;

    [Header("퀵슬롯 관련")]
    public JmSlot linkedSlot = null;

    [Header("디폴트 설정")]
    public Sprite defaultSprite;

    private void Start()
    { 
        if(quickSlotHighlight != null)
        {
            quickSlotHighlight.SetActive(false);
        }

        if(stackText != null)
        {
            stackText.gameObject.SetActive(false);
        }
    }

    public void ClearSlot()
    {
        iconImage.sprite = defaultSprite;

        if(currentItem != null)
            currentItem = null;

        if (quickSlotHighlight.activeInHierarchy)
            quickSlotHighlight.SetActive(false);

        if (stackText.gameObject.activeInHierarchy)
            stackText.gameObject.SetActive(false);

        UpdateData();
    }

    public void SetIndex(int index)
    {
        slotIndex = index;
    }

    public void UpdateData()
    {
        if(currentItem == null)
        {
            iconImage.sprite = defaultSprite;
            stackText.gameObject.SetActive(false);
        }
        else
        {
            if (currentItem != null && currentItem.Icon != null)
                iconImage.sprite = currentItem.Icon;
            else
                iconImage.sprite = null;

            if (quickSlotHighlight != null)
            //    이건 설정하는거 따로 만들기
            {
                if(linkedSlot != null)
                {
                    quickSlotHighlight.SetActive(true);
                }
                else
                {
                    quickSlotHighlight.SetActive(false);
                }
            }


            if (currentItem.IsStackable)
            {
                stackText.gameObject.SetActive(true);
                stackText.text = currentItem.MaxStack.ToString();
                //    일단 max로 지정
                //    후에 생성된다면 이걸로 수정
            }
        }

    }
}
