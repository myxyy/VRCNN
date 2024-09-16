
using System.Runtime.InteropServices;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ForwardTest : UdonSharpBehaviour
{
    [SerializeField]
    private IVariable _a;
    [SerializeField]
    private Material _initA; 
    [SerializeField]
    private IVariable _b;
    [SerializeField]
    private Material _initB; 
    [SerializeField]
    private IVariable _c;
    [SerializeField]
    private Material _initC; 
    [SerializeField]
    private IVariable _d;
    [SerializeField]
    private Material _initD; 
    [SerializeField]
    private IVariable _e;
    [SerializeField]
    private Material _initE; 
    [SerializeField]
    private bool _forward;

    private bool _isTextureInit = false;
    private void Start()
    {
    }

    private void Update()
    {
        if (
            !_isTextureInit &&
            _a.IsTextureReady() &&
            _b.IsTextureReady() &&
            _c.IsTextureReady() &&
            _d.IsTextureReady() &&
            _e.IsTextureReady()
        )
        {
            VRCGraphics.Blit(null, _a.Data(), _initA);
            VRCGraphics.Blit(null, _b.Data(), _initB);
            VRCGraphics.Blit(null, _c.Data(), _initC);
            VRCGraphics.Blit(null, _d.Data(), _initD);
            VRCGraphics.Blit(null, _e.Data(), _initE);
            _isTextureInit = true;
        }

        if (_forward)
        {
            VRCGraphics.Blit(null, _a.Data(), _initA);
            VRCGraphics.Blit(null, _b.Data(), _initB);
            VRCGraphics.Blit(null, _c.Data(), _initC);
            VRCGraphics.Blit(null, _d.Data(), _initD);
            VRCGraphics.Blit(null, _e.Data(), _initE);
 
            _a.Forward();
            _forward = false;
        }
    }
}
