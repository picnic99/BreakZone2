using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUIBinding : MonoBehaviour {

    public Button btn_skillEditor;
    public Button btn_backScene;
    public Button animDebugBtn;
    public GameObject obj_skillEditor;
    public GameObject obj_skillEditorStage;
    public GameObject obj_opt;


















    public static DebugUIBinding _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public static DebugUIBinding GetInstance()
    {
        return _instance;
    }
}