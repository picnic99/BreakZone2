
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.LogicScripts.Client.Manager;
using UnityEngine;
using Assets.LogicScripts.Client.Net.PB;
using Assets.LogicScripts.Client.Net.PB.Enum;
using Msg;
using Assets.LogicScripts.Utils;
using Assets.LogicScripts.Client.Entity;

namespace Assets.LogicScripts.Client
{
    /// <summary>
    /// 行为转发器
    /// </summary>
    class ActionManager : Manager<ActionManager>
    {

        public override void AddEventListener()
        {
            base.AddEventListener();
            EventDispatcher.GetInstance().On(ProtocolId.CLIENT_PLAYER_LOGIN_REP, LoginRepHandle);
            EventDispatcher.GetInstance().On(ProtocolId.CLIENT_PLAYER_BASE_INFO_NTF, HandleBaseInfoNtf);
            EventDispatcher.GetInstance().On(ProtocolId.CLIENT_SELECT_ROLE_REP, SelectCrtHandle);
            EventDispatcher.GetInstance().On(ProtocolId.CLIENT_ENTER_SCENE_REP, EnterSceneHandle);

        }

        public override void RemoveEventListener()
        {
            base.RemoveEventListener();
            EventDispatcher.GetInstance().Off(ProtocolId.CLIENT_PLAYER_LOGIN_REP, LoginRepHandle);
            EventDispatcher.GetInstance().Off(ProtocolId.CLIENT_PLAYER_BASE_INFO_NTF, HandleBaseInfoNtf);
            EventDispatcher.GetInstance().Off(ProtocolId.CLIENT_SELECT_ROLE_REP, SelectCrtHandle);
            EventDispatcher.GetInstance().Off(ProtocolId.CLIENT_ENTER_SCENE_REP, EnterSceneHandle);

        }


        public void SendLoginReq(string username, string password, int type)
        {
/*            if (!String.IsNullOrEmpty(username) || !String.IsNullOrEmpty(password))
            {
                //提示
            }*/
            var req = new PlayerLoginReq();
            req.Username = username;
            req.Password = password;
            req.Type = type;
            NetManager.GetInstance().SendProtocol(req);
        }

        public void SendSelectCrtReq(int crtId)
        {
            var req = new SelectCrtReq();
            req.CrtId = crtId;
            NetManager.GetInstance().SendProtocol(req);
        }

        public void SendEnterSceneReq(int sceneId = 1)
        {
            var req = new EnterSceneReq();
            req.SceneId = sceneId;
            NetManager.GetInstance().SendProtocol(req);
        }

        public void SelectCrtHandle(object[] args)
        {
            Protocol proto = args[0] as Protocol;

            SelectCrtRep rep = proto.GetDataInstance<SelectCrtRep>();

            var result = rep.Result;
            var crtId = rep.CrtId;
            if (result == ProtocolResultEnum.SUCCESS)
            {
                CommonUtils.Logout("选择角色" + crtId + "完毕");
                SendEnterSceneReq(PlayerManager.GetInstance().Self.lastStaySceneId);
            }
        }

        public void EnterSceneHandle(object[] args)
        {
            Protocol proto = args[0] as Protocol;

            EnterSceneRep rep = proto.GetDataInstance<EnterSceneRep>();

            var result = rep.Result;
            var SceneId = rep.SceneId;
            if (result == ProtocolResultEnum.SUCCESS)
            {
                CommonUtils.Logout("进入场景" + SceneId + "请求成功");
                GameSceneManager.GetInstance().SwitchScene(RegSceneClass.GameRoomScene);
            }
        }

        private void LoginRepHandle(object[] args)
        {
            Protocol proto = args[0] as Protocol;

            PlayerLoginRep rep = proto.GetDataInstance<PlayerLoginRep>();

            var result = rep.Result;
            var type = rep.Type;
            if (result == ProtocolResultEnum.SUCCESS)
            {
                if (type == LoginTypeEnum.LOGIN_IN)
                {
                    //登录
                    CommonUtils.Logout("登录成功!");
                }
                else if (type == LoginTypeEnum.LOGIN_Out)
                {
                    //退出登录
                    CommonUtils.Logout("登出成功!");
                    //清理数据
                    PlayerManager.GetInstance().ClearPlayer();
                    //返回到登录场景
                    GameSceneManager.GetInstance().SwitchScene(RegSceneClass.LoginScene);
                }
            }
            else
            {
                CommonUtils.Logout("PlayerLoginRep 操作失败");
            }
        }

