
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[RequireComponent(typeof(MeshRenderer))]
public class Display : UdonSharpBehaviour
{
    [SerializeField]
    private GameObject _displayPrefab;
    private Material _displayMaterial;

    private void Start()
    {
        var displayObject = Instantiate(_displayPrefab);
        _displayMaterial = displayObject.GetComponent<MeshRenderer>().material;
        Destroy(displayObject);
        var renderer = GetComponent<MeshRenderer>();
        renderer.material = _displayMaterial;
    }

    public void SetTexture(RenderTexture renderTexture)
    {
        _displayMaterial.SetTexture("_MainTex", renderTexture);
    }
}
