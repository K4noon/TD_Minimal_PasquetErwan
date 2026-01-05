using UnityEngine;

public class Ennemy_Base : MonoBehaviour
{
    public int HP = 100;
    public int HorizontalVelocity = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider != null && collision.collider.CompareTag("Bullet"))
        {
            // Récupère le composant Bullet pour lire la valeur DMG
            Bullet bullet = collision.collider.GetComponent<Bullet>();
            if (bullet != null)
            {
                HP -= bullet.DMG;
                if (HP < 0) HP = 0;

                // Désactive la balle pour éviter des collisions répétées (pooling / réutilisation)
                collision.collider.gameObject.SetActive(false);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
   if (HP <= 0)
        {
            Destroy(gameObject);
        }
        transform.Translate(HorizontalVelocity * Time.deltaTime * Vector2.left);
    }
   
}
