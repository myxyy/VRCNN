
using System.Runtime.InteropServices;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ForwardTest : UdonSharpBehaviour
{
    [SerializeField]
    private IVariable[] _variableList;
    [SerializeField]
    private IVariable _forwardTrigger;
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

    private bool _isTextureInit = false;
    private float _seed1 = 0.625425f;
    private float _seed2 = 12341.622425f;
    private float _seed3 = 0.1624425f;
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
        var isTextureReady = true;
        foreach (var variable in _variableList)
        {
            isTextureReady = isTextureReady && variable.IsTextureReady();
        }

        if (!_isTextureInit && isTextureReady)
        {
            foreach (var variable in _variableList)
            {
                variable.Initialize();
                variable.ZeroGrad();
            }
            _isTextureInit = true;

            var indexList = new int[64];
            for (int i = 0; i<64; i++)
            {
                indexList[i] = _permutedIndexList[i];
            }
            _batchX.FetchData(indexList);
            _batchY.FetchData(indexList);
        }

        if (_forward)
        {
            foreach (var variable in _variableList)
            {
                variable.Initialize();
                variable.ZeroGrad();
            }
 
            _forwardTrigger.Forward();
            _forward = false;
        }
    }
}
