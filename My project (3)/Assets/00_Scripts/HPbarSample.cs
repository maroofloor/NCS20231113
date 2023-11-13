using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPbarSample : MonoBehaviour
{
    public static HPbarSample instance = null;

    public Slider HPBar;

    public int HP = 100;
    public int MaxHP = 100;
    public int Damage = 10;
    public Text HPtxt;

    public Slider EnemyHPBar;

    public int EnemyHP = 100;
    public int EnemyMaxHP = 100;
    public int EnemyDamage = 10;
    public Text EnemyHPtxt;
    public Image Enemyimg;
    public Sprite Deadimg;
    public Slider EnemyGaugeSlider;
    float EnemyAttackGauge = 0f ;
    bool EnemyIsAlive => EnemyHP > 0;

    Coroutine cor = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if (instance != null)
            {
                Destroy(this.gameObject);
            }            
        }

        HP = MaxHP;
        HPBar.maxValue = MaxHP;

        EnemyHP = EnemyMaxHP;
        EnemyHPBar.maxValue = EnemyMaxHP;
    }

    private void Start()
    {
        if (cor == null)
        {
            StartCoroutine(GaugeRaise());
        }
    }

    private void Update()
    {
        EnemyGaugeSlider.value = EnemyAttackGauge;

        if (HP < 0)
        {
            HP = 0;
        }
        if (EnemyHP <= 0)
        {
            EnemyHP = 0;
            Enemyimg.sprite = Deadimg;
        }

        HPBar.value = HP;
        HPBar.maxValue = MaxHP;
        HPtxt.text = $"{HP} / {MaxHP}";

        EnemyHPBar.value = EnemyHP;
        EnemyHPBar.maxValue = EnemyMaxHP;
        EnemyHPtxt.text = $"{EnemyHP} / {EnemyMaxHP}";

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //DamageCorrected(EnemyHP, Damage);

            EnemyHP -= Damage;
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            LevelUP();
        }

        if (EnemyAttackGauge >= 1f)
        {
            if (EnemyIsAlive)
            {
                EnemyAttack();
            }            
        }

        if (EnemyAttackGauge <= 0f)
        {
            if (cor == null)
            {
                cor = StartCoroutine(GaugeRaise());
            }
        }        
    }

    void EnemyAttack()
    {
        HP -= EnemyDamage;
        EnemyAttackGauge = 0f;
        StopCoroutine(cor);
        cor = null;
    }

    public IEnumerator GaugeRaise()
    {
        while (EnemyAttackGauge <= 1f)
        {
            yield return new WaitForSeconds(0.1f);
            EnemyAttackGauge += 0.02f;
        }
    }

    //void DamageCorrected(int hp, int damage)
    //{
    //    hp -= damage;
    //}

    void LevelUP()
    {
        MaxHP += 100;
        HP += 100;
    }
}