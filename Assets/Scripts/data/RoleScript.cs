using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoleScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("roleName1", "���ֵ�����");
            param.Add("roleName2", "����С����");
            SceneMgr.ins.ToNewScene("GameScene", param);
        }
    }
}
