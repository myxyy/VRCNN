
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Add : IFunction 
{
    [SerializeField]
    private IVariable _inputA;
    [SerializeField]
    private IVariable _inputB;
    [SerializeField]
    private IVariable _output;
    [SerializeField]
    private GameObject _addPrefab;
    private Material _addMaterial;

    private void Start()
    {
        var addObject = Instantiate(_addPrefab);
        _addMaterial = addObject.GetComponent<MeshRenderer>().material;
        Destroy(addObject);

        __inputList = new IVariable[2]{_inputA, _inputB};
        __output = _output;
        InitVariables();
    }

    public override void Forward()
    {
        base.Forward();
        if (IsForwardReady())
        {
            _addMaterial.SetTexture("_MatA", _inputA.Data());
            _addMaterial.SetTexture("_MatB", _inputB.Data());
            VRCGraphics.Blit(null, _output.Data(), _addMaterial);
            FireOutputForward();
        }
    }

    public override void Backward()
    {
        base.Backward();
        if (IsBackwardReady())
        {
            if (_inputA.Grad() != null)
            {
                VRCGraphics.Blit(_output.Grad(), _inputA.Grad());
            }
            if (_inputB.Grad() != null)
            {
                VRCGraphics.Blit(_output.Grad(), _inputB.Grad());
            }
            FireInputBackward();
        }
    }

}
