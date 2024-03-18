#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;
    using UnityEditor;
    using Sirenix.OdinInspector;
    using Sirenix.Utilities;
    using Sirenix.OdinInspector.Editor;
    using Sirenix.Utilities.Editor;
    using System.Collections.Generic;
    using System;
    using System.IO;
    using Unity.Plastic.Newtonsoft.Json;
    using static Sirenix.OdinInspector.Demos.AnimToolsEditor.AnimToolClass;

    public class AnimToolsEditor : OdinMenuEditorWindow
    {
        [MenuItem("Quick/������ع���")]
        private static void OpenWindow()
        {
            var window = GetWindow<AnimToolsEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        }

        /// <summary>
        /// ���Ʋ˵���
        /// </summary>
        /// <returns></returns>
        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: false)
            {
                { "Tools",new AnimToolClass(),EditorIcons.UnityLogo} // Draws the this.someData field in this case.
            };
            return tree;
        }


        [Serializable]
        public class AnimToolClass
        {
            [Serializable]
            public class M_Vector3
            {
                public float X;
                public float Y;
                public float Z;

                public static M_Vector3 Exchange(Vector3 v)
                {
                    var MV = new M_Vector3();
                    MV.X = v.x;
                    MV.Y = v.y;
                    MV.Z = v.z;
                    return MV;
                }
            }

            [Serializable]
            public class AnimFileData
            {
                [VerticalGroup("��������"), LabelWidth(70)]
                public string animName;

                [VerticalGroup("��������"), LabelWidth(50)]
                public List<M_Vector3> curves;

                [VerticalGroup("��������"), LabelWidth(50)]
                public float time;

                [NonSerialized]
                private AnimationClip clip;
                [NonSerialized]
                private AnimationCurve curveX = null;
                [NonSerialized]
                private AnimationCurve curveY = null;
                [NonSerialized]
                private AnimationCurve curveZ = null;


                public void SetClip(AnimationClip c)
                {
                    clip = c;
                }
                /// <summary>
                /// ����������Ϣ
                /// </summary>
                /// <param name="rate"></param>
                public void CreateCurve(int rate)
                {
                    EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(clip);

                    foreach (var curveBinding in curveBindings)
                    {
                        var curveName = curveBinding.propertyName;
                        if (curveName.StartsWith("RootT.x"))
                        {
                            curveX = AnimationUtility.GetEditorCurve(clip, curveBinding);
                            continue;
                        }
                        if (curveName.StartsWith("RootT.y"))
                        {
                            curveY = AnimationUtility.GetEditorCurve(clip, curveBinding);
                            continue;
                        }
                        if (curveName.StartsWith("RootT.z"))
                        {
                            curveZ = AnimationUtility.GetEditorCurve(clip, curveBinding);
                            continue;
                        }
                    }
                    ExportData(rate);
                }

                public void ExportData(int FrameRate)
                {
                    List<M_Vector3> list = new List<M_Vector3>();
                    float fixedTime = 1f / FrameRate;

                    float animTime = clip.length;

                    float count = Mathf.Ceil(animTime / fixedTime);
                    for (int i = 0; i <= count; i++)
                    {
                        var pos = GetoffsetByTime(i * fixedTime);
                        list.Add(M_Vector3.Exchange(pos));
                    }
                    curves = list;
                    time = clip.length;
                }

                public Vector3 GetoffsetByTime(float time)
                {
                    float x = curveX != null ? curveX.Evaluate(time) : 0;
                    float y = curveY != null ? curveY.Evaluate(time) : 0;
                    float z = curveZ != null ? curveZ.Evaluate(time) : 0;
                    return new Vector3(x, y, z);
                }
            }

            [TableList(ShowIndexLabels = true)]
            public List<AnimFileData> clips = new List<AnimFileData>();

            [Title("������")]
            [PropertyRange(0, "maxRate")]
            public int rate = 20;

            [HideInInspector]
            public int maxRate = 60;

            [Button("��ȡ���ж���")]
            public void InitAnimClips()
            {
                string path = Application.dataPath + "/Res/Anims";

                List<string> files = PrintAnimFiles(path);
                clips.Clear();
                foreach (var item in files)
                {
                    var c = AssetDatabase.LoadAssetAtPath<AnimationClip>(item);
                    var data = new AnimFileData();
                    data.SetClip(c);
                    string p = item.Substring(17,item.Length - 22);
                    p = p.Replace('\\','/');
                    data.animName = p;
                    data.CreateCurve(rate);
                    clips.Add(data);
                }
            }

            [Button("�������������ļ�")]
            public void GetAnimCurve()
            {
                if(clips.Count == 0)
                {
                    Debug.LogWarning("δ�ҵ��κζ����ļ� ���ȵ����ȡ���ж�����");
                    return;
                }
                string json = JsonConvert.SerializeObject(clips);
                Debug.Log(json);

                string filePath = Application.dataPath + "/../Config/output/customJson";
                File.WriteAllText(filePath + "/animData.json", json);

                QuickToolsEditor.ShellExecute(IntPtr.Zero, "open", filePath, "", "", 1);
            }

            static List<string> PrintAnimFiles(string folderPath)
            {
                List<string> result = new List<string>();
                int len = Application.dataPath.Length - 5;
                try
                {
                    // �����ļ����е��ļ������ļ���  
                    foreach (string filePath in Directory.GetFiles(folderPath, "*.anim", SearchOption.AllDirectories))
                    {
                        // ��ӡ�ļ�������·��  
                        string s = filePath.Substring(len -1, filePath.Length - len + 1);
                        result.Add(s);
                    }
                }
                catch (Exception ex)
                {
                    // ������ܳ��ֵ��쳣�������ļ��в����ڻ�û���㹻��Ȩ�޵�  
                    Console.WriteLine("An error occurred: " + ex.Message);
                }

                return result;
            }
        }
    }
}
#endif
