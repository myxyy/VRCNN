
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class IFunction : UdonSharpBehaviour
{
    public virtual void Forward() => _isForwardComplete = false;
    private bool _isForwardComplete = false;
    public bool IsForwardComplete() => _isForwardComplete;
    public virtual void Backward() => _isBackwardComplete = false;
    private bool _isBackwardComplete = false;
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
    protected void FireOutputForward()
    {
        _isForwardComplete = true;
        __output.Forward();
    }
    protected void FireInputBackward()
    {
        _isBackwardComplete = true;
        foreach (var input in __inputList)
        {
            input.Backward();
        }
    }
}
