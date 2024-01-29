using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRoomScene : GameScene
{
    private Character crt;

    public GameRoomScene()
    {
        SceneName = RegSceneClass.GameRoomScene;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        CameraManager.GetInstance().OpenCam();

        //生成主角
        crt = CharacterManager.GetInstance().CreateFightCharacter(GameContext.SelectCrtId);
        SceneCrts.Add(crt);
        crt.baseInfo.isNeedStateBar = false;
        crt.trans.parent = GameObject.Find("SceneRoot").transform;
        GameContext.CurRole = crt;
        CameraManager.GetInstance().OpenCam();
        UIManager.GetInstance().ShowUI(RegUIClass.BaseInfoOptUI);
        //生成NPC
        Character npc = CharacterManager.GetInstance().CreateFightCharacter(4);
        SceneCrts.Add(npc);
        npc.state = CharacterState.ENEMY;
        npc.trans.parent = GameObject.Find("SceneRoot").transform;
        Transform npcPos = bind["npcPos"].AS<GameObject>().transform;
        npc.physic.Move(npcPos.position,0.01f);
        npc.trans.rotation = npcPos.rotation;
        
        //同步其它角色
    }
}
