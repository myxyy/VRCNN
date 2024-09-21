using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class CrossEntropy : IFunction 
{
    [SerializeField]
    private IVariable _input;
    [SerializeField]
    private IVariable _label;
    [SerializeField]
    private IVariable _output;
    [SerializeField]
    private GameObject _crossEntropyPrefab;
    private Material _crossEntropyMaterial;

    private void Start()
    {
        var crossEntropyObject = Instantiate(_crossEntropyPrefab);
        _crossEntropyMaterial = crossEntropyObject.GetComponent<MeshRenderer>().material;
        Destroy(crossEntropyObject);

        __inputList = new IVariable[1]{_input};
        __output = _output;
        InitVariables();
    }

    public override void Forward()
    {
        base.Forward();
        if (IsForwardReady())
        {
            _crossEntropyMaterial.SetTexture("_X", _input.Data());
            _crossEntropyMaterial.SetTexture("_Y", _label.Data());
            VRCGraphics.Blit(null, _output.Data(), _crossEntropyMaterial, 0);
            FireOutputForward();
        }
    }

    public override void Backward()
    {
        base.Backward();
        Debug.Log($"CETest0:{name}");
        if (IsBackwardReady())
        {
            Debug.Log($"CETest1:{name}");
            if (_input.Grad() != null)
            {
                _crossEntropyMaterial.SetTexture("_X", _input.Data());
                _crossEntropyMaterial.SetTexture("_Y", _label.Data());
                _crossEntropyMaterial.SetTexture("_OutGrad", _output.Grad());
                VRCGraphics.Blit(null, _input.Grad(), _crossEntropyMaterial, 1);
            }
            FireInputBackward();
        }
    }
}


