using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataScript : MonoBehaviour
{
    //�����÷�
    public long blueScore= 0;
    //�췽�÷�
    public long redScore = 0;
    //ȫ�ֻ������������ٶ�
    public int speed;
    //�������������ٶ�
    public int blueSpeed;
    //�췽���������ٶ�
    public int redSpeed;
    //��ǰ���� �ڼ���
    public int now=1;
    //��ʷ�ɼ�   ʤ�� 0  ����  1 �췽
    public List<int> winner;
    //����
    public static GameDataScript data;
    //0��ͣ  1��Ϸ�� 2����
    public int gameStatus = 0;

    void Awake()
    {
        if (data == null) { data = this; DontDestroyOnLoad(gameObject); }
        else if (data != this) { Destroy(gameObject); }
    }

}