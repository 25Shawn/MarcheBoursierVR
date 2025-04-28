using UnityEngine;

public class PlayerTradingController : MonoBehaviour
{
    [Header("Finances du joueur")]
    public float startingMoney = 1000f;
    public float currentMoney;
    public bool hasAsset = false;
    public float buyPrice = 0f;

    void Start()
    {
        currentMoney = startingMoney;
    }

    // Appelé quand le joueur achète
    public bool Buy(float marketPrice)
    {
        if (hasAsset) return false; // déjà un actif, pas de double achat
        if (currentMoney < marketPrice) return false; // pas assez d'argent

        buyPrice = marketPrice;
        hasAsset = true;
        currentMoney -= marketPrice;
        Debug.Log($"Achat à {marketPrice:F2} - Argent restant : {currentMoney:F2}");
        return true;
    }

    // Appelé quand le joueur vend
    public bool Sell(float marketPrice)
    {
        if (!hasAsset) return false;

        float profit = marketPrice - buyPrice;
        currentMoney += marketPrice;
        hasAsset = false;

        Debug.Log($"Vente à {marketPrice:F2} - Profit : {profit:F2} - Argent total : {currentMoney:F2}");
        return true;
    }
}
