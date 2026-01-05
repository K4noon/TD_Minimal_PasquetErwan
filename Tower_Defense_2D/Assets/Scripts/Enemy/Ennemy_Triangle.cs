using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Ennemy_Triangle : Ennemy_Base
{
    [SerializeField] private float jumpDelay = 1.5f;
    [SerializeField] private float jumpForce = 5f;

    private Rigidbody2D rb;
    private Coroutine jumpCoroutine;
    private bool isTouchingGround;
    private bool hasJumpedThisContact;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Ajuste les statistiques héritées : légèrement moins rapide, HP à la moitié
        HorizontalVelocity = Mathf.Max(1, (int)(HorizontalVelocity * 0.8f)); // légèrement moins rapide
        HP = HP / 2; // moitié des PV
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Appelle la logique de base (ex: détection des projectiles)
        base.OnCollisionEnter2D(collision);

        // Vérifie que la collision concerne le GameObject nommé "Ground"
        // et que les deux colliders sont des BoxCollider2D
        if (collision.collider != null
            && collision.collider.name == "Ground"
            && collision.collider is BoxCollider2D
            && collision.otherCollider is BoxCollider2D)
        {
            isTouchingGround = true;
            hasJumpedThisContact = false;

            if (jumpCoroutine == null)
                jumpCoroutine = StartCoroutine(JumpAfterDelay());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Si on quitte le Ground, annule la temporisation et réinitialise l'état
        if (collision.collider != null
            && collision.collider.name == "Ground"
            && collision.collider is BoxCollider2D
            && collision.otherCollider is BoxCollider2D)
        {
            isTouchingGround = false;
            hasJumpedThisContact = false;

            if (jumpCoroutine != null)
            {
                StopCoroutine(jumpCoroutine);
                jumpCoroutine = null;
            }
        }
    }

    private IEnumerator JumpAfterDelay()
    {
        float elapsed = 0f;
        while (elapsed < jumpDelay)
        {
            // Si on perd le contact avant la fin du délai, on annule
            if (!isTouchingGround)
            {
                jumpCoroutine = null;
                yield break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Si on est toujours en contact et qu'on n'a pas encore sauté pour ce contact : impulsion
        if (isTouchingGround && !hasJumpedThisContact && rb != null)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            hasJumpedThisContact = true;
        }

        jumpCoroutine = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
