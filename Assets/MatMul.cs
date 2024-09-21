
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

    private int _currentMInd = 0;

    private int _mode = SLEEP;
    private const int SLEEP = 0;
    private const int FORWARD = 1;
    private const int BACKWARD_A = 2;
    private const int BACKWARD_B = 3;
    private bool _isRequestBackward = false;

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
        if (_mode == SLEEP)
        {
            if (_isRequestBackward)
            {
                _isRequestBackward = false;
                _mode = BACKWARD_A;
            }
            else
            {
                return;
            }
        }

        RenderTexture matA = null;
        RenderTexture matB = null;
        RenderTexture matC = null;
        switch (_mode)
        {
            case FORWARD:
            {
                matA = _inputA.Data();
                matB = _inputB.Data();
                matC = _output.Data();
                break;
            }
            case BACKWARD_A:
            {
                if (_inputA.Grad() == null)
                {
                    _mode = BACKWARD_B;
                    return;
                }
                matA = _output.Grad();
                matB = _inputB.Data(); // Transpose
                matC = _inputA.Grad();
                break;
            }
            case BACKWARD_B:
            {
                if (_inputB.Grad() == null)
                {
                    _mode = SLEEP;
                    return;
                }
                matA = _inputA.Data(); // Transpose
                matB = _output.Grad();
                matC = _inputB.Grad();
                break;
            }
            default:
            {
                break;
            }
        }

        var rowA = _mode == BACKWARD_B ? matA.height : matA.width;
        var colA = _mode == BACKWARD_B ? matA.width : matA.height;
        var rowB = _mode == BACKWARD_A ? matB.height : matB.width;
        var colB = _mode == BACKWARD_A ? matB.width : matB.height;
        var rowC = matC.width;
        var colC = matC.height;

        if (
            rowA != rowC ||
            colB != colC ||
            colA != rowB
        )
        {
            Debug.LogError("Matrix size missmatch");
        }

        var m = colA;
        var mIndMaxCurrentFrame = Mathf.Min(_currentMInd + _numAccumulatePerFrame, m);
        var matCTemp = new RenderTexture(matC.width, matC.height, 0, RenderTextureFormat.RFloat, 0);
        matCTemp.filterMode = FilterMode.Point;
        _matmulMaterial.SetTexture("_MatA", matA);
        _matmulMaterial.SetTexture("_MatB", matB);
        _matmulMaterial.SetFloat("_TransposeA", _mode == BACKWARD_B ? 1 : 0);
        _matmulMaterial.SetFloat("_TransposeB", _mode == BACKWARD_A ? 1 : 0);
        for (; _currentMInd < mIndMaxCurrentFrame; _currentMInd++)
        {
            _matmulMaterial.SetInt("_MInd", _currentMInd);
            VRCGraphics.Blit(matC, matCTemp, _matmulMaterial);
            VRCGraphics.Blit(matCTemp, matC);
        }
        if (_currentMInd == m)
        {
            switch (_mode)
            {
                case FORWARD:
                {
                    FireOutputForward();
                    _mode = SLEEP;
                    break;
                }
                case BACKWARD_A:
                {
                    _currentMInd = 0;
                    _mode = BACKWARD_B;
                    break;
                }
                case BACKWARD_B:
                {
                    FireInputBackward();
                    _mode = SLEEP;
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
    }

    public override void Forward()
    {
        base.Forward();
        if (IsForwardReady())
        {
            _currentMInd = 0;
            _mode = FORWARD;
        }
    }

    public override void Backward()
    {
        base.Backward();
        if (IsBackwardReady())
        {
            _currentMInd = 0;
            _isRequestBackward = true;
        }
    }
}
