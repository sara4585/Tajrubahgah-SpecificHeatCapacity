using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class HideAndSeekTextAnimation : MonoBehaviour
{
    Color TextColor;

    [SerializeField] Color VisibleColor;
    [SerializeField] Color InvisibleColor;

    float t = 0;

    private void Start()
    {
        TextColor = (this.GetComponent<TextMeshPro>().color);
        
    }

    void FixedUpdate()
    {
        Debug.Log("text color: " + TextColor);
        t += Time.deltaTime;
        TextColor = Color.Lerp(VisibleColor, InvisibleColor, t);

        if (t > 1)
        {
            Color TempColor;
            TempColor = VisibleColor;
            VisibleColor = InvisibleColor;
            InvisibleColor = TempColor;
            t = 0f;
        }
    }
}
