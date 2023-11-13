using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterBattleGameScript : MonoBehaviour
{
    public Sprite[] sprites;

    public GameObject StartPase;
    public GameObject BattlePase;

    public static MonsterBattleGameScript Instance = null;

    public Sprite[] SpritesMonster;

    public InputField InputFieldHP;
    public InputField InputFieldAtt;

    public Toggle[] SelectToggles;

    public Text SelectedMonsterNameInfo;
    string SelectedMonsterName;
    public string[] names { get; private set; } = new string[3] { "Worm", "Wolf", "Skull" };
    public Image SelectedMonsterImg;
    public int SelectedMonsterHP = 0;
    public int SelectedMonsterAtt = 0;
    public Button ButtonGameStart;

    public Monster MyMonster = null;

    public Image EnemyMonsterImg;
    public Monster EnemyMonster = null;

    private void Awake()
    {
        #region 싱글톤 켜기
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        #endregion
                
        BattlePase.gameObject.SetActive(false);
        StartPase.gameObject.SetActive(true);
        ToggleSelected();
    }

    private void Update()
    {
        if (MyMonster.HP < 0)
        {
            MyMonster.HP = 0;
        }
        if (EnemyMonster.HP < 0)
        {
            EnemyMonster.HP = 0;
        }
    }

    public void ToggleSelected()
    {
        for (int i = 0; i < SelectToggles.Length; i++)
        {
            if (SelectToggles[i].isOn)
            {
                SelectedMonsterImg.sprite = SpritesMonster[i];
                SelectedMonsterNameInfo.text = $"이름  : {names[i]}";
                SelectedMonsterName = names[i];
            }
        }        
    }
    public void GameStartButtonClicked()
    {
        int.TryParse(InputFieldHP.text, out SelectedMonsterHP);
        int.TryParse(InputFieldAtt.text, out SelectedMonsterAtt);
        EnemyMonsterImg.sprite = sprites[Random.Range(0, sprites.Length)];

        MyMonster = new Monster(SelectedMonsterImg, SelectedMonsterName, SelectedMonsterHP, SelectedMonsterAtt);
        EnemyMonster = new Monster(EnemyMonsterImg, names[Random.Range(0, 3)], Random.Range(SelectedMonsterHP-10, SelectedMonsterHP-5), Random.Range(SelectedMonsterAtt-5, SelectedMonsterAtt-2));

        StartPase.gameObject.SetActive(false);
        BattlePase.gameObject.SetActive(true);

    }

    //================================================================

    //public Dropdown dropdown;
    //public Transform ContentsTr;
    //Dictionary<int, List<Image>> SearchDic = new Dictionary<int, List<Image>>();
    //    //{ 
    //    //    { 0, new List<Image> { Instance.ContentsTr.GetChild(0).GetComponent<Image>(), Instance.ContentsTr.GetChild(2).GetComponent<Image>() } },
    //    //    { 1, new List<Image> { Instance.ContentsTr.GetChild(1).GetComponent<Image>(), Instance.ContentsTr.GetChild(3).GetComponent<Image>() } },
    //    //    { 2, new List<Image> { Instance.ContentsTr.GetChild(4).GetComponent<Image>(), Instance.ContentsTr.GetChild(5).GetComponent<Image>() } },
    //    //};
    //public void DropDown()
    //{
    //    int index = dropdown.value;
    //    //for (int i = 0; i < SearchDic.Count; i++)
    //    //{
    //    //    for (int j = 0; j < SearchDic[i].Count; j++)
    //    //    {
    //    //        if (i == index)
    //    //        {
    //    //            SearchDic[i][j].gameObject.SetActive(true);
    //    //        }
    //    //        else
    //    //        {
    //    //            SearchDic[i][j].gameObject.SetActive(false);
    //    //        }
    //    //    }
    //    //}
    //}

    public class Monster
    {
        public Image Img;
        public string Name = "";
        public int HP = 0;
        public int MaxHP = 0;
        public int Att = 0;
        public bool IsAlive => HP > 0;//HP가 0보다 크면 살아있음

        public Monster(Image image, string name, int hp, int att)
        {
            Img = image;
            Name = name;
            MaxHP = hp;
            HP = MaxHP;
            Att = att;
        }
    }
}


