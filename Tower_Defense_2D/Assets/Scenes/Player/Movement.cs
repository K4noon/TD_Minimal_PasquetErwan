using UnityEngine;

public class Movement : MonoBehaviour
{
    public float Speed = 10;
    private float Jump = 0;
    public float JumpForce = 1;
    public Rigidbody2D rb;

    // Check si touche le sol
    public bool bGrounded = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject); 
        //collision.gameObject
        if (collision.collider.name == "Ground")
        {
            //Debug.Log("entre");
            bGrounded = true; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        float translation = Input.GetAxis("Horizontal") * Speed; //recupere les inputs
        translation *= Time.deltaTime;
        transform.Translate(translation, 0, 0); //deplace le joueur


        if (Input.GetAxis("Jump") !=0 && bGrounded) //si appuie sur espace et touche le sol
        {
            Jump = Input.GetAxis("Jump") * JumpForce; 
            rb.AddForce(new Vector3(0, Jump, 0), ForceMode2D.Impulse); //ajoute une force vers le haut
            bGrounded = false; 
        }

        
        
    }
}
