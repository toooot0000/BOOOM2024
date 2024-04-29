using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataScript : MonoBehaviour
{
    //蓝方得分
    public long blueScore= 0;
    //红方得分
    public long redScore = 0;
    //全局基础方块下落速度
    public int speed;
    //蓝方方块下落速度
    public int blueSpeed;
    //红方方块下落速度
    public int redSpeed;
    //当前局数 第几局
    public int now=1;
    //历史成绩   胜者 0  蓝方  1 红方
    public List<int> winner;
    //单例
    public static GameDataScript data;
    //0暂停  1游戏中 2结算
    public int gameStatus = 0;

    public RoleAttr blueRole;
    public RoleAttr redRole;
    void Awake()
    {
        if (data == null) { data = this; DontDestroyOnLoad(gameObject); }
        else if (data != this) { Destroy(gameObject); }
    }


}

//角色基础信息
public class RoleAttr
{
    public string name;
    public string skill;
    public Sprite sprite;

}