using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Hit_Reg : MonoBehaviour
{
    // Optionnel : configuration via l'Inspector pour décider si le collider est un trigger
    [SerializeField]
    [Tooltip("Si vrai, le BoxCollider2D de ce GameObject doit être en mode Trigger")]
    private bool useTrigger = false;

    void Start()
    {
        // S'assure qu'un BoxCollider2D est présent et configure son mode si nécessaire
        var bc = GetComponent<BoxCollider2D>();
        if (bc != null)
        {
            bc.isTrigger = useTrigger;
        }
    }

    // Détecte les collisions physiques 2D (si le collider n'est pas en trigger)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider == null) return;

        // On considère qu'une "bullet de la pool" a le composant Bullet
        var bullet = collision.collider.GetComponent<Bullet>() ?? collision.gameObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            Debug.Log($"Hit_Reg : bullet détectée par '{gameObject.name}' -> {bullet.gameObject.name}");
            // Remet la bullet dans le pool en la désactivant
            bullet.gameObject.SetActive(false);
            // TODO: ajouter logique de dégâts / effets si nécessaire
        }
    }

    // Détecte les entrées trigger 2D (si le collider est en trigger)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;

        var bullet = other.GetComponent<Bullet>();
        if (bullet != null)
        {
            Debug.Log($"Hit_Reg (Trigger) : bullet détectée par '{gameObject.name}' -> {bullet.gameObject.name}");
            bullet.gameObject.SetActive(false);
            // TODO: ajouter logique de dégâts / effets si nécessaire
        }
    }
}
