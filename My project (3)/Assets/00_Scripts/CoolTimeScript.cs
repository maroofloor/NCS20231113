using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolTimeScript : MonoBehaviour
{
    public Image[] CoolTimeImgs;
    public Text[] CoolTimeTxts;

    bool FstCool => SkillCools[0] != 0f;
    bool SndCool => SkillCools[1] != 0f;
    bool TrdIsCool => SkillCools[2] != 0f;

    Dictionary<int, float> CoolTimes = new Dictionary<int, float>() { { 0, 10f }, { 1, 20f }, { 2, 15f } };

    float[] SkillCools = new float[3] { 0f, 0f, 0f };
    float[] SkillCoolMax = new float[3] { 0f, 0f, 0f };
    //float SecSkillCool = 0;
    //float ThrSkillcool = 0;

    Coroutine[] cor = new Coroutine[3] { null, null, null };

    private void Awake()
    {

    }

    private void Update()
    {
        for (int i = 0; i < CoolTimeTxts.Length; i++)
        {
            CoolTimeTxts[i].text = $"{(int)SkillCools[i]}s";
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseSkill(0, CoolTimes[0]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UseSkill(1, CoolTimes[1]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UseSkill(2, CoolTimes[2]);            
        }

        CoolimgsSet(0, FstCool);
        CoolimgsSet(1, SndCool);
        CoolimgsSet(2, TrdIsCool);

        for (int i = 0; i < SkillCools.Length; i++)
        {
            if (SkillCools[i] < 0f)
                SkillCools[i] = 0f;

            if (SkillCools[i] == 0f)
            {
                StopCoroutine(cor[i]);
                cor[i] = null;
            }

        }
    }

    void CoolimgsSet(int index, bool OnOff)
    {
        CoolTimeImgs[index].gameObject.SetActive(OnOff);
    }

    void UseSkill(int input, float cool)
    {
        if (SkillCools[input] > 0f)
        {
            return;
        }
        if (input == 2)
        {
            HPbarSample.instance.EnemyHP -= 30;
        }
        SkillCools[input] = cool;
        CoolTimeImgs[input].gameObject.SetActive(true);
        if (cor[input] == null)
        {
            cor[input] = StartCoroutine(SkillCooldown(input, cool));
        }
        #region 스킬이 한개인 경우...
        //if (SkillCool != 0 || HPbarSample.instance.HP <= 0)
        //{
        //    return;
        //}            
        //else
        //{
        //    SkillCool = cool;
        //    CoolTimeImgs[input].gameObject.SetActive(true);
        //    if (cor == null)
        //    {
        //        cor = StartCoroutine(SkillCooldown(input, SkillCool));
        //    }
        //}
        #endregion 
    }
    IEnumerator SkillCooldown(int input, float cooltime)
    {
        SkillCoolMax[input] = cooltime;
        while (SkillCools[input] > 0)
        {
            CoolTimeImgs[input].fillAmount = SkillCools[input] / SkillCoolMax[input];
            yield return new WaitForSeconds(0.1f);
            SkillCools[input] -= 0.1f;
        }
    }
}
