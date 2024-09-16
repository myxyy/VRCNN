
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class MatMul : UdonSharpBehaviour
{
    public RenderTexture MatA;
    public RenderTexture MatB;
    public RenderTexture MatC;
    [SerializeField]
    private GameObject _matmulPrefab;
    private Material _matmulMaterial;
    [SerializeField]
    private int _numAccumulatePerFrame = 16;
    private bool _isExecute = false;

    private int _currentMInd = 0;

    private void Start()
    {
        var matmulObject = Instantiate(_matmulPrefab);
        _matmulMaterial = matmulObject.GetComponent<MeshRenderer>().material;
        Destroy(matmulObject);
    }

    private void Update()
    {
        if (!_isExecute)
        {
            return;
        }

        if (
            MatA.height != MatC.height ||
            MatB.width != MatC.width ||
            MatA.width != MatB.height
        )
        {
            Debug.LogError("Matrix size missmatch");
        }

        var m = MatA.height;
        var mIndMaxCurrentFrame = Mathf.Min(_currentMInd + _numAccumulatePerFrame, m);
        var matCTemp = new RenderTexture(MatC.width, MatC.height, 0, RenderTextureFormat.RFloat, 0);
        _matmulMaterial.SetTexture("_MatA", MatA);
        _matmulMaterial.SetTexture("_MatB", MatB);
        for (; _currentMInd < mIndMaxCurrentFrame; _currentMInd++)
        {
            _matmulMaterial.SetInt("_MInd", _currentMInd);
            VRCGraphics.Blit(MatC, matCTemp, _matmulMaterial);
            VRCGraphics.Blit(matCTemp, MatC);
        }
        if (_currentMInd == m)
        {
            _isExecute = false;
        }
    }

    [ContextMenu("Execute")]
    public void Execute()
    {
        _currentMInd = 0;
        _isExecute = true;
    }
}
