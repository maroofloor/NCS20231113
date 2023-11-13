using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapesScript : MonoBehaviour
{
    public SpriteRenderer[] shapes;
    //public SpriteRenderer Triangle;
    //public SpriteRenderer Square;
    //public SpriteRenderer Circle;

    //public ToggleGroup ToggleGroup;

    public Toggle[] toggles;

    public Toggle[] clrtoggles;
    //public Toggle TriangleTog;
    //public Toggle SquareTog;
    //public Toggle CircleTog;

    public Slider slider;

    Color color;

    //Color[] colors = new Color[3] { Color.red, Color.green, Color.blue };

    #region
    //private void Awake()
    //{
    //    ColorToggle();
    //    ClickedToggle();

    //    slider.value = 10f;
    //}

    //public void SliderControl()
    //{
    //    _color.a = slider.value;
    //    for (int i = 0; i < shapes.Length; i++)
    //    {
    //        shapes[i].color = _color;
    //    }
    //}
    #endregion

    private void Awake()
    {
        slider.value = 10;

        AlphaSlider();
        ColorToggle();
        ShapesToggle();
    }

    public void AlphaSlider()
    {
        color.a = slider.value * 0.1f;

        for (int i = 0; i < shapes.Length; i++)
        {
            shapes[i].color = color;
        }
    }
    public void ColorToggle()
    {
        color.r = clrtoggles[0].isOn ? 1f : 0f;
        color.g = clrtoggles[1].isOn ? 1f : 0f;
        color.b = clrtoggles[2].isOn ? 1f : 0f;

        for (int i = 0; i < shapes.Length; i++)
        {
            shapes[i].color = color;
        }

        #region
        //float Alpha = slider.value * 0.1f;

        //_color = new Color(clrtoggles[0].isOn ? 1f : 0f,
        //    clrtoggles[1].isOn ? 1f : 0f,
        //    clrtoggles[2].isOn ? 1f : 0f,
        //    Alpha/*단계별로 하는게 아닐때 == slider.value */);

        //for (int i = 0; i < shapes.Length; i++)
        //{
        //    shapes[i].color = _color;
        //}
        #endregion
    }
    public void ShapesToggle()
    {
        for (int i = 0; i < shapes.Length; i++)
        {
            shapes[i].gameObject.SetActive(toggles[i].isOn);
        }

        #region
        //for (int i = 0; i < toggles.Length; i++)
        //{
        //    shapes[i].gameObject.SetActive(toggles[i].isOn);
        //}
        //if (TriangleTog.isOn)
        //{
        //    Shapeobjs[0].SetActive(true);
        //    Shapeobjs[1].SetActive(false);
        //    Shapeobjs[2].SetActive(false);
        //}
        //else if (SquareTog.isOn)
        //{
        //    Shapeobjs[0].SetActive(false);
        //    Shapeobjs[1].SetActive(true);
        //    Shapeobjs[2].SetActive(false);
        //}
        //else if (CircleTog.isOn)
        //{
        //    Shapeobjs[0].SetActive(false);
        //    Shapeobjs[1].SetActive(false);
        //    Shapeobjs[2].SetActive(true);
        //}
        #endregion
    }
}

