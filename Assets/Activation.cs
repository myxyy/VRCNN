using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Activation : IFunction 
{
    [SerializeField]
    private IVariable _input;
    [SerializeField]
    private IVariable _output;
    [SerializeField]
    private GameObject _activationPrefab;
    private Material _activationMaterial;

    private void Start()
    {
        var activationObject = Instantiate(_activationPrefab);
        _activationMaterial = activationObject.GetComponent<MeshRenderer>().material;
        Destroy(activationObject);

        __inputList = new IVariable[1]{_input};
        __output = _output;
        InitVariables();
    }

    public override void Forward()
    {
        base.Forward();
        if (IsForwardReady())
        {
            if (_activationMaterial)
            {
                VRCGraphics.Blit(_input.Data(), _output.Data(), _activationMaterial, 0);
            }
            else
            {
                VRCGraphics.Blit(_input.Data(), _output.Data());
            }
            FireOutputForward();
        }
    }

    public override void Backward()
    {
        base.Backward();
        if (IsBackwardReady())
        {
            if (_input.Grad() != null)
            {
                if (_activationMaterial)
                {
                    _activationMaterial.SetTexture("_Grad", _output.Grad());
                    VRCGraphics.Blit(_input.Data(), _input.Grad(), _activationMaterial, 1);
                }
                else
                {
                    VRCGraphics.Blit(_input.Data(), _input.Grad());
                }
            }
            FireInputBackward();
        }
    }
}

