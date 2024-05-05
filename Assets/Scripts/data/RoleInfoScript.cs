using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleInfoScript : MonoBehaviour
{
    public List<RoleInfo> roleInfos { get; set; }

    public static RoleInfoScript data;

    void Awake()
    {
        RoleInfo role1 = new RoleInfo("空手道鲨鱼", "消除后，对方10秒内的得分翻倍");
        RoleInfo role2 = new RoleInfo("怪盗卡牌猫", "消除后，10秒内自己无法进行旋转操作");
        RoleInfo role3 = new RoleInfo("蓬蓬蝠", "消除后，对方接下来的5次方块均变为长条");
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