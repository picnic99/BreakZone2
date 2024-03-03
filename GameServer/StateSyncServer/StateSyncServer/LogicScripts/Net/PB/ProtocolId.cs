﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Net.PB
{
    public class ProtocolId
    {
        public static int CLIENT_PLAYER_LOGIN_REQ = 10001;//角色登录请求
        public static int CLIENT_PLAYER_LOGIN_REP = 10002;//角色登录返回
        public static int CLIENT_PLAYER_BASE_INFO_NTF = 10003;//玩家基础信息
        public static int CLIENT_SELECT_ROLE_REQ = 10004;//选择角色请求
        public static int CLIENT_SELECT_ROLE_REP = 10005;//选择角色返回
        public static int CLIENT_ENTER_SCENE_REQ = 10006;//进入场景请求
        public static int CLIENT_ENTER_SCENE_REP = 10007;//进入场景返回
        public static int CLENT_GET_GAME_DATA_REQ = 10008;//获取游戏数据请求
        public static int CLENT_GET_GAME_DATA_REP = 10009;//获取游戏数据返回
        public static int CLENT_SYNC_GAME_DATA_NTF = 10010;//游戏数据同步通知
        public static int CLIENT_GAME_PLAYER_OPT_REQ = 20001;//玩家操作
        public static int CLIENT_GAME_PLAYER_OPT_REP = 20002;//玩家操作返回
        public static int CLIENT_OBJ_CREATE_NTF = 20003;//物体创建
        public static int CLIENT_OBJ_TRANSFORM_NTF = 20004;//物体变化
        public static int CLIENT_ANIM_PLAY_NTF = 20005;//动画播放
        public static int CLIENT_AUDIO_PLAY_NTF = 20006;//音频播放
        public static int CLIENT_PLAYER_PROPERTY_CHANGE_NTF = 20007;//玩家属性发送改变
    }
}
