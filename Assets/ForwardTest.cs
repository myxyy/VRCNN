
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ForwardTest : UdonSharpBehaviour 
{
    [SerializeField]
    private IVariable[] _parameterList;
    [SerializeField]
    private IVariable[] _variableList;
    [SerializeField]
    private IVariable[] _completeBackwardVariableList;
    [SerializeField]
    private Material _init; 
    [SerializeField]
    private bool _forward;
    [SerializeField]
    private int _size = 70000;
    private int[] _permutedIndexList;
    [SerializeField]
    private GetMNISTBatchX _batchX;
    [SerializeField]
    private GetMNISTBatchY _batchY;
    [SerializeField]
    private bool _optimize = false;
    [SerializeField]
    private float _learningRate = 0.1f;
    [SerializeField]
    private Material _addMaterial;

    private bool _isParameterInit = false;
    private float _seed1 = 0.625425f;
    private float _seed2 = 12341.622425f;
    private float _seed3 = 0.1624425f;
    private const int BATCH_SIZE = 64;
    private const int DATASET_SIZE = 70000;
    private int _currentIndex = 0;
    private void Start()
    {
        _permutedIndexList = new int[_size];
        for (int i = 0; i < _size; i++)
        {
            _permutedIndexList[i] = i;
        }
        for (int i = 0; i < _size; i++)
        {
            var swapInd = Mathf.FloorToInt(_seed1 * (_size - i)) + i;
            var temp = _permutedIndexList[i];
            _permutedIndexList[i] = _permutedIndexList[swapInd];
            _permutedIndexList[swapInd] = temp;
            _seed1 = _seed1 * _seed2 + _seed3;
            _seed1 -= Mathf.FloorToInt(_seed1);
        }
    }

    private void Update()
    {
        var isParameterReady = true;
        foreach (var parameter in _parameterList)
        {
            isParameterReady = isParameterReady && parameter.IsTextureReady();
        }
        var isVariableReady = true;
        foreach (var variable in _variableList)
        {
            isVariableReady = isVariableReady && variable.IsTextureReady();
        }

        if (!_isParameterInit && isParameterReady)
        {
            foreach (var parameter in _parameterList)
            {
                parameter.Initialize();
            }
            _isParameterInit = true;
        }

        if (_forward)
        {
            if (isParameterReady && isVariableReady)
            {
                StartForward();
                _forward = false;
            }
        }

        if (_optimize)
        {
            Optimize();
            _optimize = false;
        }
    }

    public void Next()
    {
        Optimize();
        StartForward();
    }

    public void StartForward()
    {
        var indexList = new int[BATCH_SIZE];
        for (int i = 0; i<indexList.Length; i++)
        {
            indexList[i] = _permutedIndexList[_currentIndex];
            _currentIndex = (_currentIndex + 1) % DATASET_SIZE;
        }
        _batchX.FetchData(indexList);
        _batchY.FetchData(indexList);

        _batchX.Reset();

        _batchX.Forward();
        _batchY.Forward();
    }

    private void Optimize()
    {
        foreach (var parameter in _parameterList)
        {
            _addMaterial.SetTexture("_MatA", parameter.Data());
            _addMaterial.SetTexture("_MatB", parameter.Grad());
            _addMaterial.SetFloat("_CoefB", -_learningRate);
            var tempData = new RenderTexture(parameter.Data().width, parameter.Data().height, 0, RenderTextureFormat.RFloat);
            tempData.filterMode = FilterMode.Point;
            VRCGraphics.Blit(null, tempData, _addMaterial);
            VRCGraphics.Blit(tempData, parameter.Data());
        }
    }
}
