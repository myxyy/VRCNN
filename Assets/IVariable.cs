﻿
using UdonSharp;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class IVariable : UdonSharpBehaviour
{
    public virtual RenderTexture Data(){ return null; }
    public virtual RenderTexture Grad(){ return null; }
    private bool _isTextureReady;
    protected bool SetIsTextureReady() => _isTextureReady = true;
    public bool IsTextureReady() => _isTextureReady;
    public virtual void Forward(){
        foreach (var output in _outputList)
        {
            output.Forward();
        }
    }
    public virtual void Backward()
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

    public bool IsNoBackward()
    {
        foreach (var output in _outputList)
        {
            if (!output.IsNoBackward())
            {
                return false;
            }
        }
        return true;
    }

    public void Reset()
    {
        if (_input != null)
        {
            _input.Reset();
        }
        foreach (var output in _outputList)
        {
            output.Reset(); 
        }
    }
 
    private IFunction _input = null;
    public void SetInput(IFunction input)
    {
        if (_input == null)
        {
            _input = input;
        }
        else
        {
            Debug.LogError($"IVariable must not have more than one input: {_input.name} {input.name}");
        }
    }
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

