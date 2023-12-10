using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBinding : MonoBehaviour {

    public Button btn_skillEditor;
    public Button btn_backScene;
    public Button animDebugBtn;
    public GameObject obj_skillEditor;
    public GameObject obj_skillEditorStage;
    public GameObject obj_opt;


















    public static UIBinding _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public static UIBinding GetInstance()
    {
        return _instance;
    }
}