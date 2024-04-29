using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class gameInit : MonoBehaviour
{
    public TextMeshProUGUI blueRoleName;
    public TextMeshProUGUI redRoleName;
    public List<Sprite> numCdSprites;
    public List<Image> numCdImgs;
    public int second = 30;
    private static int OneHour = 3600;
    private static int OneMininue = 60;
    // Start is called before the first frame update
    void Start()
    {
        GameDataScript.data.gameStatus = 1;
        InvokeRepeating("CountDown", 1f, 1f);
/*        Dictionary<string, object> param = SceneMgr.ins.ReadSceneData();
        roleName1.text = param["roleName1"].ToString();
        roleName2.text = param["roleName2"].ToString();*/

    }

    // Update is called once per frame
    void Update()
    {
    }

    // 倒计时方法(通过InvokeRepeating(调用的方法，第一次调用的时间，调用一次的时间间隔)在Start中重复调用)
    // 适用于固定时间重复执行
    private void CountDown()
    {
        if (GameDataScript.data.gameStatus != 1)
            return;
        //则倒计时减一秒
        second--;
        if (second <= 0)
        {
            GameDataScript.data.gameStatus = 2;
        }
        string time = FormatSToHMS(second);
        for (int i = 0; i < numCdImgs.Count; i++)
        {
            
            int seek = Convert.ToInt32(time[i]+"");
            numCdImgs[i].sprite = numCdSprites[seek];
        }
        //当时间小于等于0时  停止InvokeRepeating的重复调用
        //if (second <= 0)
        //CancelInvoke("CountDown");
    }
    void changeCd()
    {

    }
    //把秒变成时:分:秒
    public static string FormatSToHMS(int _time)
    {
        int _hour = _time / OneHour;
        int _min = 0;
        int _sec = 0;
        if (_hour > 0)
        {
            _min = (_time % OneHour) / OneMininue;
            _sec = _min > 0 ? (_time % OneHour) % OneMininue : _time % OneHour;
        }
        else
        {
            _min = _time / OneMininue;
            _sec = _min > 0 ? _time % OneMininue : _time;
        }

        return string.Format("{0:00}{1:00}", _min, _sec);
    }
}
