using UnityEngine;

public static class UnityExtensionMethods
{
    /// <summary>
    /// Permet de vérifier si un objet Unity est détruit correctement.
    /// </summary>
    public static bool IsDestroyed(this Object obj)
    {
        return obj == null || obj.Equals(null);
    }
}
