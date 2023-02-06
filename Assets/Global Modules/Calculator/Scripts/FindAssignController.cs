using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Tajurbah_Gah
{
    public class FindAssignController : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] int index;
        public void OnPointerDown(PointerEventData eventData)
        {
            AssignToInput();
        }

        void AssignToInput()
        {
            AssignButtonController assignButtonController = FindObjectOfType<AssignButtonController>();

            if (assignButtonController!=null)
            {
                InputField inputField = GetComponent<InputField>();
                Debug.Log("assign input field called in find assign controller: " + inputField.name);
                if (inputField.interactable == true && ObservationsFormHandler.ObservationsFormHandlerInstance.ValidPointerDownField(index))

                {
                    Debug.Log("true....");
                    assignButtonController.AssignInputField(inputField, index);
                    ObservationsFormHandler.ObservationsFormHandlerInstance.ResetInputField(inputField);
                }
            }
        }
    }
}