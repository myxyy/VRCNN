
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class IFunction : UdonSharpBehaviour
{
    public virtual void Forward(){}
    protected bool _isForwardComplete = false;
    public bool IsForwardComplete() => _isForwardComplete;
    public virtual void Backward(){}
    protected bool _isBackwardComplete = false;
    public bool IsBackwardComplete() => _isBackwardComplete;
    protected IVariable[] __inputList;
    protected bool IsForwardReady()
    {
        foreach (var input in __inputList)
        {
            if (!input.IsForwardReady())
            {
                return false;
            }
        }
        return true;
    }
    protected bool IsBackwardReady() => __output.IsBackwardReady();
    protected IVariable __output;
    protected void InitVariables()
    {
        foreach (var input in __inputList)
        {
            input.AddOutput(this);
        }
        if (__output != null)
        {
            __output.SetInput(this); 
        }
    }
    protected void FireOutputForward() => __output.Forward();
    protected void FireInputBackward()
    {
        foreach (var input in __inputList)
        {
            input.Backward();
        }
    }
}
