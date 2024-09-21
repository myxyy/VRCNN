using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC.SDKBase;

public class AddBias : IFunction 
{
    [SerializeField]
    private IVariable _input;
    [SerializeField]
    private IVariable _bias;
    [SerializeField]
    private IVariable _output;
    [SerializeField]
    private GameObject _addBiasPrefab;
    private Material _addBiasMaterial;
    [SerializeField]
    private int _numAccumulatePerFrame = 16;

    private int _currentMInd = 0;

    private int _mode = SLEEP;
    private const int SLEEP = 0;
    private const int AccBiasGrad = 1;
    private bool _isRequestAccBiasGrad = false;

    private void Start()
    {
        var addBiasObject = Instantiate(_addBiasPrefab);
        _addBiasMaterial = addBiasObject.GetComponent<MeshRenderer>().material;
        Destroy(addBiasObject);

        __inputList = new IVariable[2]{_input, _bias};
        __output = _output;
        InitVariables();
    }

    private void Update()
    {
        if (_mode == SLEEP)
        {
            if (_isRequestAccBiasGrad)
            {
                _isRequestAccBiasGrad = false;
                _mode = AccBiasGrad;
            }
            else
            {
                return;
            }
        }

        if (
            _input.Data().width != _output.Data().width ||
            _input.Data().height != _output.Data().height ||
            _bias.Data().width != 1 ||
            _input.Data().height!= _bias.Data().height
        )
        {
            Debug.LogError("AddBias: shape missmatch");
        }

        var mIndMaxCurrentFrame = Mathf.Min(_currentMInd + _numAccumulatePerFrame, _output.Grad().width);
        var biasGradTemp = new RenderTexture(_bias.Grad().width, _bias.Grad().height, 0, RenderTextureFormat.RFloat, 0);
        biasGradTemp.filterMode = FilterMode.Point;
        _addBiasMaterial.SetTexture("_OutGrad", _output.Grad());
        for (; _currentMInd < mIndMaxCurrentFrame; _currentMInd++)
        {
            _addBiasMaterial.SetInt("_MInd", _currentMInd);
            VRCGraphics.Blit(_bias.Grad(), biasGradTemp, _addBiasMaterial, 1);
            VRCGraphics.Blit(biasGradTemp, _bias.Grad());
        }

        if (_currentMInd == _output.Grad().width)
        {
            FireInputBackward();
            _mode = SLEEP;
        }
    }
 


    public override void Forward()
    {
        base.Forward();
        if (IsForwardReady())
        {
            _addBiasMaterial.SetTexture("_Input", _input.Data());
            VRCGraphics.Blit(_bias.Data(), _output.Data(), _addBiasMaterial, 0);
            FireOutputForward();
        }
    }

    public override void Backward()
    {
        base.Backward();
        if (IsBackwardReady())
        {
            VRCGraphics.Blit(_output.Grad(), _input.Grad());
            _isRequestAccBiasGrad = true;
        }
    }
}
