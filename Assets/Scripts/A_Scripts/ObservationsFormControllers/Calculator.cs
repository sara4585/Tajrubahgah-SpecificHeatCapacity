using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System;

public class Calculator : MonoBehaviour
{
    #region AttributesDeclaration

    [SerializeField] Text InputField;
    [SerializeField] Text OutputField;
    [SerializeField] TextMeshProUGUI OpenBracketText;

    bool ClearOutput;                   //allow fresh data in output field 

    string MemoryStoredValue;

    int MaxOpenBracketsSize;
    int OpenBracketCurrentSize;

    bool IsOperatorPressed ;         //handle the repeatition of pressed operator

    List<int> OpenBracketIndexList;

    bool ResultCalculated;

    #endregion AttributesDeclaration

    void Start()
    {
        OutputField.text = "0";
        InputField.text = "";
        MemoryStoredValue = "";

        ResultCalculated = false;

        DeactivateOperatorPressedStatus();

        ActivateClearOutputStatus();

        MaxOpenBracketsSize = 25;

        OpenBracketCurrentSize = -1;    //update status frst increment then assign
        UpdateOpenBracketStatus(OpenBracketCurrentSize);

        OpenBracketIndexList = new List<int>(MaxOpenBracketsSize);
    }
    
    public void digitClick(string digit)
    {
        if (ResultCalculated)
        {
            InputField.text = "";
            ResultCalculated = false;
        }

        if (ClearOutput)
        {
            DeactivateClearOutputStatus();

            if (digit == "<<<")     //del button
            {
                ActivateClearOutputStatus();
                return;
            }

            else if (digit == "0")
            {
                ActivateClearOutputStatus();

                if (OutputField.text.Equals("0"))
                    return;

                else
                    OutputField.text = digit;
            }

            else if (digit == ".")
                OutputField.text = "0" + digit;

            else
                OutputField.text = digit;
        }

        else
        {
            if (digit == "." && OutputField.text.Contains("."))
                return;

            else if (digit == "<<<")
            {
                int OTPFLen = OutputField.text.Length - 1;

                if (OTPFLen + 1 == 1)
                {
                    ActivateClearOutputStatus();
                    OutputField.text = "0";
                }

                else
                    OutputField.text = OutputField.text.Remove(OTPFLen, 1);
            }

            else
                OutputField.text += digit;
        }

        DeactivateOperatorPressedStatus();
    }

    public void OnClickEqualBtn()
    {
        if (ResultCalculated)
            return;

        else
        {
            if (InputField.text.Equals(""))
                InputField.text += OutputField.text;

            else if (IsOperator(InputField.text[InputField.text.Length - 1]))
            {
                if (OutputField.text[0].Equals('-'))
                    FormatInputStringWrtOperatorForEvaluation();
                else
                    InputField.text += OutputField.text;
            }

            else if (IsOpenBracket(InputField.text[InputField.text.Length - 1]))
            {
                if (OutputField.text[0] == '-')
                    InputField.text += "0";

                InputField.text += OutputField.text;
            }

            else { }    //close bracket ha...

            AddCloseBracketsIfMissing();

            string exp;
            if (InputField.text[0] == '-')
                exp = "0" + InputField.text;
            else
                exp = InputField.text;

            //exp += InputField.text; // Substring(0, InputField.text.Length);

            OutputField.text = evaluate(exp).ToString();

            InputField.text += " =";

            OpenBracketCurrentSize = -1;
            UpdateOpenBracketStatus(OpenBracketCurrentSize);

            ResetOpenBracketsIndexList();

            ResultCalculated = true;

            ActivateClearOutputStatus();

            DeactivateOperatorPressedStatus();
        }
    }

