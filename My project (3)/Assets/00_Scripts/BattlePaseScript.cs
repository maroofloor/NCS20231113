using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePaseScript : MonoBehaviour
{
    public GameObject GameoverPase;
    public Text GameoverTxt;
    public Button ButtonRestart;

    // 내 몬스터
    public Image MyImg; // 이미지
    public Slider MyHPSlider; // HP슬라이더
    public Text MyHPTxt; // HP슬라이더 위에 표시되는 HP숫자
    public Text MyMonsterTxt; // 몬스터 이름, 공격력

    //적 몬스터
    public Image Enemyimg; // 이미지
    public Slider EnemyHPSlider; // HP슬라이더
    public Text EnemyHPTxt;// HP슬라이더 위에 표시되는 HP숫자
    public Text EnemyMonsterTxt;// 몬스터 이름, 공격력

    //스킬 쿨타임 관련
    public Image[] SkillCooldownImgArr; // 스킬 쿨다운 이미지 배열
    public Text[] SkillCoolTxtArr; // 스킬 쿨다운 이미지 위에 표시되는 숫자 " 1.2초" 면 " 1.2s " 형식
    float[] SkillCoolArr = new float[3] { 0f, 0f, 0f }; // 실제로 돌고있는 스킬쿨타임
    Dictionary<int, float> SkillCoolDic = new Dictionary<int, float>(3) { { 0, 1.5f }, { 1, 2f }, { 2, 2f } }; // 해당스킬에 적용 될 쿨타임
    Coroutine[] SkillCoolcorArr = new Coroutine[3] { null, null, null }; // 스킬쿨다운 코루틴을 담을 코루틴배열...

    int WinCount = 0; // 잡은 몬스터 수

    public void Start()
    {
        GameoverPase.gameObject.SetActive(false);

        SkillCoolClear(); // 스킬 쿨 관련 정리...
        SkillCoolArr[2] = SkillCoolDic[2];
        SkillCooldownImgArr[2].gameObject.SetActive(true);
        if (SkillCoolcorArr[2] == null)
            SkillCoolcorArr[2] = StartCoroutine(SkillCooldown(2));

        //이미지를 몬스터클래스에서 가져옮
        MyImg.sprite = MonsterBattleGameScript.Instance.MyMonster.Img.sprite;
        Enemyimg.sprite = MonsterBattleGameScript.Instance.EnemyMonster.Img.sprite;

        //이름과 공격력 텍스트를 몬스터 클래스에서 가져옮
        MyMonsterTxt.text = $"이  름 : {MonsterBattleGameScript.Instance.MyMonster.Name} \n공격력: {MonsterBattleGameScript.Instance.MyMonster.Att}";
        EnemyMonsterTxt.text = $"이  름 : {MonsterBattleGameScript.Instance.EnemyMonster.Name} \n공격력: {MonsterBattleGameScript.Instance.EnemyMonster.Att}";

        //최대체력을 몬스터 클래스에서 가져와 체력바에 세팅
        EnemyHPSlider.maxValue = MonsterBattleGameScript.Instance.EnemyMonster.MaxHP;
        MyHPSlider.maxValue = MonsterBattleGameScript.Instance.MyMonster.MaxHP;
    }

    public void Update()
    {
        #region UI 세팅...

        // 내몬스터와 적몬스터의 체력과 최대 체력을 몬스터클래스로 부터 가져와서 체력바와 체력바 위에 텍스트에 세팅
        MyHPSlider.value = MonsterBattleGameScript.Instance.MyMonster.HP;
        MyHPTxt.text = $"{MonsterBattleGameScript.Instance.MyMonster.HP} / {MonsterBattleGameScript.Instance.MyMonster.MaxHP}";
        EnemyHPSlider.value = MonsterBattleGameScript.Instance.EnemyMonster.HP;
        EnemyHPTxt.text = $"{MonsterBattleGameScript.Instance.EnemyMonster.HP} / {MonsterBattleGameScript.Instance.EnemyMonster.MaxHP}";

        #endregion

        AliveCheck(); // 둘 중 하나가 죽었는지 체크하고 죽었으면 게임오버 or 이어하기

        SkillCoolClear(); // 스킬 쿨 관련 정리...

        #region 키 입력시 스킬 발동함수 실행...
        if (Input.GetKeyDown(KeyCode.Alpha1)) // 숫자 버튼 1 을 눌러서 공격하기
            UseSkill(0);

        if (Input.GetKeyDown(KeyCode.Alpha2)) // 숫자 버튼 2 를 눌러서 방어하기
            UseSkill(1);

        if (SkillCoolArr[2] <= 0f) //적이 공격 쿨타임이 될 때 마다 공격함
            UseSkill(2); ;
        #endregion
    }

    public void RestartButtonClicked()
    {
        WinCount++;
        MonsterBattleGameScript.Instance.EnemyMonster = new MonsterBattleGameScript.Monster
            (
            MonsterBattleGameScript.Instance.EnemyMonsterImg,
            MonsterBattleGameScript.Instance.names[Random.Range(0, 3)], 
            Random.Range(MonsterBattleGameScript.Instance.SelectedMonsterHP - 10, MonsterBattleGameScript.Instance.SelectedMonsterHP - 5), 
            Random.Range(MonsterBattleGameScript.Instance.SelectedMonsterAtt - 5, MonsterBattleGameScript.Instance.SelectedMonsterAtt - 2)
            );
        MonsterBattleGameScript.Instance.EnemyMonster.Img.sprite = MonsterBattleGameScript.Instance.sprites[Random.Range(0, MonsterBattleGameScript.Instance.sprites.Length)];

        Enemyimg.sprite = MonsterBattleGameScript.Instance.EnemyMonster.Img.sprite;
        EnemyHPSlider.maxValue = MonsterBattleGameScript.Instance.EnemyMonster.MaxHP;
        EnemyMonsterTxt.text = $"이  름 : {MonsterBattleGameScript.Instance.EnemyMonster.Name} \n공격력: {MonsterBattleGameScript.Instance.EnemyMonster.Att}";

        GameoverPase.gameObject.SetActive(false);
    }

    void AliveCheck() // 둘중 하나가 죽었는지 체크... 죽었으면 게임오버창을 띄움...
    {
        if (MonsterBattleGameScript.Instance.MyMonster.IsAlive == false || MonsterBattleGameScript.Instance.EnemyMonster.IsAlive == false) // 둘 중 하나가 죽으면..
        {
            if (MonsterBattleGameScript.Instance.MyMonster.IsAlive)
            {
                GameoverTxt.text = $"축하합니다!\n{MonsterBattleGameScript.Instance.MyMonster.Name}가 이겼습니다!";
            }
            else if (MonsterBattleGameScript.Instance.EnemyMonster.IsAlive)
            {
                GameoverTxt.text = $"아쉽네요,\n{MonsterBattleGameScript.Instance.MyMonster.Name}가 졌습니다...\n{WinCount}마리를 잡았습니다.";
                ButtonRestart.gameObject.SetActive(false); // 플레이어가 죽었으면 이어하기버튼 비활성화...
            }
            else
            {
                GameoverTxt.text = $"둘이 동시에 죽었거나 에러가 발생했습니다.";
                ButtonRestart.gameObject.SetActive(false);
            }

            for (int i = 0; i < SkillCoolArr.Length; i++) // 죽었으면 쿨타임 관련된 것 모두 초기화...
            {
                SkillCoolArr[i] = 0f;
                SkillCooldownImgArr[i].fillAmount = SkillCoolArr[i];
                SkillCoolTxtArr[i].text = string.Format("{0 : 0.0}s", SkillCoolArr[i]);
            }
            SkillCoolClear();

            GameoverPase.gameObject.SetActive(true);
        }
        else
            return;
    }

    void SkillCoolClear() // 스킬 쿨타임 관련된 것들 정리...
    {
        for (int i = 0; i < SkillCoolArr.Length; i++)
        {
            if (SkillCoolArr[i] <= 0f) // 해당 스킬쿨이 0 보다 작거나 같다면...
            {
                SkillCoolArr[i] = 0f; // 0 으로 바꾸고...
                SkillCooldownImgArr[i].gameObject.SetActive(false); // 쿨다운 이미지를 비활성화 한 후...
                if (SkillCoolcorArr[i] != null) // 해당 스킬쿨코루틴이 null 이 아니면...
                {
                    StopCoroutine(SkillCoolcorArr[i]);
                    SkillCoolcorArr[i] = null; // null 로 바꿔라
                }
            }
        }
    }

    void UseSkill(int index)
    {
        if (SkillCoolArr[index] > 0f || MonsterBattleGameScript.Instance.MyMonster.IsAlive == false || MonsterBattleGameScript.Instance.EnemyMonster.IsAlive == false)        
            return;

        if (index == 0)
            MonsterBattleGameScript.Instance.EnemyMonster.HP -= MonsterBattleGameScript.Instance.MyMonster.Att;
        else if (index == 1)
        {
            MonsterBattleGameScript.Instance.MyMonster.MaxHP += 5;
            MyHPSlider.maxValue = MonsterBattleGameScript.Instance.MyMonster.MaxHP;
            MonsterBattleGameScript.Instance.MyMonster.HP += 5;
        }
        else if (index == 2)
            MonsterBattleGameScript.Instance.MyMonster.HP -= MonsterBattleGameScript.Instance.EnemyMonster.Att;

        SkillCoolArr[index] = SkillCoolDic[index];
        SkillCooldownImgArr[index].gameObject.SetActive(true);

        if (SkillCoolcorArr[index] == null)        
            SkillCoolcorArr[index] = StartCoroutine(SkillCooldown(index));
    }

    IEnumerator SkillCooldown(int index)
    {
        float _Cool = SkillCoolDic[index];

        while (SkillCoolArr[index] > 0f)
        {
            SkillCooldownImgArr[index].fillAmount = SkillCoolArr[index] / _Cool;
            yield return new WaitForSeconds(0.01f);
            SkillCoolArr[index] -= 0.01f;
            SkillCoolTxtArr[index].text = string.Format("{0 : 0.0}s", SkillCoolArr[index]);
        }
    }

    #region 간소화 하기 전...
    //void EnemyAttack()
    //{
    //    if (EnemyAttackCoolTime > 0f || MonsterBattleGameScript.Instance.EnemyMonster.IsAlive == false || MonsterBattleGameScript.Instance.MyMonster.IsAlive == false)
    //    {
    //        return;
    //    }
    //    MonsterBattleGameScript.Instance.MyMonster.HP -= MonsterBattleGameScript.Instance.EnemyMonster.Att;
    //    EnemyAttackCoolImg.gameObject.SetActive(true);
    //    EnemyAttackCoolTime = 2f;
    //    if (EnemyAttackcor == null)
    //    {
    //        EnemyAttackcor = StartCoroutine(EnemyAttackCooldown());
    //    }
    //}

    //void UseMyAttack()
    //{
    //    if (MyAttackCoolTime > 0f || MonsterBattleGameScript.Instance.EnemyMonster.IsAlive == false || MonsterBattleGameScript.Instance.MyMonster.IsAlive == false)
    //    {
    //        return;
    //    }
    //    MonsterBattleGameScript.Instance.EnemyMonster.HP -= MonsterBattleGameScript.Instance.MyMonster.Att;
    //    MyAttackCoolImg.gameObject.SetActive(true);
    //    MyAttackCoolTime = 1.5f;
    //    if (MyAttackcor == null)
    //    {
    //        MyAttackcor = StartCoroutine(MyAttackCooldown());
    //    }
    //}
    //void UseMyDefense()
    //{
    //    if (MyDefenseCoolTime > 0f || MonsterBattleGameScript.Instance.EnemyMonster.IsAlive == false || MonsterBattleGameScript.Instance.MyMonster.IsAlive == false)
    //    {
    //        return;
    //    }

    //    MonsterBattleGameScript.Instance.MyMonster.MaxHP += 5;
    //    MonsterBattleGameScript.Instance.MyMonster.HP += 5;
    //    MyDefenseCoolImg.gameObject.SetActive(true);
    //    MyDefenseCoolTime = 2f;
    //    if (MyDefensecor == null)
    //    {
    //        MyDefensecor = StartCoroutine(MyDefenseCooldown());
    //    }
    //}

    //IEnumerator MyAttackCooldown()
    //{
    //    float _cool = MyAttackCoolTime;
    //    while (MyAttackCoolTime > 0f)
    //    {            
    //        MyAttackCoolImg.fillAmount = MyAttackCoolTime / _cool;
    //        yield return new WaitForSeconds(0.01f);
    //        MyAttackCoolTime -= 0.01f;
    //    }
    //}

    //IEnumerator MyDefenseCooldown()
    //{
    //    float _cool = MyDefenseCoolTime;
    //    while (MyDefenseCoolTime > 0f)
    //    {
    //        MyDefenseCoolImg.fillAmount = MyDefenseCoolTime / _cool;
    //        yield return new WaitForSeconds(0.01f);
    //        MyDefenseCoolTime -= 0.01f;
    //    }
    //}

    //IEnumerator EnemyAttackCooldown()
    //{
    //    float _cool = EnemyAttackCoolTime;
    //    while (EnemyAttackCoolTime > 0f)
    //    {
    //        EnemyAttackCoolImg.fillAmount = EnemyAttackCoolTime / _cool;
    //        yield return new WaitForSeconds(0.01f);
    //        EnemyAttackCoolTime -= 0.01f;
    //    }
    //}
    #endregion
}
