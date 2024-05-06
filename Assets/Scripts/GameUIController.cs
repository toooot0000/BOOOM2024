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

    private void Awake(){
        
        GameController.Shared.PlayerPointChanged += (index, point, combo) => {
            scores[index].ShowNumber(point);
        };

        GameController.Shared.PlayerNextBlockUpdated += (i, type) => {
            nextBlock[i].sprite = (i == 0 ? blueBlocks : redBlocks).First(p => p.type == type).spr;
        };
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
        }

        scores[0].ShowNumber(0);
        scores[1].ShowNumber(0);
        
    }
}