    public void operatorClick(string op)
    {
        if (ResultCalculated)
        {
            InputField.text = "";
            ResultCalculated = false;
        }
        
        if (!IsOperatorPressed)  //frst time gonna press
        {
            if (InputField.text.Equals(""))
                InputField.text += OutputField.text + op;

            else
            {
                //must do when press equal, remove all insignificant zeroes after decimal           
                if (OutputField.text.Contains("."))
                    OutputField.text = (double.Parse(OutputField.text)).ToString();
                
                if (IsCloseBracket(InputField.text[InputField.text.Length - 1]))
                    InputField.text += op;

                else if (IsOpenBracket(InputField.text[InputField.text.Length - 1]))
                {
                    
                    if (OutputField.text[0].Equals('-'))                //(-56    -> + ==> (0-56)+
                        InputField.text += "0" + OutputField.text + op;
                    else
                        InputField.text += OutputField.text + op;
                }

                else //operator ha..
                {
                    if (OutputField.text[0] == '-')
                    {
                        FormatInputStringWrtOperatorForEvaluation();
                        InputField.text += op;
                    }

                    else
                        InputField.text += OutputField.text + op;
                }
            }

            string exp;

            if (InputField.text[0] == '-')
                exp = "0";
            else
                exp = "";
            
            //bracket tk evaluate   (-56    +2

            if (OpenBracketIndexList.Count != 0)  //1+2+(3+4+    OBCS+1 = 5, (9)-(1(lastOP))-(OBSC+1) 
                exp += InputField.text.Substring(OpenBracketIndexList[(OpenBracketIndexList.Count) - 1] + 1, (InputField.text.Length) - (1) - (OpenBracketIndexList[(OpenBracketIndexList.Count) - 1] + 1));

            else
                exp += InputField.text.Substring(0, InputField.text.Length - 1);    //deduct last operator

            double result = evaluate(exp);
            OutputField.text = result.ToString();

            ActivateOperatorPressedStatus();
        }

        else
        {
            if (InputField.text.Length - 1 >= 0)    //empty nhi ha..
            {
                InputField.text = InputField.text.Insert(InputField.text.Length -1, op);

                InputField.text = InputField.text.Remove(InputField.text.Length - 1, 1);       //replace last index
            }
        }

        ActivateClearOutputStatus();

    }

    public void ClearClick()
    {
        if (!OutputField.text.Equals("0"))                 //agr ans 0 ha... to hmesha pehly, answer ko clear krna.. then uper field clear ho gi..
        {
            OutputField.text = "0";

            if (InputField.text != "" && IsCloseBracket(InputField.text[InputField.text.Length - 1]))
            {
                int IndexOfLastOpenBracket = InputField.text.LastIndexOf("(");

                if (IndexOfLastOpenBracket >= 0)
                    InputField.text = InputField.text.Remove(IndexOfLastOpenBracket);

            }
        }

        else if (InputField.text != "")
        {
            InputField.text = "";

            OpenBracketCurrentSize = -1;
            UpdateOpenBracketStatus(OpenBracketCurrentSize);

            ResetOpenBracketsIndexList();
        }

        ActivateClearOutputStatus();

        DeactivateOperatorPressedStatus();

        //ResultCalculated = false;
    }

    public void OnClickMemory(string op)
    {
        if (ResultCalculated)
        {
            InputField.text = "";
            ResultCalculated = false;
        }

        if (op == "MR")
        {
            if (MemoryStoredValue == "")
                return;
            else
                OutputField.text = MemoryStoredValue;
        }

        else if (op == "MS")
            MemoryStoredValue = OutputField.text;

        DeactivateOperatorPressedStatus();
        ActivateClearOutputStatus();
    }

    public void OpenBracketClick(string op)
    {
        if (ResultCalculated)
        {
            InputField.text = "";
            ResultCalculated = false;
        }

        if (!UpdateOpenBracketStatus(OpenBracketCurrentSize))
        {
            return;
        }

        else
        {
            if (InputField.text.Equals("") || IsOpenBracket(InputField.text[InputField.text.Length - 1]))
                InputField.text += op;

            else if (IsOperator(InputField.text[InputField.text.Length - 1]))
            {
                InputField.text += op;
                OutputField.text = "0";
            }

            else if (IsCloseBracket(InputField.text[InputField.text.Length - 1]))   //1+2+(4+5) => 1+2+( ===> ( index = 4 => 9-(4+1)
            {
                int IndexOfLastOpenBracket = InputField.text.LastIndexOf("(");

                InputField.text = InputField.text.Remove(IndexOfLastOpenBracket + 1);
            }

            OpenBracketIndexList.Add(InputField.text.Length - 1);
        }

        ActivateClearOutputStatus();

        DeactivateOperatorPressedStatus();
    }

