
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SetDataToMaterial : UdonSharpBehaviour
{
    [SerializeField]
    private IVariable _variable;
    [SerializeField]
    private Material _material;
    private bool _isInit = false;

    private void Update()
    {
        if (!_isInit && _variable.IsTextureReady())
        {
            _material.SetTexture("_MainTex", _variable.Data());
            _isInit = true;
        }
    }
}
