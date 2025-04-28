using System.Collections.Generic;
using UnityEngine;

public class JoueurTradingControleur : MonoBehaviour
{
    [Header("Finances du joueur")]
    public float argentInitial = 1000f;
    public float argentActuel;

    // Structure pour représenter un achat
    [System.Serializable]
    public class Achat
    {
        public float prixUnitaire;
        public int quantite;

        public Achat(float prix, int qte)
        {
            prixUnitaire = prix;
            quantite = qte;
        }
    }

    // Portefeuille : marché → liste d’achats
    private Dictionary<string, List<Achat>> portefeuille = new Dictionary<string, List<Achat>>();

    void Start()
    {
        argentActuel = argentInitial;
    }

    /// <summary>
    /// Le joueur achète un actif sur un marché donné
    /// </summary>
    public bool Acheter(string nomMarche, float prixUnitaire, int quantite)
    {
        float coutTotal = prixUnitaire * quantite;
        if (argentActuel < coutTotal)
        {
            Debug.LogWarning("Pas assez d’argent pour cet achat.");
            return false;
        }

        argentActuel -= coutTotal;

        if (!portefeuille.ContainsKey(nomMarche))
        {
            portefeuille[nomMarche] = new List<Achat>();
        }

        portefeuille[nomMarche].Add(new Achat(prixUnitaire, quantite));

        Debug.Log($"Achat de {quantite} unités sur {nomMarche} à {prixUnitaire:F2}$ - Total dépensé : {coutTotal:F2}$ - Solde : {argentActuel:F2}$");
        return true;
    }

    /// <summary>
    /// Le joueur vend tout ce qu’il possède sur un marché donné au prix actuel
    /// </summary>
    public bool VendreTout(string nomMarche, float prixVente)
    {
        if (!portefeuille.ContainsKey(nomMarche) || portefeuille[nomMarche].Count == 0)
        {
            Debug.LogWarning($"Aucun actif à vendre sur le marché {nomMarche}.");
            return false;
        }

        float gainTotal = 0f;
        float coutTotal = 0f;
        int totalQuantite = 0;

        foreach (Achat achat in portefeuille[nomMarche])
        {
            gainTotal += prixVente * achat.quantite;
            coutTotal += achat.prixUnitaire * achat.quantite;
            totalQuantite += achat.quantite;
        }

        float profit = gainTotal - coutTotal;
        argentActuel += gainTotal;

        portefeuille[nomMarche].Clear(); // vide les actifs du marché

        Debug.Log($"Vente de {totalQuantite} unités sur {nomMarche} à {prixVente:F2}$ - Gain : {gainTotal:F2}$ - Profit net : {profit:F2}$ - Solde : {argentActuel:F2}$");
        return true;
    }

    public bool PeutAcheter(float prixUnitaire, int quantite)
    {
        return argentActuel >= prixUnitaire * quantite;
    }

    public bool PeutVendre(string nomMarche)
    {
        return portefeuille.ContainsKey(nomMarche) && portefeuille[nomMarche].Count > 0;
    }
}
