using Assets.LogicScripts.Client.Net.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.LogicScripts.Client.Manager;
using Assets.LogicScripts.Client.Net.Enum;
using UnityEngine;

namespace Assets.LogicScripts.Client
{
    /// <summary>
    /// 行为转发器
    /// </summary>
    class ActionManager : Manager<ActionManager>
    {
        public void Handle(Protocol protocol)
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
        }

        private void Handle_PlayerBaseInfoNtf(Protocol proto)
        {
            PlayerBaseInfoNtf action = proto as PlayerBaseInfoNtf;
            var baseInfo = action.baseInfo;
            if (baseInfo != null)
            {
                Common.GameContext.playerBaseInfo = baseInfo;
            }
        }
    }
}