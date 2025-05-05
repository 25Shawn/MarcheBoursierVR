using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class NewsTicker : MonoBehaviour
{
    public TextMeshPro newsText;
    public string[] news = {
        "L'économie mondiale connaît une forte croissance !",
        "La bourse subit une baisse importante des actions.",
        "Une nouvelle fusion d'entreprises promet de nouveaux horizons.",
        "Les taux d'intérêt augmentent, un impact négatif pour les investisseurs."
    };
    private int currentIndex = 0;

    public Animation warningAnimation;
    public GameObject lumiereAlarmeRougeGameObject;
    public float vitesseDApparition = 3f;

    void Start()
    {
        lumiereAlarmeRougeGameObject.SetActive(false);
        StartCoroutine(ShowNewsLoop());
    }

    IEnumerator ShowNewsLoop()
    {
        while (true)
        {
            // Mettre à jour le texte
            newsText.text = news[currentIndex];
            UpdateNewsText();

            Debug.Log(news[currentIndex]);

            // Attendre quelques secondes avant de passer à la suivante
            yield return new WaitForSeconds(vitesseDApparition);

            // Passer à la prochaine nouvelle
            currentIndex = (currentIndex + 1) % news.Length;
        }
    }

    void UpdateNewsText()
    {
        bool isGoodNews = news[currentIndex].Contains("croissance") || news[currentIndex].Contains("fusion");

        if (isGoodNews)
        {
            newsText.color = Color.green;
            int randomValue = Random.Range(5, 11);
            lumiereAlarmeRougeGameObject.gameObject.SetActive(false);
            Debug.Log("Bonne nouvelle! Valeur random: " + randomValue);
        }
        else
        {
            newsText.color = Color.red;
            int randomValue = Random.Range(1, 5);
            Debug.Log("Mauvaise nouvelle! Valeur random: " + randomValue);

            bool activateRedLight = (randomValue == 1 || randomValue == 2);

            if (warningAnimation != null)
            {
                if (activateRedLight)
                {
                    warningAnimation.Play();
                    lumiereAlarmeRougeGameObject.SetActive(true);
                    Debug.Log("Alerte! Animation de lumière rouge activée !");
                }
                else
                {
                    lumiereAlarmeRougeGameObject.SetActive(false);
                    Debug.Log("Pas d'alerte, disparition de la lumière rouge.");
                }
            }
        }
    }
}
