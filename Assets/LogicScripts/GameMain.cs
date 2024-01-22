using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameMain : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        //Time.timeScale = 0.2f;
        UIManager.GetInstance().ShowUI(RegUIClass.SelectRoleUI);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameContext.SelfRole == null) return;


        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
        //GameContext.SelfRole.characterAnimator.moveBlendMixer.SetPointer(x,z);

        if (Input.GetKeyDown(KeyCode.M))
        {
            GameContext.SelfRole.physic.Move(Vector3.up, 2f);
            GameContext.SelfRole.physic.Move(-Vector3.forward, 2f);

            //toClearConsole();
            //InputManager.GetInstance().ChangeState(GameContext.SelfRole, StateType.Roll);
            //InputManager.GetInstance().ChangeState(GameContext.SelfRole, StateType.Move);
            //InputManager.GetInstance().ChangeState(GameContext.SelfRole, StateType.Roll);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            GameContext.SelfRole.physic.FreezonMove(5f);

            //toClearConsole();
            //InputManager.GetInstance().ChangeState(GameContext.SelfRole, StateType.Roll);
            //InputManager.GetInstance().ChangeState(GameContext.SelfRole, StateType.Move);
            //InputManager.GetInstance().ChangeState(GameContext.SelfRole, StateType.Roll);
        }
    }

    private void toClearConsole()
    {
/*        //获取UnityEditor程序集里面的UnityEditorInternal.LogEntries类型，也就是把关于Console的类提出来
        var logEntries = System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");
        //在logEntries类里面找到名为Clear的方法，且其属性必须是public static的，等同于得到了Console控制台左上角的clear，然后通过Invoke进行点击实现
        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        clearMethod.Invoke(null, null);*/

               Type log = typeof(EditorWindow).Assembly.GetType("UnityEditor.LogEntries");
               var clearMethod = log.GetMethod("Clear");
               clearMethod.Invoke(null, null);
    }
} 
