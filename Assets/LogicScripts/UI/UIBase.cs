using UnityEngine;

public class UIBase
{
    public string uiPath;
    private GameObject root;
    public GameObject UIRoot
    {
        get
        {
            if (root != null) return root;
            if(uiPath!=null && uiPath != "")
            {
                GameObject obj = ResourceManager.GetInstance().GetObjInstance<GameObject>("UI/" + uiPath);
                root = obj;
                return root;
            }
            return null;
        }
    }
}