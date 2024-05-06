using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;
using static Unity.Burst.Intrinsics.X86.Avx;

public class RoleScript : MonoBehaviour
{
    public List<SkeletonDataAsset> roleAssets;
    SkeletonAnimation blueAnimation; // 指向Spine动画的引用
    SkeletonAnimation redAnimation; // 指向Spine动画的引用
    List<RoleInfo> roleInfos { get; set; }
    public List<Image> roleList;
    public Transform imgParent;
    public Sprite blueStatusSprite;
    public Sprite redStatusSprite;
    public Sprite allSprite;
    GameObject blueStatusImg;
    GameObject redStatusImg;
    GameObject blueSelected;
    GameObject redSelected;
    int blueSeek=0;
    int redSeek=1;
    //0选择 1选中
    int buleStatus = 0;
    int redStatus = 0;
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {

        GameObject tmp = GameObject.Find("blueRoleAnimation");
        blueAnimation=tmp.GetComponent<SkeletonAnimation>();
        GameObject tmp2 = GameObject.Find("redRoleAnimation");
        redAnimation = tmp2.GetComponent<SkeletonAnimation>();
        blueSelected = GameObject.Find("blueSelected");
        blueSelected.SetActive(false);
        redSelected = GameObject.Find("redSelected");
        redSelected.SetActive(false);
        RoleInfo role1 = new RoleInfo("空手道鲨鱼", "消除后，对方10秒内的得分翻倍");
        RoleInfo role2 = new RoleInfo("异闻录浣熊", "消除后，10秒内自己无法进行旋转操作");
        RoleInfo role3 = new RoleInfo("蓬蓬蝠", "消除后，对方接下来的5次方块均变为长条");
        roleInfos = new List<RoleInfo>
        {
            role1,
            role2,
            role3
        };
        updateSelect();
        updateRoleInfo();
    }
    // 信息赋值
    void updateRoleInfo()
    {
        RoleInfo blueRoleInfo = roleInfos[blueSeek];
        GameObject blue1 = GameObject.Find("blueName");
        blue1.GetComponent<TextMeshProUGUI>().text= blueRoleInfo.name;
        GameObject blue2 = GameObject.Find("blueSkillText");
        blue2.GetComponent<TextMeshProUGUI>().text = blueRoleInfo.skillText;

        RoleInfo redRoleInfo = roleInfos[redSeek];
        GameObject red1 = GameObject.Find("redName");
        red1.GetComponent<TextMeshProUGUI>().text = redRoleInfo.name;
        GameObject red2 = GameObject.Find("redSkillText");
        red2.GetComponent<TextMeshProUGUI>().text = redRoleInfo.skillText;
    }
    // 选中状态修改
    void updateSelect()
    {
        Destroy(blueStatusImg);
        Image blueRole = roleList[blueSeek];
        blueStatusImg = new GameObject("imgParent" + blueSeek);
        blueStatusImg.transform.SetParent(imgParent, false);
        // 添加Image组件
        Image blueImage = blueStatusImg.AddComponent<Image>();
        RectTransform blueTransform = blueStatusImg.GetComponent<RectTransform>();
        blueTransform.sizeDelta = new Vector2(236, 134);// 设置大小
        blueTransform.position = new Vector2(blueRole.transform.position.x-5, blueRole.transform.position.y);  // 设置RectTransform，调整位置
        // 确保有一个Sprite或Texture，这里以Sprite为例
        blueImage.sprite = blueStatusSprite;

        Destroy(redStatusImg);
        Image redRole = roleList[redSeek];
        redStatusImg = new GameObject("redImgParent" + redSeek);
        redStatusImg.transform.SetParent(imgParent, false);
        // 添加Image组件
        Image redImage = redStatusImg.AddComponent<Image>();
        RectTransform redTransform = redStatusImg.GetComponent<RectTransform>();
        redTransform.sizeDelta = new Vector2(236, 134);// 设置大小
        redTransform.position = new Vector2(redRole.transform.position.x - 5, redRole.transform.position.y);  // 设置RectTransform，调整位置
        // 确保有一个Sprite或Texture，这里以Sprite为例
        redImage.sprite = redStatusSprite;

        if (blueSeek == redSeek)
        {
            redImage.sprite = allSprite;
            blueImage.sprite = allSprite;
        }
    }
    void updateBlueAnimation()
    {
        blueAnimation.ClearState();
        blueAnimation.skeletonDataAsset = roleAssets[blueSeek];
        blueAnimation.Initialize(true);
        blueAnimation.transform.rotation = new Quaternion(0, 0, 0, 0);
        if (blueSeek == 1)
            blueAnimation.transform.rotation = new Quaternion(0, 180, 0, 0);
        blueAnimation.state.SetAnimation(0, "idle", true);
    }
    void updateRedAnimation()
    {
        redAnimation.ClearState();
        redAnimation.skeletonDataAsset = roleAssets[redSeek];
        redAnimation.Initialize(true);
        redAnimation.transform.rotation = new Quaternion(0, 0, 0, 0);
        if (redSeek != 1)
            redAnimation.transform.rotation = new Quaternion(0, 180, 0, 0);
        redAnimation.state.SetAnimation(0, "idle", true);
    }
    // Update is called once per frame
    void Update()
    {
        if (buleStatus==0)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                blueSeek--;
                if (blueSeek < 0)
                {
                    blueSeek = 1;
                }
                updateBlueAnimation();
                updateSelect();
                updateRoleInfo();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                blueSeek++;
                if (blueSeek >1 )
                {
                    blueSeek = 0;
                }
                updateBlueAnimation();
                updateSelect();
                updateRoleInfo();
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (buleStatus == 1)
            {
                buleStatus = 0;
                blueSelected.SetActive(false);
            }
            else
            {
                buleStatus = 1;
                blueSelected.SetActive(true);
            }
        }
        if (redStatus == 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                redSeek--;
                if (redSeek < 0)
                {
                    redSeek = 1;
                }
                updateRedAnimation();
                updateSelect();
                updateRoleInfo();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                redSeek++;
                if (redSeek > 1)
                {
                    redSeek = 0;
                }
                updateRedAnimation();
                updateSelect();
                updateRoleInfo();
            }
        }
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            if (redStatus == 1)
            {
                redStatus = 0;
                redSelected.SetActive(false);
            }
            else
            {
                redStatus = 1;
                redSelected.SetActive(true);
            }
        }
        if(redStatus==1&& buleStatus == 1){

            SideEffectType TypeOfIndex(int i) => i switch{
                0 => SideEffectType.DoublePoints,
                1 => SideEffectType.CantSpin,
                _ => SideEffectType.Length
            };

            String NameOfIndex(int i) => i switch{
                0 => "空手道鲨鱼",
                1 => "异闻录浣熊",
                _ => throw new ArgumentOutOfRangeException(nameof(i), i, null)
            };

            var selectionBlue = new PlayerSelection(){
                name = NameOfIndex(blueSeek),
                img = roleList[blueSeek].sprite,
                skeleton = roleAssets[blueSeek],
                type = TypeOfIndex(blueSeek),
                needFlipSkeleton = blueSeek == 1
            };
            var selectionRed = new PlayerSelection(){
                name = NameOfIndex(redSeek),
                img = roleList[redSeek].sprite,
                skeleton = roleAssets[redSeek],
                type = TypeOfIndex(redSeek),
                needFlipSkeleton = redSeek != 1
            };

            GameDataScript.data.playerSelections = new[]{ selectionBlue, selectionRed };
            
            SceneManager.LoadScene("GameScene");
        }
    }
}
