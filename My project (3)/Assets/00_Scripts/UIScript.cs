using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public Dropdown dropdown;
    public Transform contents;
    public GameObject scrollContentsPrefab;
    public Dictionary<int, List<Image>> allScrollContents = new Dictionary<int, List<Image>>();

    public Sprite[] sprites;

    private void Start()
    {
        dropdown.ClearOptions();

        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        GameObject tmp;

        options.Add(new Dropdown.OptionData("전체선택", null));

        for (int i = 0; i < 3; i++)
        {
            options.Add(new Dropdown.OptionData("슬라임" + (i + 1), sprites[i]));

            allScrollContents.Add(i, new List<Image>());
            for (int j = 0; j < 2; j++)
            {
                tmp = Instantiate(scrollContentsPrefab, contents);

                allScrollContents[i].Add(tmp.GetComponent<Image>());
                tmp.GetComponent<Image>().sprite = sprites[i];
            }
        }

        dropdown.AddOptions(options);

    }
}
