using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Platform;
using Oculus.Platform.Models;
using UnityEngine.UI;


// This class coordinates In-App-Purchases (IAP) for the application.  Follow the
// instructions in the Readme for setting up IAP on the Meta Quest dashboard. Only
// one consumable IAP item is used is the demo: the Power-Ball!
public class IAPManager : MonoBehaviour
{

    // where to record to display the current price for the IAP item
    [SerializeField] private Text m_availableItems;
    [SerializeField] private Text m_priceText;
    [SerializeField] private Text m_purchasedItems;

    // purchasable IAP products we've configured on the Meta Quest dashboard
    private const string CONSUMABLE_1 = "com.bb.huntduck.freeitem";

    void Start()
    {
        GetProductPrices();
        GetPurchasedProducts();
    }

    // get the current price for the configured IAP item
    public void GetProductPrices()
    {
        string[] skus = { CONSUMABLE_1 };
        IAP.GetProductsBySKU(skus).OnComplete(GetProductsBySKUCallback);
    }

    void GetProductsBySKUCallback(Message<ProductList> msg)
    {
        if (msg.IsError) return;


        foreach (Product p in msg.GetProductList())
        {
            Debug.LogFormat("Product: sku:{0} name:{1} price:{2}", p.Sku, p.Name, p.FormattedPrice);
            if (p.Sku == CONSUMABLE_1)
            {
                m_availableItems.text += $"{p.Name} - {p.FormattedPrice}";
                m_priceText.text = $"{p.FormattedPrice}\n";
            }
        }
    }

    // fetches the Durable purchased IAP items.  should return none unless you are expanding the
    // to sample to include them.
    public void GetPurchasedProducts()
    {
        // Call IAP mehtod and then call private mthod when complete to do something with that data
        IAP.GetViewerPurchases().OnComplete(GetViewerPurchasesCallback);
    }

    void GetViewerPurchasesCallback(Message<PurchaseList> msg)
    {
        if (msg.IsError) return;

        foreach (Purchase p in msg.GetPurchaseList())
        {
            Debug.LogFormat("Purchased: sku:{0} granttime:{1} id:{2}", p.Sku, p.GrantTime, p.ID);
            m_purchasedItems.text += $"{p.Sku} - {p.GrantTime}";
        }
    }

    public void BuyItem()
    {
        IAP.LaunchCheckoutFlow(CONSUMABLE_1).OnComplete(BuyItemCallback);
    }

    private void BuyItemCallback(Message<Purchase> msg)
    {
        if (msg.IsError) return;


        m_purchasedItems.text = string.Empty;
        GetPurchasedProducts();
    }
}