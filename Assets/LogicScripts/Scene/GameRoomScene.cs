using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRoomScene : GameScene
{
    private _Character crt;

    public GameRoomScene()
    {
        SceneName = RegSceneClass.GameRoomScene;
        IsFightScene = true;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //生成主角
/*        crt = CharacterManager.GetInstance().CreateFightCharacter(GameContext.SelectCrtId);
        SceneCrts.Add(crt);
        crt.baseInfo.isNeedStateBar = false;
        crt.trans.parent = GameObject.Find("SceneRoot").transform;
        GameContext.CurRole = crt;*/
        CameraManager.GetInstance().OpenCam();
        UIManager.GetInstance().ShowUI(RegUIClass.BaseInfoOptUI);
        //生成NPC
        
        //同步其它角色
    }
}
