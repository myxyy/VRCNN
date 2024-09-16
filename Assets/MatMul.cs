
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class MatMul : IFunction 
{
    [SerializeField]
    private IVariable _inputA;
    [SerializeField]
    private IVariable _inputB;
    [SerializeField]
    private IVariable _output;
    [SerializeField]
    private GameObject _matmulPrefab;
    private Material _matmulMaterial;
    [SerializeField]
    private int _numAccumulatePerFrame = 16;
    private bool _isExecute = false;

    private int _currentMInd = 0;

    private void Start()
    {
        var matmulObject = Instantiate(_matmulPrefab);
        _matmulMaterial = matmulObject.GetComponent<MeshRenderer>().material;
        Destroy(matmulObject);

        __inputList = new IVariable[2]{_inputA, _inputB};
        __output = _output;
        InitVariables();
    }

    private void Update()
    {
        if (!_isExecute)
        {
            return;
        }

        var matA = _inputA.Data();
        var matB = _inputB.Data();
        var matC = _output.Data();

        if (
            matA.height != matC.height ||
            matB.width != matC.width ||
            matA.width != matB.height
        )
        {
            Debug.LogError("Matrix size missmatch");
        }

        var m = matA.height;
        var mIndMaxCurrentFrame = Mathf.Min(_currentMInd + _numAccumulatePerFrame, m);
        var matCTemp = new RenderTexture(matC.width, matC.height, 0, RenderTextureFormat.RFloat, 0);
        _matmulMaterial.SetTexture("_MatA", matA);
        _matmulMaterial.SetTexture("_MatB", matB);
        for (; _currentMInd < mIndMaxCurrentFrame; _currentMInd++)
        {
            _matmulMaterial.SetInt("_MInd", _currentMInd);
            VRCGraphics.Blit(matC, matCTemp, _matmulMaterial);
            VRCGraphics.Blit(matCTemp, matC);
        }
        if (_currentMInd == m)
        {
            _isExecute = false;
            _isForwardComplete = true;
            FireOutputForward();
        }
    }

    public override void Forward()
    {
        if (!IsForwardReady())
        {
            return;
        }
        _currentMInd = 0;
        _isExecute = true;
        _isForwardComplete = false;
    }
}