    public void CloseBracketClick(string op)
    {
        if (OpenBracketCurrentSize <= 0)    //empty or ( nhi ha..
            return;

        else
        {
            char INPFLastChar = InputField.text[InputField.text.Length - 1];

            if (IsOperator(INPFLastChar))
            {
                if (OutputField.text[0] == '-')
                {
                    if (INPFLastChar == '-')
                    {
                        FormatInputStringWrtOperatorForEvaluation();
                        InputField.text += op;
                    }
                }

                else
                    InputField.text += OutputField.text + op;

            }

            else if (IsOpenBracket(INPFLastChar))
            {
                if (OutputField.text[0] == '-')
                    InputField.text = InputField.text + "0" + OutputField.text + op;

                else
                    InputField.text += OutputField.text + op;  
            }

            else    //is last close bracket?
                InputField.text += op;

            //1+2+(3+4+3)    11 total valS , strt index of (-> (4)- RSBI
            string exp = InputField.text.Substring(OpenBracketIndexList[(OpenBracketIndexList.Count) - 1], ((InputField.text.Length) - (OpenBracketIndexList[(OpenBracketIndexList.Count) - 1])));

            OutputField.text = evaluate(exp).ToString();

            ReduceOpenBracketIndexList();

            OpenBracketCurrentSize -= 2;
            UpdateOpenBracketStatus(OpenBracketCurrentSize);
            
            ActivateClearOutputStatus();

            DeactivateOperatorPressedStatus();
        }
    }

    public string GetOutputValue()
    {
        decimal Res = Convert.ToDecimal(OutputField.text);
        
        Res = Math.Round(Res, 4);

        return Res.ToString();

    }

    public void OnClickPlus_Minus()
    {
        if (OutputField.text != "")
            OutputField.text = (double.Parse(OutputField.text) * -1).ToString();

        DeactivateOperatorPressedStatus();
    }

    public void OnClickInverse()
    {
        if (OutputField.text != "")
            OutputField.text = (1 / double.Parse(OutputField.text)).ToString();

        DeactivateOperatorPressedStatus();
    }

    #region UtilityFunctions

    bool UpdateOpenBracketStatus(int size)      //6 ko 5 -> (6-2)++ -> 6
    {
        if (OpenBracketCurrentSize + 1 > MaxOpenBracketsSize)
            return false;

        if (++OpenBracketCurrentSize == 0)
            OpenBracketText.text = "";

        else
            OpenBracketText.text = "<sub>" + OpenBracketCurrentSize + "</sub>";

        return true;
    }

    bool IsOperator(char op)
    {
        return (op == '+' || op == '-' || op == '*' || op == '/');
    }

    bool IsOpenBracket(char Ch)
    {
        return (Ch == '(');
    }

    bool IsCloseBracket(char Ch)
    {
        return (Ch == ')');
    }

    bool IsNumber(char Ch)
    {
        return (Ch >= '0' && Ch <= '9');
    }

