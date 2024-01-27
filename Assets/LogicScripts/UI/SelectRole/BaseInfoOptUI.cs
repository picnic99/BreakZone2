using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInfoOptUI : UIBase
{
    //private GameObject infoRoot { get { return UIBase.GetBind<GameObject>(Root, "infoRoot"); } }

    public BaseInfoOptUI()
    {
        uiPath = RegPrefabs.BaseInfoOptUI;
        layer = UILayers.MIDDLE;

    }
}
