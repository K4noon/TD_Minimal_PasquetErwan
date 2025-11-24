using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Décalage vers la droite (en unités Unity) appliqué à la position de tir")]
    private float rightOffset = 0.2f;

    [Header("Mouvement")]
    [SerializeField]
    [Tooltip("Vitesse horizontale de base de la balle")]
    private float baseSpeed = 5f;

    [SerializeField]
    [Tooltip("Boost initial ajouté à la vitesse horizontale lors du tir")]
    private float initialBoost = 4f;

    [SerializeField]
    [Tooltip("Taux de décroissance du boost (unités par seconde)")]
    private float boostDecay = 2f;

    private Rigidbody2D rb;
    private float currentBoost = 0f;
    private bool isShot = false;

    // Wrapper pour utiliser "linearVelocity" à la place de Rigidbody2D.velocity
    private Vector2 linearVelocity
    {
        get => rb != null ? rb.linearVelocity : Vector2.zero;
        set { if (rb != null) rb.linearVelocity = value; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Positionne la balle et active le boost horizontal décroissant
    public void shoot(Vector3 posPlayer)
    {
        // Réactive la balle si elle était désactivée (pooling)
        gameObject.SetActive(true);

        // Positionne avec un léger offset vers la droite (axe X monde)
        transform.position = posPlayer + Vector3.right * rightOffset;

        // Initialise le boost et l'état de tir
        currentBoost = initialBoost;
        isShot = true;

        // Applique immédiatement la vitesse horizontale (préserve la vitesse verticale actuelle)
        linearVelocity = new Vector2(baseSpeed + currentBoost, linearVelocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isShot) return;

        if (currentBoost > 0f)
        {
            // Diminue le boost au fil du temps
            currentBoost -= boostDecay * Time.deltaTime;
            if (currentBoost < 0f) currentBoost = 0f;
        }

        // Met à jour la composante horizontale de la vélocité via linearVelocity
        linearVelocity = new Vector2(baseSpeed + currentBoost, linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Quand la balle touche l'objet nommé "Ground", on la réinitialise et on la range (désactive)
        if (collision.collider != null && collision.collider.name == "Ground")
        {
            // Arrête la balle et réinitialise l'état
            isShot = false;
            currentBoost = 0f;
            linearVelocity = Vector2.zero;

            // Réinitialise l'axe/pivot à plat avant de retourner au pool
            ResetPivotToFlat();

            // Désactive l'objet pour qu'il soit considéré comme retourné au pool
            gameObject.SetActive(false);
        }
    }

    // S'assure aussi que si l'objet est désactivé depuis ailleurs, son pivot est réinitialisé
    private void OnDisable()
    {
        ResetPivotToFlat();
    }

    // Remet la rotation locale à plat (utile pour éviter les déviations)
    private void ResetPivotToFlat()
    {
        // Remet la rotation locale à zéro (plan XY plat pour 2D -> rotation Z = 0)
        transform.rotation = Quaternion.identity;
        transform.localEulerAngles = Vector3.zero;
    }
}
