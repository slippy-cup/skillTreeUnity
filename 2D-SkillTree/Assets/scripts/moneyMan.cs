using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class moneyMan : MonoBehaviour
{
    public int playerCurrency
    { get; set; }

    [SerializeField] int money;

    [SerializeField] Button m_Button;

    [SerializeField]
    TextMeshProUGUI moneyLabel;

    
    // Start is called before the first frame update
    void Start()
    {
        m_Button.onClick.AddListener(onClick);
        moneyLabel.text = "Money: " + playerCurrency.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void onClick()
    {
        playerCurrency = playerCurrency + money;
        Debug.Log("Money is added");
        UpdateText();
    }

    public void UpdateText()
    {
        moneyLabel.text = "Money: " + playerCurrency.ToString();
    }
}