        private void HandleBaseInfoNtf(object[] args)
        {
            Protocol proto = args[0] as Protocol;
            PlayerBaseInfoNtf rep = proto.GetDataInstance<PlayerBaseInfoNtf>();
            var username = rep.Username;
            var lastStaySceneId = rep.LastStaySceneId;
            
            Vector3 lastStayPos = new Vector3(
                rep.LastStayPosX, 
                rep.LastStayPosY,
                rep.LastStayPosZ);

            Player p = new Player(username, (int)lastStaySceneId, lastStayPos);
            PlayerManager.GetInstance().Self = p;
            CommonUtils.Logout("玩家信息同步完成! 即将切换到选择角色");
            GameSceneManager.GetInstance().SwitchScene(RegSceneClass.SelectRoleScene);
        }



        /*        public void Handle(Protocol protocol)
                {
                    switch (protocol.protocolId)
                    {
                        case ProtocolId.CLIENT_PLAYER_LOGIN_REP:
                            Debug.Log("收到角色登录返回");
                            Handle_LoginRep(protocol);
                            break;
                        case ProtocolId.CLIENT_PLAYER_BASE_INFO_NTF:
                            Debug.Log("收到玩家的基础信息");
                            Handle_PlayerBaseInfoNtf(protocol);
                            break;
                        case ProtocolId.CLIENT_SELECT_ROLE_REP:
                            Debug.Log("选择角色返回");

                            break;
                        case ProtocolId.CLIENT_ENTER_SCENE_REP:
                            Debug.Log("进入场景返回");
                            break;
                        case ProtocolId.CLENT_GET_GAME_DATA_REP:
                            Debug.Log("获取游戏数据返回");
                            break;
                        case ProtocolId.CLENT_SYNC_GAME_DATA_NTF:
                            Debug.Log("游戏数据同步通知");
                            break;
                        case ProtocolId.CLIENT_GAME_PLAYER_OPT_REP:
                            Debug.Log("玩家操作返回");
                            break;
                        case ProtocolId.CLIENT_OBJ_CREATE_NTF:
                            Debug.Log("物体创建");
                            break;
                        case ProtocolId.CLIENT_OBJ_TRANSFORM_NTF:
                            Debug.Log("物体变化");
                            break;
                        case ProtocolId.CLIENT_ANIM_PLAY_NTF:
                            Debug.Log("动画播放");
                            break;
                        case ProtocolId.CLIENT_AUDIO_PLAY_NTF:
                            Debug.Log("音频播放");
                            break;
                        case ProtocolId.CLIENT_PLAYER_PROPERTY_CHANGE_NTF:
                            Debug.Log("玩家属性发送改变");
                            break;
                        default:
                            break;
                    }
                }

                public void Send_LoginReq(string username,string password)
                {
                    if (!String.IsNullOrEmpty(username) || !String.IsNullOrEmpty(password))
                    {
                        //提示
                    }
                    var req = new PlayerLoginReq();
                    req.username = username;
                    req.password = password;
                    NetManager.GetInstance().SendProtocol(req);
                }
                private void Handle_LoginRep(Protocol proto)
                {
                    PlayerLoginRep action = proto as PlayerLoginRep;
                    int result = action.result;
                    int type = action.type;
                    if (result == (int)ProtocolResultEnum.SUCCESS)
                    {
                        if (type == 0)
                        {
                            //退出登录
                            //清理数据
                            //返回到登录场景
                        }
                        else if (type == 1)
                        {
                            //登录
                            //写入数据
                            //进入到选择角色场景
                        }
                    }
                }*/

        private void Handle_PlayerBaseInfoNtf(Protocol proto)
        {
            /*            PlayerBaseInfoNtf action = proto as PlayerBaseInfoNtf;
                        var baseInfo = action.baseInfo;
                        if (baseInfo != null)
                        {
                            Common.GameContext.playerBaseInfo = baseInfo;
                        }*/
        }
    }
}