using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManger : MonoBehaviour
{
    public GameObject PackSack;
    public GameObject ItemDescribe;
    public GameObject GameUI;
    public GameObject MainMenu;
    public GameObject ShopUI;


    /*------------------------------------------*/
    public void Button_1()
    {
        MainMenu.SetActive(false);
        GameUI.SetActive(true);
    }
    /*------------------------------------------*/

    public void Button_4()
    {
        GameUI.SetActive(false);
        ShopUI.SetActive(true);
    }
    /*------------------------------------------*/
    public void Button_5()
    {
        ItemDescribe.SetActive(true);
        GameUI.SetActive(false);
        PackSack.SetActive(true);
    }
    /*------------------------------------------*/
    public void Button_6()
    {
        GameUI.SetActive(true);
        PackSack.SetActive(false);
        ItemDescribe.SetActive(false);
    }
    /*------------------------------------------*/
    public void Button_7()
    {
        ShopUI.SetActive(false);
        GameUI.SetActive(true);
    }
    /*------------------------------------------*/
}
