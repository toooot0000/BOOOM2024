using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    public Button button;
    public Image blueResult1;
    public Image blueResult2;
    public Image blueResult3;
    public Image redResult1;
    public Image redResult2;
    public Image redResult3;
    public Sprite[] sprites;
    private void Start()
    {
        Debug.Log("脚本事件绑定");
        button.onClick.AddListener(OnButtonClick);
    }
    public void OnButtonClick()
    {
        Debug.Log("脚本事件chufa");
        Image blueResult= blueResult1;
        Image redResult= redResult1;
        switch (GameDataScript.data.now)
        {
            case 2:
                blueResult = blueResult2;
                redResult = redResult2;
                break;
            case 3:
                blueResult = blueResult3;
                redResult = redResult3;
                break;
        }
        blueResult.sprite = sprites[2];
        redResult.sprite = sprites[1];
        GameDataScript.data.now++;
    }
    
}
