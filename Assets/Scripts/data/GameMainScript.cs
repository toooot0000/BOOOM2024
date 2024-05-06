using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GameMainScript : MonoBehaviour
{

    public TextMeshProUGUI gameSpeed;
    public int second = 30;
    public List<Sprite> blueScoreSprites;
    public List<Image> blueScoreImgs;
/*    public List<Sprite> redScoreSprites;
    public List<Image> redScoreImgs;*/
    private static int OneHour = 3600;
    private static int OneMininue = 60;
    private static int tmp = 0;
    public Canvas canvas;
    /*    public TextMeshProUGUI roleName1;
        public TextMeshProUGUI roleName2;
        public List<Sprite> numCdSprites;
        public List<Image> numCdImgs;
    // Start is called before the first frame update*/
    void Start()
    {

        //GameDataScript.data.gameStatus = 1;
        InvokeRepeating("UpdateScore", 1f, 1f);
        /*        Dictionary<string, object> param = SceneMgr.ins.ReadSceneData();
                roleName1.text = param["roleName1"].ToString();
                roleName2.text = param["roleName2"].ToString();*/

    }
    //分数测试
    void UpdateScore()
    {
        // System.Random r = new System.Random();
        // GameDataScript.data.speed = r.Next(9);
        // GameDataScript.data.blueScore = r.Next(9999999);
        // GameObject[] objects = GameObject.FindGameObjectsWithTag("score");
        // foreach (GameObject obj in objects)
        // {
        //     Destroy(obj);
        // }
        // Vector2 position = new Vector2(-740, -232);
        // int[] result = CastScore(GameDataScript.data.blueScore.ToString());
        //
        // for (int i = 0; i < result.Length; i++)
        // {
        //     
        //     GameObject imageContainer = new GameObject("scoreImage" + second);
        //     imageContainer.tag = "score";
        //     imageContainer.transform.SetParent(canvas.transform, false);
        //     Image dynamicImage = imageContainer.AddComponent<Image>();
        //     RectTransform rectTransform = imageContainer.GetComponent<RectTransform>();
        //     rectTransform.sizeDelta = new Vector2(41, 69);
        //     rectTransform.anchoredPosition = new Vector2(-740 + i * 50, -232);
        //     dynamicImage.sprite = blueScoreSprites[result[i]];
        // }
    }
    // Update is called once per frame
    void Update()
    {
        string speedStr="SPEED:"+GameDataScript.data.speed;
        gameSpeed.text = speedStr;
    }
    int[] CastScore(String scoreStr)
    {
        int[] result = new int[scoreStr.Length];
        for (int i = 0; i < scoreStr.Length; i++)
        {
            result[i] = Convert.ToInt32(scoreStr[i]+"");
            Debug.Log(result[i] + "====="+ scoreStr[i]);
        }
        return result;
    }
    // 倒计时方法(通过InvokeRepeating(调用的方法，第一次调用的时间，调用一次的时间间隔)在Start中重复调用)
    // 适用于固定时间重复执行
    private void CountDown()
    {
        if (GameDataScript.data.gameStatus != 1)
            return;
        //则倒计时减一秒
        second--;
        tmp++;
        if (second <= 0)
        {
            GameDataScript.data.gameStatus = 2;
        }
        string time = FormatSToHMS(second);
        GameObject imageContainer = new GameObject("Image"+ second);
        imageContainer.transform.SetParent(canvas.transform, false);
        // 添加Image组件
        Image dynamicImage = imageContainer.AddComponent<Image>();
        RectTransform rectTransform = imageContainer.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(41, 69);// 设置大小
        rectTransform.anchoredPosition = new Vector2(-401 - tmp*50, -232);  // 设置RectTransform，调整位置
        // 确保有一个Sprite或Texture，这里以Sprite为例
        dynamicImage.sprite = blueScoreSprites[2];
        
        
        /*        for (int i = 0; i < blueScoreImgs.Count; i++)
                {

                    int seek = Convert.ToInt32(time[i] + "");
                    Debug.Log(seek + "-----");
                    blueScoreImgs[i].sprite = blueScoreSprites[seek];
                }*/
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
