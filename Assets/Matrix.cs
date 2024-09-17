
using System.Collections.Generic;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Matrix : IVariable
{
    [SerializeField]
    private int _row = 128;
    [SerializeField]
    private int _column = 128;
    [SerializeField]
    private Display _dataDisplay;
    [SerializeField]
    private Display _gradDisplay;

    private RenderTexture _data;
    public override RenderTexture Data() => _data;
    private RenderTexture _grad;
    public override RenderTexture Grad() => _grad;
    private void Start()
    {
        _data = new RenderTexture(_row, _column, 0, RenderTextureFormat.RFloat, 0);
        _grad = new RenderTexture(_row, _column, 0, RenderTextureFormat.RFloat, 0);
        if (_dataDisplay)
        {
            Debug.Log($"{gameObject.name} {_data == null}");
            _dataDisplay.SetTexture(_data);
        }
        if (_gradDisplay)
        {
            _gradDisplay.SetTexture(_grad);
        }
        _isTextureReady = true;
    }
}
