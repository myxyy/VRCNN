
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

    private bool _isTextureInit = false;
    private void Start()
    {
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
