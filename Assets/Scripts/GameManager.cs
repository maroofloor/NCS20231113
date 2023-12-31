using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    public Player player;
    [SerializeField]
    Transform mainCanvasTr;
    [SerializeField]
    Image playerHPBar;
    [SerializeField]
    Text potionNumTxt;
    [SerializeField]
    Image RollCoolImg;
    float rollMaxColl = 3f;

    void Update()
    {
        RollCoolImg.fillAmount = player.rollCool / rollMaxColl;
    }
    void FixedUpdate()
    {
        playerHPBar.fillAmount = player.myStat.HP / player.myStat.MaxHP;
    }
}
