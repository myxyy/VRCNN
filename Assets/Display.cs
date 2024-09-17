
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[RequireComponent(typeof(MeshRenderer))]
public class Display : UdonSharpBehaviour
{
    [SerializeField]
    private IVariable _target;
    [SerializeField]
    private bool _isGrad = false;
    [SerializeField]
    private GameObject _displayPrefab;
    private Material _displayMaterial;
    private bool _isSetComplete = false;

    private void Update()
    {
        if (!_isSetComplete && _target.IsTextureReady())
        {
            SetTexture(_isGrad ? _target.Grad() : _target.Data());
        }
    }
    private void SetTexture(RenderTexture renderTexture)
    {
        if (renderTexture == null)
        {
            return;
        }
        var displayObject = Instantiate(_displayPrefab);
        _displayMaterial = displayObject.GetComponent<MeshRenderer>().material;
        Destroy(displayObject);
        var renderer = GetComponent<MeshRenderer>();
        renderer.material = _displayMaterial;
        _displayMaterial.SetTexture("_MainTex", renderTexture);
    }
}
