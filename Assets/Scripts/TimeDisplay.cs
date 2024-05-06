using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TimeDisplay: ScoreDisplay{

    public Sprite colon;

    public override void ShowNumber(int timeInSec){
        var digits = CastTime(timeInSec);
        var pos = Vector3.zero;
        var i = 0;
        for (; i < digits.Length; i++){
            if (i >= Images.Count){
                var obj = new GameObject($"_image_{i}");
                obj.transform.SetParent(transform);
                Images.Add(obj.AddComponent<Image>());
            }
            var img = Images[i];
            img.enabled = true;
            img.sprite = digits[i] == -1 ? colon : numbers[digits[i]];
            ((RectTransform)img.transform).anchoredPosition = pos;
            ((RectTransform)img.transform).sizeDelta = new( digits[i] == 1 ? 30 : 41, 69);
            pos += Vector3.right * width;
        }

        for (;i < Images.Count;i++){
            Images[i].enabled = false;
        }
    }

    protected int[] CastTime(int timeInSec){
        var min = timeInSec / 60;
        var sec = timeInSec % 60;
        var minDig = ClampTo2Digits(CastScore(min));
        var secDig = ClampTo2Digits(CastScore(sec));
        return minDig.Concat(new []{-1}).Concat(secDig).ToArray();
    }

    private int[] ClampTo2Digits(int[] digits){
        return new int[2]{
            digits.Length > 1 ? digits[^2] : 0,
            digits.Length > 0 ? digits[^1] : 0,
        };
    }
}