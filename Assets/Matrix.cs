
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
    private Material _init;
    public override void Initialize() => VRCGraphics.Blit(null, _data, _init);

    private RenderTexture _data;
    public override RenderTexture Data() => _data;
    private RenderTexture _grad;
    public override RenderTexture Grad() => _grad;
    private void Start()
    {
        _data = new RenderTexture(_row, _column, 0, RenderTextureFormat.RFloat, 0);
        _data.filterMode = FilterMode.Point;
        _grad = new RenderTexture(_row, _column, 0, RenderTextureFormat.RFloat, 0);
        _grad.filterMode = FilterMode.Point;
        _isTextureReady = true;
    }

    public override void ZeroGrad()
    {
        _grad = new RenderTexture(_row, _column, 0, RenderTextureFormat.RFloat, 0);
    }
}
