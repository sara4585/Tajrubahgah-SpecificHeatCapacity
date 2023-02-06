using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tajurbah_Gah
{
    public class calc : MonoBehaviour
    {

        [SerializeField] Text output;
        [SerializeField] Text tempOutput;
        [SerializeField] Text outputLastOp;
        private string lastOp;
        private float leftOperand;
        private bool clearOutput;
        private bool onlyOp;

        // Use this for initialization
        void Start()
        {
            clearOutput = true;
            onlyOp = false;
            lastOp = "";
            leftOperand = 0;
            outputLastOp.text = lastOp;
            output.text = "";
        }

        // Update is called once per frame
        void Update()
        {

        }

        public string GetOutputValue()
        {
            return output.text;
        }

        public void digitClick(string digit)
        {
            if (onlyOp || (digit == "." && output.text.Contains(".")))
            {
                return;
            }
            if (clearOutput)
            {
                if (digit != "<<<")
                {
                    output.text = digit.ToString();
                    clearOutput = false;
                }
            }
            else
            {
                if (digit == "<<<")
                {
                    output.text = output.text.Remove(output.text.Length - 1, 1);
                }
                else
                {
                    output.text += digit.ToString();
                }
            }
        }

        public void opClick(string op)
        {
            if (lastOp != "")
            {
                // check to see whether the user provided any operands after the last operation
                if (clearOutput)
                {
                    // if not, do nothing
                    return;
                }
                // evaluate last operation, using leftOperand and current value of output
                // store result of last operation in leftOperand
                leftOperand = evaluate(leftOperand, float.Parse(output.text), lastOp);

                // update temp output with the result
                tempOutput.text = leftOperand.ToString();
            }
            else
            {
                leftOperand = float.Parse(output.text);
                // show it in the temp output
                tempOutput.text = leftOperand.ToString();
            }
            if (op != "=")
            {
                // set lastOp as op
                lastOp = op;
                output.text = "";
            }
            else
            {
                // clear the last operation
                lastOp = "";
                // move the temp value to the final output display
                output.text = tempOutput.text;
                tempOutput.text = "";
            }
            clearOutput = true;
            onlyOp = false;
            outputLastOp.text = lastOp;
        }

        public void opClickUnary(string op)
        {
            if (output.text != "")
            {
                output.text = evaluateUnary(float.Parse(output.text), op).ToString();
                //clearOutput = true;
                onlyOp = true;
            }
        }

        public void resetCalc()
        {
            Start();
        }

        private float evaluateUnary(float operand, string op)
        {
            if (op == "1/x")
            {
                if (operand != 0)
                {
                    return (1 / operand);
                }
            }
            else if (op == "x^2")
            {
                return (operand * operand);
            }
            else if (op == "sqrt")
            {
                return Mathf.Sqrt(operand);
            }
            else if (op == "sin")
            {
                return Mathf.Sin(Mathf.Deg2Rad * operand);
            }
            else if (op == "cos")
            {
                return Mathf.Cos(Mathf.Deg2Rad * operand);
            }
            else if (op == "tan")
            {
                return Mathf.Tan(Mathf.Deg2Rad * operand);
            }
            // failsafe is do-nothing:
            return operand;
        }

        private float evaluate(float left, float right, string op)
        {

            if (op == "+")
            {
                return left + right;
            }
            else if (op == "-")
            {
                return left - right;
            }
            else if (op == "*")
            {
                return left * right;
            }
            else if (op == "/")
            {
                if (right != 0)
                {
                    return left / right;
                }
                else
                {
                    return left;
                }
            }

            return 0.0f;
        }
    }
}