﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransfigurationBuff : Buff
{
    public int Duration { get; set; }
    public Transform target;
    private Animator animator;

    private GameObject originRender;
    private Avatar originAvatar;
    private RuntimeAnimatorController originController;
    private Vector3 originArrowPosition;

    private GameObject targetRender;
    private Vector3 targetArrowPosition;

    public TransfigurationBuff(int duration, Transform target)
    {
        this.target = target;
        if (duration == 0)
        {
            Duration = duration;
        }
        else
        {
            Duration = RoundManager.GetInstance().Players.Count * duration - 1;
        }
    }

    public void Apply(Transform character)
    {
        animator = character.GetComponent<Animator>();

        originRender = character.Find("Render").gameObject;
        originArrowPosition = character.GetComponent<CharacterStatus>().arrowPosition;
        originAvatar = character.GetComponent<Animator>().avatar;
        originController = character.GetComponent<Animator>().runtimeAnimatorController;
        originRender.SetActive(false);

        if (animator.avatar != target.GetComponent<Animator>().avatar)
        {
            var originHash = animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
            var originTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            targetArrowPosition = target.GetComponent<CharacterStatus>().arrowPosition;
            var go = target.Find("Render").gameObject;
            targetRender = GameObject.Instantiate(go);
            targetRender.transform.SetParent(character);
            targetRender.transform.position = character.position;
            targetRender.transform.rotation = character.rotation;

            animator.avatar = target.GetComponent<Animator>().avatar;
            animator.runtimeAnimatorController = target.GetComponent<Animator>().runtimeAnimatorController;

            //否则会无限循环触发事件，从而无限创建Render。
            animator.Play(originHash, 0, originTime + 0.02f);
            character.GetComponent<CharacterStatus>().arrowPosition = targetArrowPosition;
            character.GetComponent<CharacterStatus>().SetTransfiguration();
        }
        else
        {
            character.GetComponent<CharacterStatus>().SetTransfiguration();
            originRender.SetActive(true);
        }
    }

    public Buff Clone()
    {
        throw new NotImplementedException();
    }

    public void Undo(Transform character)
    {
        if (!originRender.activeInHierarchy)
        {
            GameObject.Destroy(targetRender);
            originRender.SetActive(true);
            character.GetComponent<CharacterStatus>().arrowPosition = originArrowPosition;

            var animator = character.GetComponent<Animator>();

            animator.avatar = originAvatar;
            animator.runtimeAnimatorController = originController;
        }
        
        character.GetComponent<CharacterStatus>().SetNoumenon();
    }
}