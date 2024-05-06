using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

[Serializable]
public struct PlayerSelection{
    public String name;
    public Sprite img;
    public SkeletonDataAsset skeleton;
    public bool needFlipSkeleton;
    public SideEffectType type;
}

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
    
    //��ɫѡ����
    public PlayerSelection[] playerSelections = new PlayerSelection[2];
    
    void Awake()
    {
        if (data == null) { data = this; DontDestroyOnLoad(gameObject); }
        else if (data != this) { Destroy(gameObject); }
    }

}