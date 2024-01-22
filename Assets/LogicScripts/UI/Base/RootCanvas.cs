using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootCanvas : UIBase
{
    private BaseCanvas BottomCvs;
    private BaseCanvas MiddleCvs;
    private BaseCanvas FrontCvs;
    private BaseCanvas SystemCvs;

    public RootCanvas()
    {
        uiPath = RegPrefabs.RootCanvas;
    }

    public override void OnLoad()
    {
        base.OnLoad();
        var bottomCvsUI = this.UIRoot.transform.Find("BottomCvs");
        var middleCvsUI = this.UIRoot.transform.Find("MiddleCvs");
        var frontCvsUI = this.UIRoot.transform.Find("FrontCvs");
        var systemCvsUI = this.UIRoot.transform.Find("SystemCvs");
        BottomCvs = new BaseCanvas(bottomCvsUI.gameObject);
        MiddleCvs = new BaseCanvas(middleCvsUI.gameObject);
        FrontCvs = new BaseCanvas(frontCvsUI.gameObject);
        SystemCvs = new BaseCanvas(systemCvsUI.gameObject);

        GameObject.DontDestroyOnLoad(this.UIRoot);
    }

    public BaseCanvas GetCvsByLayer(UILayers layer)
    {
        if (layer == UILayers.BOTTOM)
        {
            return BottomCvs;
        }
        if (layer == UILayers.MIDDLE)
        {
            return MiddleCvs;
        }
        if (layer == UILayers.FRONT)
        {
            return FrontCvs;
        }
        if (layer == UILayers.SYSTEM)
        {
            return SystemCvs;
        }
        return null;
    }

    public override void OnUnLoad()
    {
        base.OnUnLoad();
    }

    public void Init()
    {

    }

}
