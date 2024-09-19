
using UdonSharp;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class IVariable : UdonSharpBehaviour
{
    public virtual RenderTexture Data(){ return null; }
    public virtual RenderTexture Grad(){ return null; }
    protected bool _isTextureReady;
    public bool IsTextureReady() => _isTextureReady;
    public void Forward(){
        foreach (var output in _outputList)
        {
            output.Forward();
        }
    }
    public void Backward()
    {
        if (IsBackwardReady() && _input != null)
        {
            _input.Backward();
        }
    }
    public bool IsBackwardReady()
    {
        foreach (var output in _outputList)
        {
            if (!output.IsBackwardComplete())
            {
                return false;
            }
        }
        return true;
    }
 
    private IFunction _input;
    public void SetInput(IFunction input) => _input = input;
    public bool IsForwardReady() => _input ? _input.IsForwardComplete() : true;
    private IFunction[] _outputList = new IFunction[0];
    public void AddOutput(IFunction output)
    {
        var newOutputList = new IFunction[_outputList.Length+1];
        for (int i = 0; i < _outputList.Length; i++)
        {
            newOutputList[i] = _outputList[i];
        }
        newOutputList[_outputList.Length] = output;
        _outputList = newOutputList;
    }

    public virtual void ZeroGrad(){}
    public virtual void Initialize(){}
}

