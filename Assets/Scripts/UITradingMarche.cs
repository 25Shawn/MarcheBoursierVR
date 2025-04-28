using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// G�re l'interface d'achat/vente pour un march�.
/// </summary>
public class UITradingMarche : MonoBehaviour
{
    [Header("R�f�rences logiques")]
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
                // Met � jour le texte du prix actuel affich� � l'�cran
                textePrixActuel.text = $"Prix actuel : {graphique.prixActuel:F2}$";
            }

            // --- Gestion de l'interaction du bouton "Acheter" ---
            if (boutonAcheter != null && joueur != null && graphique != null && champQuantite != null)
            {
                // Active ou d�sactive le bouton Acheter en fonction de l'argent disponible et de la quantit�
                boutonAcheter.interactable = joueur.PeutAcheter(graphique.prixActuel, GetQuantite());
            }

            // --- Gestion de l'interaction du bouton "Vendre" ---
            if (boutonVendre != null && joueur != null)
            {
                // Active ou d�sactive le bouton Vendre si le joueur poss�de des actifs � vendre sur ce march�
                boutonVendre.interactable = joueur.PeutVendre(nomMarche);
            }
        }
        catch (MissingReferenceException e)
        {
            // Si une r�f�rence UI est d�truite en cours de route, affiche un avertissement sans crasher le jeu
            Debug.LogWarning($"R�f�rence UI d�truite dans UITradingMarche ({nomMarche}) : {e.Message}");
        }
    }

    /// <summary>
    /// Retourne la quantit� entr�e, ou 0 si non valide.
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
    /// Appel� par le bouton "Acheter"
    /// </summary>
    public void ActionAcheter()
    {
        if (joueur == null || graphique == null) return;
        int quantite = GetQuantite();
        float prix = graphique.prixActuel;
        joueur.Acheter(nomMarche, prix, quantite);
    }

    /// <summary>
    /// Appel� par le bouton "Vendre"
    /// </summary>
    public void ActionVendre()
    {
        if (joueur == null || graphique == null) return;
        float prix = graphique.prixActuel;
        joueur.VendreTout(nomMarche, prix);
    }
}
