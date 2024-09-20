
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Misc;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class GetMNISTBatchY : IVariable
{
    [SerializeField]
    private int _batchSize = 64;
    private const int VECTOR_SIZE = 10;
    [SerializeField]
    private Material _MNISTYMaterial;
    [SerializeField]
    private Material _showMNISTLabelMaterial;

    private RenderTexture _data;
    public override RenderTexture Data() => _data;
    private void Start()
    {
        _data = new RenderTexture(_batchSize, VECTOR_SIZE, 0, RenderTextureFormat.RFloat, 0);
        _data.filterMode = FilterMode.Point;
        _isTextureReady = true;

        _showMNISTLabelMaterial.SetTexture("_MainTex", _data);

        var indexList = new int[_batchSize];
        for (int i = 0; i < indexList.Length; i++)
        {
            indexList[i] = i;
        }
        _MNISTYMaterial.SetInt("_BatchSize", _batchSize);

        FetchData(indexList);
    }

    public void FetchData(int[] indexList)
    {
        var indexListFloat = new float[indexList.Length];
        for (int i = 0; i < indexList.Length; i++)
        {
            indexListFloat[i] = (float)indexList[i];
        }
        _MNISTYMaterial.SetFloatArray("_IndexList", indexListFloat);
        VRCGraphics.Blit(null, _data, _MNISTYMaterial);
    }
}
