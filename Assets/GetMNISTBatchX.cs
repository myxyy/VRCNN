
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class GetMNISTBatchX : IVariable
{
    [SerializeField]
    private int _batchSize = 64;
    private const int VECTOR_SIZE = 28*28;
    [SerializeField]
    private Material _MNISTXMaterial;

    private RenderTexture _data;
    public override RenderTexture Data() => _data;
    private void Start()
    {
        _data = new RenderTexture(_batchSize, VECTOR_SIZE, 0, RenderTextureFormat.RFloat, 0);
        _data.filterMode = FilterMode.Point;
        _isTextureReady = true;

        var indexList = new int[_batchSize];
        for (int i = 0; i < indexList.Length; i++)
        {
            indexList[i] = i;
        }
        _MNISTXMaterial.SetInt("_BatchSize", _batchSize);

        FetchData(indexList);
    }

    public void FetchData(int[] indexList)
    {
        var indexListFloat = new float[indexList.Length];
        for (int i = 0; i < indexList.Length; i++)
        {
            indexListFloat[i] = (float)indexList[i];
        }
        _MNISTXMaterial.SetFloatArray("_IndexList", indexListFloat);
        VRCGraphics.Blit(null, _data, _MNISTXMaterial);
    }

}
