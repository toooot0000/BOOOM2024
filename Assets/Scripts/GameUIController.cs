using System;
using System.Collections.Generic;
using System.Linq;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(1)]
public class GameUIController: MonoBehaviour{
    
    [Serializable]
    public struct TetrisTypePair{
        public TetrisType type;
        public Sprite spr;
    }
    
    public ScoreDisplay[] scores;
    public SkeletonAnimation[] skeletonAnimations;
    
    public Image[] nextBlock;
    public TetrisTypePair[] redBlocks;
    public TetrisTypePair[] blueBlocks;

    public TextMeshProUGUI[] names;
    public TextMeshProUGUI[] seDesc;

    public TimeDisplay timeDisplay;

    private void Awake(){
        
        GameController.Shared.PlayerPointChanged += (index, point, combo) => {
            scores[index].ShowNumber(point);
        };

        GameController.Shared.PlayerNextBlockUpdated += (i, type) => {
            nextBlock[i].sprite = (i == 0 ? blueBlocks : redBlocks).First(p => p.type == type).spr;
        };

        GameController.Shared.PlayerOverflow += playerIndices => { };

        GameController.Shared.PlayerSideEffectsUpdated += (i, effects) => { };
    }

    private void Start(){

        var selections = GameDataScript.data.playerSelections;

        foreach (var (i, s) in selections.Enumerated()){
            var sklAnim = skeletonAnimations[i];
            sklAnim.ClearState();
            sklAnim.skeletonDataAsset = s.skeleton;
            sklAnim.Initialize(true);
            sklAnim.transform.rotation = Quaternion.identity;
            if (s.needFlipSkeleton){
                sklAnim.transform.rotation = new Quaternion(0, 180, 0, 0);
            }

            names[i].text = s.name;

            seDesc[i].text = s.type switch{
                SideEffectType.DoublePoints => "消除后，对方10秒内的得分翻倍",
                SideEffectType.CantSpin => "消除后，10秒内自己无法进行旋转操作",
                _ => ""
            };
        }

        scores[0].ShowNumber(0);
        scores[1].ShowNumber(0);
        
    }

    private void Update(){
        var time = Mathf.RoundToInt(GameController.Shared.RemainingTime);
        timeDisplay.ShowNumber(time);
    }
    
    
}