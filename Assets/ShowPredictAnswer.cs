
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ShowPredictAnswer : UdonSharpBehaviour
{
    [SerializeField]
    private IVariable _predict;
    [SerializeField]
    private IVariable _answer;
    [SerializeField]
    private Material _material;
    private bool _isInit = false;

    private void Update()
    {
        if (!_isInit && _predict.IsTextureReady() && _answer.IsTextureReady())
        {
            _material.SetTexture("_MainTex", _predict.Data());
            _material.SetTexture("_LabelTex", _answer.Data());
            _isInit = true;
        }
    }
}
