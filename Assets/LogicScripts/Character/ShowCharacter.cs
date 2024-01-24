using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCharacter
{
	public CharacterAnimator characterAnimator;
    public CharacterVO characterData;
    public Transform trans;
    public Animator anim;

    public ShowCharacter(CharacterVO vo, GameObject obj)
    {
        characterData = vo;
        obj.SetActive(true);
        trans = obj.transform;
        anim = obj.GetComponent<Animator>();
        characterAnimator = new CharacterAnimator();
        characterAnimator.Init(anim, null);
        characterAnimator.Play("Default/idle", 0.2f);
    }
}
