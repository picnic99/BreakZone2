using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.LogicScripts.Client.Net.Protocols
{
    public enum ProtocolId
    {
/*        CLENT_LOGIN_IN_REQ = 10001,//登录请求
        CLENT_LOGIN_IN_REP = 10002,//登录返回
        CLENT_ADD_ROOM_REQ = 10003,//加入房间请求
        CLENT_ADD_ROOM_REP = 10004,//加入房间返回
        CLENT_PLAYER_OPT_REQ = 10005,//角色操作请求
        CLENT_PLAYER_OPT_REP = 10006,//角色操作返回
        CLENT_GET_GAME_DATA_REQ = 10007,//获取游戏数据请求
        CLENT_GET_GAME_DATA_REP = 10008,//获取游戏数据返回
        CLENT_GAME_DATA_NTF = 10009,//游戏数据同步通知
        CLENT_PLAYER_ADD_NTF = 10010,//有新玩家加入房间通知
        CLENT_BULLET_ADD_NTF = 10011,//有弹道新增*/

        CLIENT_PLAYER_LOGIN_REQ = 10001,//角色登录请求
        CLIENT_PLAYER_LOGIN_REP = 10002,//角色登录返回
        CLIENT_PLAYER_BASE_INFO_NTF = 10003,//玩家基础信息
        CLIENT_SELECT_ROLE_REQ = 10004,//选择角色请求
        CLIENT_SELECT_ROLE_REP = 10005,//选择角色返回
        CLIENT_ENTER_SCENE_REQ = 10006,//进入场景请求
        CLIENT_ENTER_SCENE_REP = 10007,//进入场景返回
        CLENT_GET_GAME_DATA_REQ = 10008,//获取游戏数据请求
        CLENT_GET_GAME_DATA_REP = 10009,//获取游戏数据返回
        CLENT_SYNC_GAME_DATA_NTF = 10010,//游戏数据同步通知
        
        CLIENT_GAME_PLAYER_OPT_REQ = 20001,//玩家操作
        CLIENT_GAME_PLAYER_OPT_REP = 20002,//玩家操作返回
        CLIENT_OBJ_CREATE_NTF = 20003,//物体创建
        CLIENT_OBJ_TRANSFORM_NTF = 20004,//物体变化
        CLIENT_ANIM_PLAY_NTF = 20005,//动画播放
        CLIENT_AUDIO_PLAY_NTF = 20006,//音频播放
        CLIENT_PLAYER_PROPERTY_CHANGE_NTF = 20007,//玩家属性发送改变
    }
}
