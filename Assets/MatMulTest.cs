
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class MatMulTest : UdonSharpBehaviour
{
    [SerializeField]
    private Display _displayMatA;
    [SerializeField]
    private Display _displayMatB;
    [SerializeField]
    private Display _displayMatC;
    [SerializeField]
    private MatMul _matMul;

    private RenderTexture _matA;
    private RenderTexture _matB;
    private RenderTexture _matC;

    [SerializeField]
    private Material _initA;
    [SerializeField]
    private Material _initB;

    [SerializeField]
    private bool _isInitialize = false;

    void Start()
    {
    }

    private void Update()
    {
        if (_isInitialize)
        {
            _matA = new RenderTexture(1024, 1024, 0, RenderTextureFormat.RFloat, 0);
            _matB = new RenderTexture(1024, 1024, 0, RenderTextureFormat.RFloat, 0);
            _matC = new RenderTexture(1024, 1024, 0, RenderTextureFormat.RFloat, 0);
            _matA.filterMode = FilterMode.Point;
            _matB.filterMode = FilterMode.Point;
            _matC.filterMode = FilterMode.Point;
            _displayMatA.SetTexture(_matA);
            _displayMatB.SetTexture(_matB);
            _displayMatC.SetTexture(_matC);
            _isInitialize = false;
            //_randomMaterial.SetFloat("_Seed", Random.Range(0f,1f));
            VRCGraphics.Blit(null, _matA, _initA);
            //_randomMaterial.SetFloat("_Seed", Random.Range(0f,1f));
            VRCGraphics.Blit(null, _matB, _initB);
            _matMul.MatA = _matA;
            _matMul.MatB = _matB;
            _matMul.MatC = _matC;
            _matMul.Execute();
        }
    }
}
