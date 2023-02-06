
using UnityEngine;
using UnityEngine.UI;

public class KeyboardDisabler : MonoBehaviour
{
    void Start()
    {
        GetComponent<InputField>().keyboardType = (TouchScreenKeyboardType)(-1);
    }

}
