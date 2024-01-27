using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : Manager<CharacterManager>
{
    public List<Character> characters = new List<Character>();

    public Character CreateCharacter(CharacterVO vo,CharacterBaseInfo info = null)
    {
        GameObject characterObj = ResourceManager.GetInstance().GetCharacterInstance<GameObject>(vo.character.ModePath);
        Character c = new Character(vo, characterObj, info);
        characters.Add(c);
        return c;
    }

    public Character CreateCharacter(int id)
    {
        var vo = CharacterConfiger.GetInstance().GetCharacterById(id);
        if(vo != null)
        {
            return CreateCharacter(vo);
        }
        return null;
    }

    public Character CreateFightCharacter(int id)
    {
        var vo = CharacterConfiger.GetInstance().GetCharacterById(id);
        if(vo != null)
        {
            GameObject characterObj = ResourceManager.GetInstance().GetCharacterInstance<GameObject>(vo.character.ModePath);
            Character c = new Character(vo, characterObj, CharacterBaseInfo.GetFightBaseInfo());
            characters.Add(c);
            return c;
        }
        return null;
    }



    public void RemoveCharacter(Character c)
    {
        c.OnDestory();
        characters.Remove(c);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        foreach (var item in characters)
        {
            item.OnUpdate();
        }
    }



}