using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour{
    public int number;
    public Sprite[] numbers;

    public Transform start;
    public float width;

    private readonly List<Image> _images = new(); 

    public void ShowNumber(int num){
        number = num;
        var digits = CastScore(num);
        var pos = Vector3.zero;
        var i = 0;
        for (; i < digits.Length; i++){
            if (i >= _images.Count){
                var obj = new GameObject($"_image_{i}");
                obj.transform.SetParent(transform);
                _images.Add(obj.AddComponent<Image>());
            }
            var img = _images[i];
            img.enabled = true;
            img.sprite = numbers[digits[i]];
            ((RectTransform)img.transform).anchoredPosition = pos;
            ((RectTransform)img.transform).sizeDelta = new( digits[i] == 1 ? 30 : 41, 69);
            pos += Vector3.right * width;
        }

        for (;i < _images.Count;i++){
            _images[i].enabled = false;
        }
    }

    private int[] CastScore(int num){
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