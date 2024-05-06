using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour{
    public int number;
    public Sprite[] numbers;

    public Transform start;
    public float width;

    protected readonly List<Image> Images = new(); 

    public virtual void ShowNumber(int num){
        number = num;
        var digits = CastScore(num);
        if (digits.Length == 0 && num == 0){
            digits = new[]{ 0 };
        }
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
            img.sprite = numbers[digits[i]];
            ((RectTransform)img.transform).anchoredPosition = pos;
            ((RectTransform)img.transform).sizeDelta = new( digits[i] == 1 ? 30 : 41, 69);
            pos += Vector3.right * width;
        }

        for (;i < Images.Count;i++){
            Images[i].enabled = false;
        }
    }

    protected int[] CastScore(int num){
        var list = ListPool<int>.Get();
        while (num>0){
            list.Add(num % 10);
            num = num / 10;
        }

        if (width > 0){
            list.Reverse();   
        }
        var ret = list.ToArray();
        ListPool<int>.Release(list);
        return ret;
    }
}