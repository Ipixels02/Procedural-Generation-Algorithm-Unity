using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropdownHandler : MonoBehaviour //класс для создания выпадающего списка с выбором типа генерации
{

    int iterations, walkLength;
    bool startRandomlyEachIteration;

    void Start()
    {
        var dropdown = transform.GetComponent<Dropdown>();
        dropdown.options.Clear();

        List<string> items = new List<string>();
        items.Add("BigDungeon");
        items.Add("SmallDungeon");

        foreach (var item in items)
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = item });
        }

        //dropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(dropdown); });
    }

    //void DropdownItemSelected(Dropdown dropdown)
    //{
    //    int index = dropdown.value;
    //    if (dropdown.options[index] == 0)
    //    {
    //        BigDungeon();
    //    }
    //    else if (dropdown.options[index] == 1)
    //    {
    //        Island();
    //    }
    //}

    //void BigDungeon()
    //{
    //    iterations = 100;
    //    walkLength = 100;
    //    startRandomlyEachIteration = true;
    //}

    //void Island()
    //{
    //    iterations = 10;
    //    walkLength = 20;
    //    startRandomlyEachIteration = false;
    //}

}
