
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class CopyData : IVariable 
{
    [SerializeField]
    private IVariable _target;

    public override RenderTexture Data() => _target.Data();

    private void Update()
    {
        if (!_isTextureReady && _target.IsTextureReady())
        {
            _isTextureReady = true;
        }
    }
}
