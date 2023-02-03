using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text totalIridiumText;
    [SerializeField] private TMP_Text iridiumPerSecondText;
    [SerializeField] private TMP_Text iridiumPerClickText;

    [SerializeField] private Button getIridiumButton;

    [SerializeField] private Button ratePlus1_Button;
    private TMP_Text ratePlus1_ButtonText;
    private int ratePlus1_Cost = 10;
    [SerializeField] private Button ratePlus10_Button;
    private TMP_Text ratePlus10_ButtonText;
    private int ratePlus10_Cost = 100;
    [SerializeField] private Button ratePlus100_Button;
    private TMP_Text ratePlus100_ButtonText;
    private int ratePlus100_Cost = 1000;
    [SerializeField] private Button ratePlus1000_Button;
    private TMP_Text ratePlus1000_ButtonText;
    private int ratePlus1000_Cost = 10000;
    [SerializeField] private Button upgradeClick_Button;
    private TMP_Text upgradeClick_ButtonText;
    private int upgradeClick_Cost = 1000;

    private int totalIridium = 0;
    private int iridiumPerSecond = 0;
    private int iridiumPerClickPercent = 1;

    private void Start()
    {
        getIridiumButton.onClick.AddListener(GetIridiumClicked);
        ratePlus1_Button.onClick.AddListener(RatePlus1Clicked);
        ratePlus10_Button.onClick.AddListener(RatePlus10Clicked);
        ratePlus100_Button.onClick.AddListener(RatePlus100Clicked);
        ratePlus1000_Button.onClick.AddListener(RatePlus1000Clicked);
        upgradeClick_Button.onClick.AddListener(UpgradeClickClicked);

        ratePlus1_ButtonText = ratePlus1_Button.GetComponentInChildren<TMP_Text>();
        ratePlus10_ButtonText = ratePlus10_Button.GetComponentInChildren<TMP_Text>();
        ratePlus100_ButtonText = ratePlus100_Button.GetComponentInChildren<TMP_Text>();
        ratePlus1000_ButtonText = ratePlus1000_Button.GetComponentInChildren<TMP_Text>();
        upgradeClick_ButtonText = upgradeClick_Button.GetComponentInChildren<TMP_Text>();

        iridiumPerClickText.text = iridiumPerClickPercent.ToString() + " % of Iridium/Sec";
        iridiumPerSecondText.text = iridiumPerSecond.ToString() + " Iridium/s";
        totalIridiumText.text = totalIridium.ToString() + " Iridium";
        ratePlus1_ButtonText.text = "Rate +1 ($" + ratePlus1_Cost.ToString() + ")";
        ratePlus10_ButtonText.text = "Rate +10 ($" + ratePlus10_Cost.ToString() + ")";
        ratePlus100_ButtonText.text = "Rate +100 ($" + ratePlus100_Cost.ToString() + ")";
        ratePlus1000_ButtonText.text = "Rate +1000 ($" + ratePlus1000_Cost.ToString() + ")";

        StartCoroutine(AddIridiumPerSecond());
    }

    private void GetIridiumClicked()
    {
        totalIridium += Mathf.Max(1, iridiumPerClickPercent * iridiumPerSecond / 100);
        totalIridiumText.text = totalIridium.ToString() + " Iridium";
    }

    private void RatePlus1Clicked()
    {
        if (totalIridium >= ratePlus1_Cost)
        {
            totalIridium -= ratePlus1_Cost;
            ratePlus1_Cost = (int)(ratePlus1_Cost * 1.2f);
            iridiumPerSecond += 1;
            iridiumPerSecondText.text = iridiumPerSecond.ToString() + " Iridium/s";
            totalIridiumText.text = totalIridium.ToString() + " Iridium";
            ratePlus1_ButtonText.text = "Rate +1 ($" + ratePlus1_Cost.ToString() + ")";
        }
    }

    private void RatePlus10Clicked()
    {
        if (totalIridium >= ratePlus10_Cost)
        {
            totalIridium -= ratePlus10_Cost;
            ratePlus10_Cost = (int)(ratePlus10_Cost * 1.2f);
            iridiumPerSecond += 10;
            iridiumPerSecondText.text = iridiumPerSecond.ToString() + " Iridium/s";
            totalIridiumText.text = totalIridium.ToString() + " Iridium";
            ratePlus10_ButtonText.text = "Rate +10 ($" + ratePlus10_Cost.ToString() + ")";
        }
    }

    private void RatePlus100Clicked()
    {
        if (totalIridium >= ratePlus100_Cost)
        {
            totalIridium -= ratePlus100_Cost;
            ratePlus100_Cost = (int)(ratePlus100_Cost * 1.2f);
            iridiumPerSecond += 100;
            iridiumPerSecondText.text = iridiumPerSecond.ToString() + " Iridium/s";
            totalIridiumText.text = totalIridium.ToString() + " Iridium";
            ratePlus100_ButtonText.text = "Rate +100 ($" + ratePlus100_Cost.ToString() + ")";
        }
    }

    private void RatePlus1000Clicked()
    {
        if (totalIridium >= ratePlus1000_Cost)
        {
            totalIridium -= ratePlus1000_Cost;
            ratePlus1000_Cost = (int)(ratePlus1000_Cost * 1.2f);
            iridiumPerSecond += 1000;
            iridiumPerSecondText.text = iridiumPerSecond.ToString() + " Iridium/s";
            totalIridiumText.text = totalIridium.ToString() + " Iridium";
            ratePlus1000_ButtonText.text = "Rate +1000 ($" + ratePlus1000_Cost.ToString() + ")";
        }
    }

    private void UpgradeClickClicked()
    {
        if (totalIridium >= upgradeClick_Cost)
        {
            totalIridium -= upgradeClick_Cost;
            upgradeClick_Cost = (int)(upgradeClick_Cost * 1.5f);
            iridiumPerClickPercent += 1;
            iridiumPerClickText.text = iridiumPerClickPercent.ToString() + " % of Iridium/Sec";
            totalIridiumText.text = totalIridium.ToString() + " Iridium";
            upgradeClick_ButtonText.text = "Upgrade Click ($" + upgradeClick_Cost.ToString() + ")";
        }
    }

    private IEnumerator AddIridiumPerSecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            totalIridium += iridiumPerSecond;
            totalIridiumText.text = totalIridium.ToString() + " Iridium";
        }
    }
}