using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleInfoScript : MonoBehaviour
{
    public List<RoleInfo> roleInfos { get; set; }

    public static RoleInfoScript data;

    void Awake()
    {
        RoleInfo role1 = new RoleInfo("���ֵ�����", "�����󣬶Է�10���ڵĵ÷ַ���");
        RoleInfo role2 = new RoleInfo("�ֵ�����è", "������10�����Լ��޷�������ת����");
        RoleInfo role3 = new RoleInfo("������", "�����󣬶Է���������5�η������Ϊ����");
        roleInfos = new List<RoleInfo>
        {
            role1,
            role2,
            role3
        };
        if (data == null) { data = this; DontDestroyOnLoad(gameObject); }
        else if (data != this) { Destroy(gameObject); }
    }
}
public class RoleInfo {

        public RoleInfo()
{
}
public RoleInfo(string name, string skillText)
    {
        this.name = name;
        this.skillText = skillText;
    }

    public int id { get; set; }
    public string name { get; set; }
    public string skillText { get; set; }

}