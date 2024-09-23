
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ShowPredictAnswer : IFunction
{
    [SerializeField]
    private IVariable _predict;
    [SerializeField]
    private IVariable _answer;
    [SerializeField]
    private Material _material;
    private RenderTexture _predictTex;
    private RenderTexture _answerTex;

    private void Start()
    {
        _predictTex = new RenderTexture(64, 10, 0, RenderTextureFormat.RFloat, 0);
        _predictTex.filterMode = FilterMode.Point;
        _answerTex = new RenderTexture(64, 10, 0, RenderTextureFormat.RFloat, 0);
        _answerTex.filterMode = FilterMode.Point;
        _material.SetTexture("_MainTex", _predictTex);
        _material.SetTexture("_LabelTex", _answerTex);

        __inputList = new IVariable[2]{_predict, _answer};
        InitVariables();
    }

    public override void Forward()
    {
        base.Forward();
        if (IsForwardReady())
        {
            VRCGraphics.Blit(_predict.Data(), _predictTex);
            VRCGraphics.Blit(_answer.Data(), _answerTex);
        }
    }
}
