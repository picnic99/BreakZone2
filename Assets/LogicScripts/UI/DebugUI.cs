using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUI : UIBase
{
    public DebugUI()
    {
        uiPath = RegPrefabs.DebugUI;
        layer = UILayers.SYSTEM;
    }
}
