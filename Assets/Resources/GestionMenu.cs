using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuVR : MonoBehaviour
{
    public void CommencerJeu()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitterJeu()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; // Arrête le mode Play dans l'éditeur
#else
        Application.Quit(); // Ferme dans un vrai jeu (build)
#endif
    }
}
