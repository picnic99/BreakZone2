using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : Manager<CharacterManager>
{
    /// <summary>
    /// 角色 死亡
    /// </summary>
    public static string CHARACTER_DIE { get { return GetInstance().GetType().Name + "CHARACTER_DIE"; } }
    /// <summary>
    /// 创建角色
    /// </summary>
    public static string CREATE_CHARACTER { get { return GetInstance().GetType().Name + "CREATE_CHARACTER"; } }
    /// <summary>
    /// 移除角色
    /// </summary>
    public static string REMOVE_CHARACTER { get { return GetInstance().GetType().Name + "REMOVE_CHARACTER"; } }
    /// <summary>
    /// 场景对应角色
    /// </summary>
    public Dictionary<string, List<_Character>> sceneCrtDic = new Dictionary<string, List<_Character>>();

    public override void AddEventListener()
    {
        base.AddEventListener();
        Eventer.On(CHARACTER_DIE, OnCharacterDie);
        GameSceneManager.Eventer.On(GameSceneManager.UNLOAD_SCENE, RemoveCrtByScene);
    }


    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        Eventer.Off(CHARACTER_DIE, OnCharacterDie);
        GameSceneManager.Eventer.Off(GameSceneManager.UNLOAD_SCENE, RemoveCrtByScene);
    }


    public void OnCharacterDie(object[] args)
    {
        _Character crt = args[0] as _Character;
        if (crt != null)
        {
            crt.eventDispatcher.Event(CharacterEvent.CHANGE_STATE, crt, StateType.Die);
        }
    }


    /// <summary>
    /// 移除场景下的所有角色
    /// </summary>
    /// <param name="args"></param>
    public void RemoveCrtByScene(object[] args)
    {
        string scene = args[0] as string;

        if (sceneCrtDic.ContainsKey(scene))
        {
            List<_Character> crts = sceneCrtDic[scene];
            foreach (var item in crts)
            {
                RemoveCharacter(item);
            }
            sceneCrtDic.Remove(scene);
        }
    }

    /// <summary>
    /// 添加角色与场景映射
    /// </summary>
    /// <param name="c"></param>
    public void AddCrtToScene(_Character c)
    {
        string sceneName = "emptyScene";
        if (GameContext.CurScene != null)
        {
            sceneName = GameContext.CurScene.SceneName;
        }
        if (sceneCrtDic.ContainsKey(sceneName))
        {
            sceneCrtDic[sceneName].Add(c);
        }
        else
        {
            List<_Character> crts = new List<_Character>() { c };
            sceneCrtDic.Add(sceneName, crts);
        }
    }

    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="vo">角色信息</param>
    /// <param name="info">参数</param>
    /// <returns></returns>
    public _Character CreateCharacter(CharacterVO vo, CharacterBaseInfo info = null)
    {
        GameObject characterObj = ResourceManager.GetInstance().GetCharacterInstance<GameObject>(vo.character.ModePath);
        _Character c = new _Character(vo, characterObj, info);
        AddCrtToScene(c);
        Eventer.Event(CREATE_CHARACTER, c);
        return c;
    }
    public _Character CreateCharacter(int id)
    {
        var vo = CharacterConfiger.GetInstance().GetCharacterById(id);
        if (vo != null)
        {
            return CreateCharacter(vo);
        }
        return null;
    }

    /// <summary>
    /// 创建战斗用角色
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public _Character CreateFightCharacter(int id)
    {
        var vo = CharacterConfiger.GetInstance().GetCharacterById(id);
        if (vo != null)
        {
            GameObject characterObj = ResourceManager.GetInstance().GetCharacterInstance<GameObject>(vo.character.ModePath);
            _Character c = new _Character(vo, characterObj, CharacterBaseInfo.GetFightBaseInfo());
            AddCrtToScene(c);
            Eventer.Event(CREATE_CHARACTER, c);
            return c;
        }
        return null;
    }

    /// <summary>
    /// 创建木桩类型角色
    /// </summary>
    /// <returns></returns>
    public _Character CreateTestCharacter()
    {
        return CreateFightCharacter(98);

    }

    public List<_Character> GetSceneCrts(string sceneName)
    {
        if (sceneCrtDic.ContainsKey(sceneName))
        {
            return sceneCrtDic[sceneName];
        }
        return null;
    }

    public List<_Character> GetCurSceneCrts()
    {
        if (GameContext.CurScene == null) return null;
        return GetSceneCrts(GameContext.CurScene.SceneName);
    }

    /// <summary>
    /// 移除角色
    /// </summary>
    /// <param name="c"></param>
    public void RemoveCharacter(_Character c)
    {
        c.OnDestory();
        List<_Character> characters = GetCurSceneCrts();
        if (characters != null)
        {
            characters.Remove(c);
            Eventer.Event(REMOVE_CHARACTER, c);
        }
    }

    /// <summary>
    /// 刷新
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();
        List<_Character> characters = GetCurSceneCrts();
        if (characters == null) return;
        foreach (var item in characters)
        {
            item.OnUpdate();
        }
    }



}