using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnButtonClickIconTextColorChange : MonoBehaviour
{
    List<Image> IconComponent = new List<Image>();
    List<Text> TextComponent = new List<Text>();

    [SerializeField] Color BeforeClickColor;
    [SerializeField] Color AfterClickColor;

    private void Start()
    {

        TextComponent.AddRange(this.GetComponentsInChildren<Text>());
        IconComponent.AddRange(this.GetComponentsInChildren<Image>());
    }

    void ChangeIconColor(int ImageIndex)
    {
        foreach (Image im in IconComponent)
        {
            //im.color = new Color(0.2941177f, 0.6980392f, 0.5529412f);
            im.color = BeforeClickColor;
        }

        IconComponent[ImageIndex].color = AfterClickColor;
    }

    void ChangeTextColor(int index)
    {
        foreach (Text ti in TextComponent)
        {
            //            ti.color = new Color(0.2941177f, 0.6980392f, 0.5529412f);
            ti.color = BeforeClickColor;

        }

        TextComponent[index].color = AfterClickColor;
    }

    public void OnClickButton(int index)
    {
        ChangeTextColor(index);
        ChangeIconColor(index);
    }
}
