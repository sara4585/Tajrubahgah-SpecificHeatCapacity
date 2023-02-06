using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CalUnitConverterTogglerBtnHandler : MonoBehaviour
{
    [SerializeField] GameObject Calculator;
    [SerializeField] GameObject UnitConverter;

    RectTransform TogglerRightUnit;
    RectTransform TogglerLeftCal;        //images

    int BtnPressingCount;

    private void Start()
    {
        BtnPressingCount = 0;
        TogglerRightUnit = transform.Find("TogglerRight").gameObject.GetComponent<RectTransform>();
        TogglerLeftCal = transform.Find("TogglerLeft").gameObject.GetComponent<RectTransform>();
        
        TogglerRightUnit.gameObject.SetActive(false);
        UnitConverter.SetActive(false);
    }

    void Slide(RectTransform rectTransform,  Vector3 targetPosition)
    {
        rectTransform.DOMove(targetPosition, 0.25f);
    }

    public void OnClickTogglerBtn()
    {
        BtnPressingCount++;
        if (BtnPressingCount % 2 == 1)  //show unit converter
        {
            TogglerLeftCal.gameObject.SetActive(false);
            TogglerRightUnit.gameObject.SetActive(true);
            Calculator.SetActive(false);
            Debug.Log("hey toggler..");
            UnitConverter.SetActive(true);
        }
        else
        {
            TogglerRightUnit.gameObject.SetActive(false);
            TogglerLeftCal.gameObject.SetActive(true);
            UnitConverter.SetActive(false);
            Calculator.SetActive(true);
            BtnPressingCount = 0;
        }
    }



}
