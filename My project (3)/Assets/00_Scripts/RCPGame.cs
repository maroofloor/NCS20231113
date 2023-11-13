using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RCP
{
    가위,
    바위,
    보자기,

    End
}

public class RCPGame : MonoBehaviour
{
    public Image MainImage;
    public Sprite[] SpritesArr;
    public Text MainTxt;
    public Text Score;
    public Button[] buttons;

    int WinPoint = 0;
    int TryPoint = 0;
    Coroutine cor = null;
    Coroutine Waitcor = null;

    bool IsRepeat = true;

    private void Start()
    {        
        Score.text = $"Win : {WinPoint} / Try : {TryPoint}";
        IsRepeat = true;
        if (cor == null)
        {
            cor = StartCoroutine(RepeatImgs());
        }
    }

    private void Update()
    {
        if (IsRepeat == true && Waitcor != null)
        {
            StopCoroutine(Waitcor);
            Waitcor = null;
        }
    }

    RCP chooseRCP = RCP.End;
    RCP ComRCP = RCP.End;

    public void ButtonClick(int choosenum)
    {
        ButtonsOnOff(false);

        IsRepeat = false;
        if (cor != null)
        {
            StopCoroutine(cor);
            cor = null;
        }        

        ComRCP = (RCP)Random.Range(0, (int)RCP.End);
        chooseRCP = (RCP)choosenum;

        MainImage.sprite = SpritesArr[(int)ComRCP];
        if (chooseRCP == ComRCP)
        {
            MainTxt.text = $"{chooseRCP}를 내서 비겼습니다.";            
        }
        else if ( (ComRCP == RCP.가위 && chooseRCP == RCP.바위) || (ComRCP == RCP.바위 && chooseRCP == RCP.보자기) || (ComRCP == RCP.보자기 && chooseRCP == RCP.가위) )
        {
            MainTxt.text = $"{chooseRCP}를 내서 이겼습니다!!!";
            WinPoint++;
        }
        else
        {
            MainTxt.text = $"{chooseRCP}를 내서 졌습니다...";
        }
        TryPoint++;
        Score.text = $"Win : {WinPoint} / Try : {TryPoint}";

        if (Waitcor == null)
        {
            Waitcor = StartCoroutine(WaitRepeat());
        }
    }

    void ButtonsOnOff(bool _bool)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(_bool);
        }
    }

    IEnumerator RepeatImgs()
    {
        int index = 0;
        while (IsRepeat)
        {
            MainImage.sprite = SpritesArr[index];
            yield return new WaitForSeconds(0.1f);
            index++;
            if (index > 2)
            {
                index = 0;
            }
        }
    }

    IEnumerator WaitRepeat()
    {
        yield return new WaitForSeconds(2f);
        IsRepeat = true;
        if (cor == null)
        {
            cor = StartCoroutine(RepeatImgs());
        }
        ButtonsOnOff(true);
    }

}