using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkManager : MonoBehaviour
{
    public Button[] perkButtons;
    public Text[] MoneyTexts;
    public Image[] checkImages;
    public Text costText;

    [HideInInspector]
    public int totalCost;

    public void Start()
    {
        totalCost =0;
    }
    private void Update()
    {
        costText.text = totalCost.ToString();
        UserManager.userInstance.perkCost = totalCost;

        
    }

    public void SelectPerk(int buttonNo)
    {
        
        if(UserManager.userInstance.perks[buttonNo])
        {
            UserManager.userInstance.perks[buttonNo] = false;
            checkImages[buttonNo].gameObject.SetActive(false);
            MoneyTexts[buttonNo].gameObject.SetActive(true);
            totalCost -= 100;
        }
        else
        {
            totalCost += 100;
            if (UserManager.userInstance.Coin - totalCost < 0)
            {
                totalCost -= 100;
            }
            else
            {
                UserManager.userInstance.perks[buttonNo] = true;
                MoneyTexts[buttonNo].gameObject.SetActive(false);
                checkImages[buttonNo].gameObject.SetActive(true);
            }
        }
    }


}
