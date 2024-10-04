
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
    [SerializeField]
    private ForwardTest _forwardTest;

    private RenderTexture _data;
    public override RenderTexture Data() => _data;
    private void Start()
    {
        _data = new RenderTexture(_batchSize, VECTOR_SIZE, 0, RenderTextureFormat.RFloat, 0);
        _data.filterMode = FilterMode.Point;
        SetIsTextureReady();
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

    public override void Backward()
    {
        base.Backward();
        _forwardTest.Next();
    }
}
