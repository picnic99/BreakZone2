using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorTools : EditorWindow
{
    float scale = 1.0f;

    [MenuItem("Examples/EditorGUI Slider usage")]
    static void Init()
    {
        var window = GetWindow<EditorTools>();
        window.position = new Rect(0, 0, 150, 30);
        window.Show();
    }

    void OnGUI()
    {
        scale = EditorGUI.Slider(new Rect(5, 5, 150, 20), scale, 0, 1);
    }

    void OnInspectorUpdate()
    {
        Time.timeScale = scale;
    }
}
