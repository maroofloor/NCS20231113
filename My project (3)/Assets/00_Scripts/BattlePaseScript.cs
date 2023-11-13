using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePaseScript : MonoBehaviour
{
    public GameObject GameoverPase;
    public Text GameoverTxt;
    public Button ButtonRestart;

    // �� ����
    public Image MyImg; // �̹���
    public Slider MyHPSlider; // HP�����̴�
    public Text MyHPTxt; // HP�����̴� ���� ǥ�õǴ� HP����
    public Text MyMonsterTxt; // ���� �̸�, ���ݷ�

    //�� ����
    public Image Enemyimg; // �̹���
    public Slider EnemyHPSlider; // HP�����̴�
    public Text EnemyHPTxt;// HP�����̴� ���� ǥ�õǴ� HP����
    public Text EnemyMonsterTxt;// ���� �̸�, ���ݷ�

    //��ų ��Ÿ�� ����
    public Image[] SkillCooldownImgArr; // ��ų ��ٿ� �̹��� �迭
    public Text[] SkillCoolTxtArr; // ��ų ��ٿ� �̹��� ���� ǥ�õǴ� ���� " 1.2��" �� " 1.2s " ����
    float[] SkillCoolArr = new float[3] { 0f, 0f, 0f }; // ������ �����ִ� ��ų��Ÿ��
    Dictionary<int, float> SkillCoolDic = new Dictionary<int, float>(3) { { 0, 1.5f }, { 1, 2f }, { 2, 2f } }; // �ش罺ų�� ���� �� ��Ÿ��
    Coroutine[] SkillCoolcorArr = new Coroutine[3] { null, null, null }; // ��ų��ٿ� �ڷ�ƾ�� ���� �ڷ�ƾ�迭...

    int WinCount = 0; // ���� ���� ��

    public void Start()
    {
        GameoverPase.gameObject.SetActive(false);

        SkillCoolClear(); // ��ų �� ���� ����...
        SkillCoolArr[2] = SkillCoolDic[2];
        SkillCooldownImgArr[2].gameObject.SetActive(true);
        if (SkillCoolcorArr[2] == null)
            SkillCoolcorArr[2] = StartCoroutine(SkillCooldown(2));

        //�̹����� ����Ŭ�������� ������
        MyImg.sprite = MonsterBattleGameScript.Instance.MyMonster.Img.sprite;
        Enemyimg.sprite = MonsterBattleGameScript.Instance.EnemyMonster.Img.sprite;

        //�̸��� ���ݷ� �ؽ�Ʈ�� ���� Ŭ�������� ������
        MyMonsterTxt.text = $"��  �� : {MonsterBattleGameScript.Instance.MyMonster.Name} \n���ݷ�: {MonsterBattleGameScript.Instance.MyMonster.Att}";
        EnemyMonsterTxt.text = $"��  �� : {MonsterBattleGameScript.Instance.EnemyMonster.Name} \n���ݷ�: {MonsterBattleGameScript.Instance.EnemyMonster.Att}";

        //�ִ�ü���� ���� Ŭ�������� ������ ü�¹ٿ� ����
        EnemyHPSlider.maxValue = MonsterBattleGameScript.Instance.EnemyMonster.MaxHP;
        MyHPSlider.maxValue = MonsterBattleGameScript.Instance.MyMonster.MaxHP;
    }

    public void Update()
    {
        #region UI ����...

        // �����Ϳ� �������� ü�°� �ִ� ü���� ����Ŭ������ ���� �����ͼ� ü�¹ٿ� ü�¹� ���� �ؽ�Ʈ�� ����
        MyHPSlider.value = MonsterBattleGameScript.Instance.MyMonster.HP;
        MyHPTxt.text = $"{MonsterBattleGameScript.Instance.MyMonster.HP} / {MonsterBattleGameScript.Instance.MyMonster.MaxHP}";
        EnemyHPSlider.value = MonsterBattleGameScript.Instance.EnemyMonster.HP;
        EnemyHPTxt.text = $"{MonsterBattleGameScript.Instance.EnemyMonster.HP} / {MonsterBattleGameScript.Instance.EnemyMonster.MaxHP}";

        #endregion

        AliveCheck(); // �� �� �ϳ��� �׾����� üũ�ϰ� �׾����� ���ӿ��� or �̾��ϱ�

        SkillCoolClear(); // ��ų �� ���� ����...

        #region Ű �Է½� ��ų �ߵ��Լ� ����...
        if (Input.GetKeyDown(KeyCode.Alpha1)) // ���� ��ư 1 �� ������ �����ϱ�
            UseSkill(0);

        if (Input.GetKeyDown(KeyCode.Alpha2)) // ���� ��ư 2 �� ������ ����ϱ�
            UseSkill(1);

        if (SkillCoolArr[2] <= 0f) //���� ���� ��Ÿ���� �� �� ���� ������
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
        EnemyMonsterTxt.text = $"��  �� : {MonsterBattleGameScript.Instance.EnemyMonster.Name} \n���ݷ�: {MonsterBattleGameScript.Instance.EnemyMonster.Att}";

        GameoverPase.gameObject.SetActive(false);
    }

    void AliveCheck() // ���� �ϳ��� �׾����� üũ... �׾����� ���ӿ���â�� ���...
    {
        if (MonsterBattleGameScript.Instance.MyMonster.IsAlive == false || MonsterBattleGameScript.Instance.EnemyMonster.IsAlive == false) // �� �� �ϳ��� ������..
        {
            if (MonsterBattleGameScript.Instance.MyMonster.IsAlive)
            {
                GameoverTxt.text = $"�����մϴ�!\n{MonsterBattleGameScript.Instance.MyMonster.Name}�� �̰���ϴ�!";
            }
            else if (MonsterBattleGameScript.Instance.EnemyMonster.IsAlive)
            {
                GameoverTxt.text = $"�ƽ��׿�,\n{MonsterBattleGameScript.Instance.MyMonster.Name}�� �����ϴ�...\n{WinCount}������ ��ҽ��ϴ�.";
                ButtonRestart.gameObject.SetActive(false); // �÷��̾ �׾����� �̾��ϱ��ư ��Ȱ��ȭ...
            }
            else
            {
                GameoverTxt.text = $"���� ���ÿ� �׾��ų� ������ �߻��߽��ϴ�.";
                ButtonRestart.gameObject.SetActive(false);
            }

            for (int i = 0; i < SkillCoolArr.Length; i++) // �׾����� ��Ÿ�� ���õ� �� ��� �ʱ�ȭ...
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

    void SkillCoolClear() // ��ų ��Ÿ�� ���õ� �͵� ����...
    {
        for (int i = 0; i < SkillCoolArr.Length; i++)
        {
            if (SkillCoolArr[i] <= 0f) // �ش� ��ų���� 0 ���� �۰ų� ���ٸ�...
            {
                SkillCoolArr[i] = 0f; // 0 ���� �ٲٰ�...
                SkillCooldownImgArr[i].gameObject.SetActive(false); // ��ٿ� �̹����� ��Ȱ��ȭ �� ��...
                if (SkillCoolcorArr[i] != null) // �ش� ��ų���ڷ�ƾ�� null �� �ƴϸ�...
                {
                    StopCoroutine(SkillCoolcorArr[i]);
                    SkillCoolcorArr[i] = null; // null �� �ٲ��
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

    #region ����ȭ �ϱ� ��...
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
