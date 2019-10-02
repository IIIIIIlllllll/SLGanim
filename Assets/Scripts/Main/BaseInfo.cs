﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseInfo : MonoBehaviour {

    public Text roleName;
    public Text roleLevel;
    public Text roleSkillPointInfo;
    public Text info;
    public Slider experience;
    public Transform roleMenu;
    public Transform skillMenu;
    public Transform itemMenu_Role;
    public Transform roleInfo;
    GameObject roleInfoPanel;
    
    public void SyncRoleMenu()
    {
        if (roleMenu.gameObject.activeSelf)
            roleMenu.gameObject.SetActive(false);
        else
            roleMenu.gameObject.SetActive(true);
    }

    public void UpdateView(object sender, EventArgs e)
    {
        if (roleInfoPanel != null)
        {
            Destroy(roleInfoPanel);
        }
        if (BattlePrepareView.isInit && BattlePrepareView.GetInstance().character.GetComponent<CharacterStatus>().playerNumber == Global.playerNumber)
        {
            gameObject.SetActive(true);
            CreateBaseInfo(BattlePrepareView.GetInstance().character);
        }
        else {
            gameObject.SetActive(false);
            
            roleInfoPanel = CreateRoleInfoPanel(BattlePrepareView.GetInstance().character);
        }
            
        if (skillMenu.gameObject.activeSelf)
            skillMenu.GetComponent<SkillMenu>().UpdateView();
        if (itemMenu_Role.gameObject.activeSelf)
            itemMenu_Role.GetComponent<ItemMenu_Role>().UpdateView();
        if(roleInfo.gameObject.activeSelf)
            roleInfo.GetComponent<RoleInfo>().UpdateView();
    }
    
    public void CreateBaseInfo(Transform character)
    {
        var DB = Global.characterDataList.Find(d => d.roleEName == character.GetComponent<CharacterStatus>().roleEName);
        var characterInfo = CharacterInfoDictionary.GetParam(character.GetComponent<CharacterStatus>().characterInfoID);
        roleName.text = DB.roleCName;
        roleLevel.text = "Lv " + DB.attributes.Find(d => d.eName == "lev").Value.ToString();
        roleSkillPointInfo.text = DB.attributes.Find(d => d.eName == "skp").Value.ToString();
        var currentHP = DB.attributes.Find(d => d.eName == "hp").Value;
        var currentMP = DB.attributes.Find(d => d.eName == "mp").Value;
        info.text = currentHP + "\n" + currentMP;
        experience.maxValue = DB.attributes.Find(d => d.eName == "exp").ValueMax;
        experience.value = DB.attributes.Find(d => d.eName == "exp").Value;
    }

    public GameObject CreateRoleInfoPanel(Transform character)
    {
        //GameObject roleInfoPanel = GameObject.Find("Canvas")?.transform.Find("RoleInfoPanel(Clone)")?.gameObject;
        //if(roleInfoPanel == null)
        GameObject roleInfoPanel = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/UI/RoleInfoPanel"), GameObject.Find("Canvas").transform);

        var roleName = roleInfoPanel.transform.Find("Content").Find("RoleName");
        var roleIdentity = roleInfoPanel.transform.Find("Content").Find("RoleIdentity");
        var roleState = roleInfoPanel.transform.Find("Content").Find("RoleState");
        var healthSlider = roleInfoPanel.transform.Find("Content").Find("Health");
        var chakraSlider = roleInfoPanel.transform.Find("Content").Find("Chakra");
        var info = roleInfoPanel.transform.Find("Content").Find("Info");

        var DB = Global.characterDataList.Find(d => d.roleEName == character.GetComponent<CharacterStatus>().roleEName);

        roleName.GetComponent<Text>().text = character.GetComponent<CharacterStatus>().roleCName.Replace(" ", "");
        roleIdentity.GetComponent<Text>().text = character.GetComponent<CharacterStatus>().identity;
        roleState.GetComponent<Text>().text = character.GetComponent<Unit>().UnitEnd ? "结束" : "待机";
        roleState.GetComponent<Text>().color = character.GetComponent<Unit>().UnitEnd ? Utils_Color.redTextColor : Utils_Color.purpleTextColor;
        healthSlider.GetComponent<Slider>().maxValue = DB.attributes.Find(d => d.eName == "hp").ValueMax;
        healthSlider.GetComponent<Slider>().value = DB.attributes.Find(d => d.eName == "hp").Value;
        chakraSlider.GetComponent<Slider>().maxValue = DB.attributes.Find(d => d.eName == "mp").ValueMax;
        chakraSlider.GetComponent<Slider>().value = DB.attributes.Find(d => d.eName == "mp").Value;
        info.GetComponent<Text>().text = healthSlider.GetComponent<Slider>().value + "\n" + chakraSlider.GetComponent<Slider>().value;

        return roleInfoPanel;
    }

    public void Clear(object sender, EventArgs e)
    {
        transform.Find("RoleMenu").gameObject.SetActive(false);
        gameObject.SetActive(false);
        if (roleInfoPanel)
            Destroy(roleInfoPanel);
    }
}
