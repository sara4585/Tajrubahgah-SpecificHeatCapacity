 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InstructionsController : MonoBehaviour
{
    static InstructionsController instance;

    public static InstructionsController Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<InstructionsController>();
            if (instance == null)
                Debug.LogError("InstructionsController not found");
            return instance;
        }
    }

    public static string[] Ins = 
                    {"1. Turn on the weight machine. Take a bob from an inventory and find its constant mass by physical balance. ",
                     "2. Take an empty polystyrene cup from an inventory and find its constant mass. ",
                     "3. Place the empty polystyrene cup on the left side. ",
                     "4. Take a graduated cylinder from an inventory and add about 30 ml of water in it. ",
                     "5. Weight cup again to find the constant mass of the cup with water. ",
                     "6. Arrange the heating setup apparatus from the inventory to heat up the solid. ",
                     "7. Take a beaker from an inventory. Add about 150 ml of water in it. ",
                     "8. Place the beaker on tripod stand. ",
                     "9. Take the thermometer from an inventory and Suspend it from the stand clamp. ",
                     "10. Turn on the burner and heat the solid bob until water starts boiling. ",
                     "11. Arrange the other apparatus for polystyrene cup setup. ",
                     "12. Adjust the thermometer of left side stand such that its bulb rests 1 cm above the bottom of cup. ",
                     "13. Observe the thermometer carefully and Enter the CONSTANT temperature of water in the cup. ",
                     "14. Adjust the left side thermometer and take it out from the cup filled with water. ",
                     "15. Now wait for water in the beaker to boil and observe thermometer in beaker by zooming the thermometer. ",
                     "16. Water is boiling in the beaker, take the bob from inventory and place in the boiling water using clamp. ",
                     "17. Place bob in boiling water for 10 minutes and wait. So that temperature of bob and water becomes same. ",
                     "18. Enter the CONSTANT temperature of the boiling water. This is also the Temperature of Bob. ",
                     "19. Quickly but Carefully transfer the hot bob into the polystyrene cup so that water will not splash and water mass remains same. ",
                     "20. Move down the thermometer into cup. ",
                     "21. Take lid from an inventory and cover the cup. " ,
                     "22. Shake the cup gently so that temperature of bob and water in cup becomes equal. " ,
                     "23. Observe the rise in temperature of thermometer in the cup. Enter the CONSTANT temperature before it begins to decrease.",
                     ""
    };

    [HideInInspector]
    public int InsIndex;

    TextMeshPro Text;
    string TextToWrite;
    [SerializeField] float TimePerCharacter;
    int CharIndex;
    float Timer;
    bool InvisibleCharacter;
    bool Type;

    SfxHandler sfx = null;
    bool playSound;

    public float _wait;

    // Start is called before the first frame update
    void Awake()
    {
        Text = this.GetComponentInChildren<TextMeshPro>();
        UpdateInstruction(0, TimePerCharacter, true, true);
        playSound = false;
      //  SuccessFeedbackTick.SetActive(false);
    }

    private void Start()
    {
        InsIndex = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (sfx == null)
            sfx = SfxHandler.SfxIns;

        if (Type)
        {
            if (sfx != null && !playSound)
            {
                sfx.PlaySound("typing_s");
                playSound = true;
            }

            Timer -= Time.deltaTime;
            while (Timer <= 0f)
            {
                Timer += TimePerCharacter;
                string txt = TextToWrite.Substring(0, CharIndex);

                if (InvisibleCharacter)
                {
                    txt += "<color=#00000000>" + TextToWrite.Substring(CharIndex) + "</color>";
                }

                Text.text = txt;
                CharIndex++;

                if (CharIndex > TextToWrite.Length - 1)
                {
                    Type = false;
                    if (sfx != null && playSound)
                    {
                        sfx.StopSound("typing_s");
                        playSound = false;
                    }
                    
                    gameObject.GetComponentInChildren<ScaleUpDownHandler>().enabled = true;
                    StartCoroutine(Wait(_wait));

                    return;
                }
            }
        }
    }

    IEnumerator Wait(float wait)
    {
        yield return new WaitForSeconds(wait);

        InstructionsController.Instance.gameObject.GetComponentInChildren<ScaleUpDownHandler>().enabled = false;
       
    }

    void UpdateInstruction(int index, float TimePerCharacter, bool InvisibleCharacter, bool Type)
    {
            TextToWrite = Ins[index];
            this.TimePerCharacter = TimePerCharacter;
            this.CharIndex = 0;
            this.InvisibleCharacter = InvisibleCharacter;
            this.Type = Type;
    }

    public void IncInsIndex()
    {
        Debug.Log("index: " + (InsIndex + 1) + " indexLength: " + Ins.Length);

        if (InsIndex + 1 < Ins.Length)
        {
 //           SuccessFeedbackTick.SetActive(true);
            StartCoroutine(WaitForSuccessMsgDisappear(0.5f));
        }
    }

    IEnumerator WaitForSuccessMsgDisappear(float time)
    {
        yield return new WaitForSeconds(time);
   //     SuccessFeedbackTick.SetActive(false);
        UpdateInstruction(++InsIndex, TimePerCharacter, true, true);
    }
}
