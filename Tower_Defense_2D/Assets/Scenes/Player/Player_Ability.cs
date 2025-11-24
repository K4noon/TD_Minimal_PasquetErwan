using System;
using UnityEngine;


public class Player_Ability : MonoBehaviour
{
    public GameObject pool;
    public Bullet[] bulletsCol;

    int nCurrentBullet = 0;

    private void Start()
    {
       bulletsCol = new Bullet [pool.transform.childCount];
        for (int i=0; i < bulletsCol.Length; i++)
        {
            bulletsCol[i] = pool.transform.GetChild(i).GetComponent<Bullet>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) // Check if the left mouse button is pressed
        {
            bulletsCol[nCurrentBullet].shoot(transform.position);
            nCurrentBullet++;  
            if (nCurrentBullet >= bulletsCol.Length)
            {
                nCurrentBullet = 0;
            }
        }
    }
}