    double evaluate(string expression)
    {
        char[] tokens = expression.ToCharArray();

        Stack<double> SVs = new Stack<double>();
        Stack<char> SOps = new Stack<char>();

        int i = 0;

        while (i < tokens.Length)
        {
            if (tokens[i] == ' ')
                continue;

            if (IsNumber(tokens[i]))
            {
                StringBuilder sbuf = new StringBuilder();

                // There may be more than one digits in number and may b point (decimal number) 
                while (i < tokens.Length && (IsNumber(tokens[i]) || tokens[i] == '.'))
                {
                    sbuf.Append(tokens[i]);
                    i++;
                }

                SVs.Push(double.Parse(sbuf.ToString()));
            }

            else if (IsOpenBracket(tokens[i]))
            {
                SOps.Push(tokens[i]);
                i++;
            }

            // Closing brace encountered, solve entire brace  
            else if (IsCloseBracket(tokens[i]))
            {
                while (SOps.Peek() != '(')
                {
                    SVs.Push(applyOp(SOps.Pop(), SVs.Pop(), SVs.Pop()));
                }
                SOps.Pop();
                i++;
            }

            // Current token is an operator.  
            else if (IsOperator(tokens[i]))
            {
                while (SOps.Count > 0 && hasPrecedence(tokens[i], SOps.Peek()))
                {
                    SVs.Push(applyOp(SOps.Pop(), SVs.Pop(), SVs.Pop()));
                }
                
                SOps.Push(tokens[i]);
                i++;
            }
        }

        // Entire expression has been parsed at this point, apply remaining  
        // ops to remaining values  
        while (SOps.Count > 0)
        {
            SVs.Push(applyOp(SOps.Pop(), SVs.Pop(), SVs.Pop()));
        }

        // Top of 'values' contains result, return it  
        return SVs.Pop();
    }

    bool hasPrecedence(char op1, char op2)
    {
        if (op2 == '(' || op2 == ')')
            return false;

        if ((op1 == '*' || op1 == '/') && (op2 == '+' || op2 == '-'))
            return false;

        else
            return true;
    }

    double applyOp(char op, double b, double a)
    {
        switch (op)
        {
            case '+':
                return a + b;
            case '-':
                return a - b;
            case '*':
                return a * b;
            case '/':
                if (b == 0)
                {
                    throw new System.NotSupportedException("Cannot divide by zero");
                }
                return a / b;
        }
        return 0;
    }


    void ActivateOperatorPressedStatus()
    {
        if (!IsOperatorPressed)
            IsOperatorPressed = true;
    }

    void DeactivateOperatorPressedStatus()
    {
        if (IsOperatorPressed)
            IsOperatorPressed = false;
    }

    void ActivateClearOutputStatus()
    {
        if (!ClearOutput)
            ClearOutput = true;
    }

    void DeactivateClearOutputStatus()
    {
        if (ClearOutput)
            ClearOutput = false;
    }

    void AddCloseBracketsIfMissing()
    {
        if (InputField.text.Contains("(") && (OpenBracketCurrentSize > 0))
        {
            for (int q = 1; q <= OpenBracketCurrentSize; q++)
                InputField.text += ")";
        }
    }

    void FormatInputStringWrtOperatorForEvaluation()
    {
        if (InputField.text[InputField.text.Length - 1] == '-')
        {
            InputField.text = InputField.text.Remove(InputField.text.Length - 1, 1);

            InputField.text = InputField.text.Insert(InputField.text.Length - 1, "+");

            InputField.text += OutputField.text.Remove(OutputField.text[0], 1);
        }

        else if (InputField.text[InputField.text.Length - 1] == '+')
        {
            InputField.text = InputField.text.Remove(InputField.text.Length - 1, 1);
            InputField.text += OutputField.text;
        }

        else if (InputField.text[InputField.text.Length - 1] == '*' || InputField.text[InputField.text.Length - 1] == '/')
        {
            //InputField.text = InputField.text.Remove(InputField.text.Length - 1, 1);
            InputField.text = InputField.text + "(0" + OutputField.text + ")";
        }
    }

    void ResetOpenBracketsIndexList()
    {
        if (OpenBracketIndexList.Count != 0)
            OpenBracketIndexList.RemoveRange(0, OpenBracketIndexList.Count);
    }

    void ReduceOpenBracketIndexList()
    {
        if (OpenBracketIndexList.Count > 0)
            OpenBracketIndexList.RemoveAt((OpenBracketIndexList.Count) - 1);
    }

    #endregion UtilityFunctions
}