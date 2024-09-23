
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class GetDrawing : IVariable
{
    [SerializeField]
    private Material _flattenMaterial;
    [SerializeField]
    private RenderTexture _drawing;
    [SerializeField]
    private int _updateFrameInterval = 60;
    private int _updateFrameCount = 0;

    private RenderTexture _data;
    public override RenderTexture Data() => _data;

    private void Start()
    {
        _data = new RenderTexture(1, _drawing.width * _drawing.height, 0, RenderTextureFormat.RFloat);
        _data.filterMode = FilterMode.Point;
        _isTextureReady = true;
    }


    private void Update()
    {
        if (_updateFrameCount < _updateFrameInterval)
        {
            _updateFrameCount++;
        }
        else
        {
            Forward();
            _updateFrameCount = 0;
        }
    }

    public override void Forward()
    {
        VRCGraphics.Blit(_drawing, _data, _flattenMaterial);
        base.Forward();
    }
}
