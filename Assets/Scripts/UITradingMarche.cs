using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Gère l'interface d'achat/vente pour un marché.
/// </summary>
public class UITradingMarche : MonoBehaviour
{
    [Header("Références logiques")]
    public string nomMarche;
    public JoueurTradingControleur joueur;
    public Graph3D graphique;

    [Header("Composants UI")]
    public TextMeshProUGUI textePrixActuel;
    public TMP_InputField champQuantite;
    public Button boutonAcheter;
    public Button boutonVendre;

    void Update()
    {
        try
        {
            // --- Affichage dynamique du prix actuel ---
            if (graphique != null && textePrixActuel != null && !textePrixActuel.IsDestroyed())
            {
                // Met à jour le texte du prix actuel affiché à l'écran
                textePrixActuel.text = $"Prix actuel : {graphique.prixActuel:F2}$";
            }

            // --- Gestion de l'interaction du bouton "Acheter" ---
            if (boutonAcheter != null && joueur != null && graphique != null && champQuantite != null)
            {
                // Active ou désactive le bouton Acheter en fonction de l'argent disponible et de la quantité
                boutonAcheter.interactable = joueur.PeutAcheter(graphique.prixActuel, GetQuantite());
            }

            // --- Gestion de l'interaction du bouton "Vendre" ---
            if (boutonVendre != null && joueur != null)
            {
                // Active ou désactive le bouton Vendre si le joueur possède des actifs à vendre sur ce marché
                boutonVendre.interactable = joueur.PeutVendre(nomMarche);
            }
        }
        catch (MissingReferenceException e)
        {
            // Si une référence UI est détruite en cours de route, affiche un avertissement sans crasher le jeu
            Debug.LogWarning($"Référence UI détruite dans UITradingMarche ({nomMarche}) : {e.Message}");
        }
    }

    /// <summary>
    /// Retourne la quantité entrée, ou 0 si non valide.
    /// </summary>
    int GetQuantite()
    {
        if (champQuantite == null || champQuantite.text == "") return 0;
        int quantite;
        if (int.TryParse(champQuantite.text, out quantite))
            return Mathf.Max(quantite, 1); // min = 1
        return 0;
    }

    /// <summary>
    /// Appelé par le bouton "Acheter"
    /// </summary>
    public void ActionAcheter()
    {
        if (joueur == null || graphique == null) return;
        int quantite = GetQuantite();
        float prix = graphique.prixActuel;
        joueur.Acheter(nomMarche, prix, quantite);
    }

    /// <summary>
    /// Appelé par le bouton "Vendre"
    /// </summary>
    public void ActionVendre()
    {
        if (joueur == null || graphique == null) return;
        float prix = graphique.prixActuel;
        joueur.VendreTout(nomMarche, prix);
    }
}
