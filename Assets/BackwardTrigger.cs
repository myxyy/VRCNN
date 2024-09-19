
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class BackwardTrigger : IFunction
{
    [SerializeField]
    private IVariable _input;
    [SerializeField]
    private Material _oneMaterial;
    void Start()
    {
        __inputList = new IVariable[1]{_input};
        InitVariables();
    }

    public override void Forward()
    {
        if (IsForwardReady())
        {
            VRCGraphics.Blit(null, _input.Grad(), _oneMaterial);
            _isBackwardComplete = true;
            _input.Backward();
        }
    }
}
