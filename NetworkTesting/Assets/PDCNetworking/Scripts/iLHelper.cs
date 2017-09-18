using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class iLHelper : MonoBehaviour
{
    Text name;
    Button drop;
    Button trade;
    Text value;

    public InputField valueField;

    public int nameId;
    public int dropId;
    public int tradeId;
    public int valueFId;
    public int valueId;

    public int listIndex;
    public void Awake()
    {
        name = transform.GetChild(nameId).GetComponent<Text>();
        drop = transform.GetChild(dropId).GetComponent<Button>();
        trade = transform.GetChild(tradeId).GetComponent<Button>();
        value = transform.GetChild(valueId).GetComponent<Text>();
        valueField = transform.GetChild(valueFId).GetComponent<InputField>();
    }
    public void Fill(Item item, int index)
    {
        listIndex = index;
        name.text = item.name;
        value.text = item.value.ToString();
        drop.onClick.AddListener(() => Inventory.invManager.Drop(index));
        trade.onClick.AddListener(() => Inventory.invManager.Trade(index));
        valueField.onEndEdit.AddListener(delegate { Inventory.invManager.ChangeValue(index); });
    }
    public void Renumber(int i)
    {
        listIndex = i;
        valueField.onEndEdit.RemoveAllListeners();
        valueField.onEndEdit.AddListener(delegate { Inventory.invManager.ChangeValue(listIndex); });
        drop.onClick.RemoveAllListeners();
        drop.onClick.AddListener(() => Inventory.invManager.Drop(listIndex));
    }
    public void Refresh()
    {
        valueId = Inventory.invManager.inventory[listIndex].value;
        value.text = valueId.ToString();

    }
}