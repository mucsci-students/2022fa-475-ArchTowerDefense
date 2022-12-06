using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Currency : MonoBehaviour
{
    // You'll start with 500
    public float shekels = 500;
    public TextMeshProUGUI moneyText;


    // Update is called once per frame
    void Update()
    {
        moneyText.SetText("Shekels: " + shekels);
    }

    public void purchase(float price)
    {
        shekels -= price;
    }

    public void addShekels(float amount)
    {
        shekels += amount;
    }
}
