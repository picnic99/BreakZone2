using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRoomScene : GameScene
{
    public GameRoomScene()
    {
        SceneName = RegSceneClass.GameRoomScene;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        //生成主角
        Character crt = CharacterManager.GetInstance().CreateFightCharacter(GameContext.SelectCrtId);
        crt.trans.parent = GameObject.Find("Plane").transform;
        GameContext.CurRole = crt;
        CameraManager.GetInstance().ShowCam();
        UIManager.GetInstance().ShowUI(RegUIClass.BaseInfoOptUI);
        //生成NPC

        //同步其它角色
    }
}
