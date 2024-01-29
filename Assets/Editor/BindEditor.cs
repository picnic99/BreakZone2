
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIBinding))]
public class BindEditor : Editor
{
    SerializedProperty list;
    private void Awake()
    {
        list =  serializedObject.FindProperty("bindList");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(list);

        //GUILayout.TextField("");
        UIBinding bind = target as UIBinding;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("一键添加"))
        {
            bind.bindList.Clear();
            for (int i = 0; i < bind.transform.childCount; i++)
            {
                Bind temp = new Bind();
                GameObject go = bind.transform.GetChild(i).gameObject;
                temp.name = go.name;
                temp.obj = go;
                temp.className = go.GetType().Name;
                temp.isSelect = false;
                bind.bindList.Add(temp);
            }
        }
        if (GUILayout.Button("添加"))
        {
            bind.bindList.Add(new Bind());
        }
        if (GUILayout.Button("删除"))
        {
            for (int i = 0; i < bind.bindList.Count; i++)
            {
                if (bind.bindList[i].isSelect)
                {
                    bind.bindList.RemoveAt(i);
                }


            }
        }
        GUILayout.EndHorizontal();

        if (bind.bindList.Count<=0)
        {
            return;
        }

        for (int i = 0; i < list.arraySize; i++)
        {
            int index = 0;
            SerializedProperty item = list.GetArrayElementAtIndex(i);
            GUILayout.BeginHorizontal();
            item.FindPropertyRelative("name").stringValue = 
                EditorGUILayout.TextField(item.FindPropertyRelative("name").stringValue, 
                new GUILayoutOption[] { GUILayout.Width(80), GUILayout.Height(20) });
            item.FindPropertyRelative("obj").objectReferenceValue = 
                EditorGUILayout.ObjectField(item.FindPropertyRelative("obj").objectReferenceValue, 
                typeof(GameObject), new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(20) }) as GameObject;
            if (item.FindPropertyRelative("name").stringValue == "" && item.FindPropertyRelative("obj").objectReferenceValue!=null) item.FindPropertyRelative("name").stringValue = item.FindPropertyRelative("obj").objectReferenceValue.name;
            if (item.FindPropertyRelative("obj").objectReferenceValue != null)
            {
                string[] sfull = GetComponentsName(item.FindPropertyRelative("obj").objectReferenceValue as GameObject);
                string[] s = new string[sfull.Length];
                for (int j = 0; j < sfull.Length; j++)
                {
                    s[j] = sfull[j].Substring(sfull[j].LastIndexOf(".") + 1, sfull[j].Length - sfull[j].LastIndexOf(".") - 1);
                }

                if (item.FindPropertyRelative("className").stringValue != null)
                {
                    for (int j = 0; j < s.Length; j++)
                    {
                        if (item.FindPropertyRelative("className").stringValue == sfull[j])
                        {
                            index = j;
                            break;
                        }

                    }
                }

                index = EditorGUILayout.Popup(index, s);

                item.FindPropertyRelative("className").stringValue = sfull[index];
            }

            item.FindPropertyRelative("isSelect").boolValue = 
                EditorGUILayout.Toggle(item.FindPropertyRelative("isSelect").boolValue, 
                new GUILayoutOption[] { GUILayout.Width(20), GUILayout.Height(20) });
            GUILayout.EndHorizontal();
        }

        serializedObject.ApplyModifiedProperties();

    }

    public string[] GetComponentsName(GameObject o)
    {
        if (o == null)
        {
            return null;
        }

        Component[] components = o.GetComponents<Component>();

        string[] sArr = new string[components.Length+1];

        int i = 1;

        sArr[0] = "UnityEngine.GameObject";

        foreach (var item in components)
        {
            sArr[i++] = item.GetType().FullName;
        }

        return sArr;
    }
}