
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class IFunction : UdonSharpBehaviour
{
    public virtual void Forward(){}
    protected bool _isForwardComplete = false;
    public virtual bool IsForwardComplete() => _isForwardComplete;
    public virtual void Backward(){}
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
    protected IVariable __output;
    protected void InitVariables()
    {
        foreach (var input in __inputList)
        {
            input.AddOutput(this);
        }
        __output.SetInput(this); 
    }
    protected void FireOutputForward() => __output.Forward();
}
