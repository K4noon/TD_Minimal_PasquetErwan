using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class Enemy_SpawnTimer : MonoBehaviour
{
    [Header("Références")]
    [Tooltip("Transform utilisé comme position de spawn (Ennemy_Pool)")]
    public Transform ennemyPoolTransform;

    [Tooltip("Prefab de Ennemy_Triangle")]
    public GameObject ennemyTrianglePrefab;

    [Tooltip("Prefab de Ennemy_Base")]
    public GameObject ennemyBasePrefab;

    [Header("Contrôle du cycle")]
    [Tooltip("Nombre de fois à répéter le cycle complet. 1 = une exécution complète, 0 = aucun, -1 = infini")]
    public int cycles = 1;

    private void Start()
    {
        if (ennemyPoolTransform == null || ennemyTrianglePrefab == null || ennemyBasePrefab == null)
        {
            Debug.LogError("Enemy_SpawnTimer : assignez ennemyPoolTransform, ennemyTrianglePrefab et ennemyBasePrefab dans l'inspecteur.");
            enabled = false;
            return;
        }

        StartCoroutine(RunSpawnCycles());
    }

    private IEnumerator RunSpawnCycles()
    {
        int executed = 0;
        // Boucle : cycles times, -1 pour infini
        while (cycles < 0 || executed < cycles)
        {
            // Timer initial de 5 secondes
            yield return new WaitForSeconds(5f);

            // Instancie Ennemy_Triangle puis Ennemy_Base avec 0.5s d'intervalle
            GameObject a = SpawnAtPool(ennemyTrianglePrefab);
            yield return new WaitForSeconds(0.5f);
            GameObject b = SpawnAtPool(ennemyBasePrefab);

            // Une fois les deux instanciés, attend 2.5s avant de ré-instancier les deux
            yield return new WaitForSeconds(2.5f);

            GameObject c = SpawnAtPool(ennemyTrianglePrefab);
            yield return new WaitForSeconds(0.5f);
            GameObject d = SpawnAtPool(ennemyBasePrefab);

            // Relance un timer de 9 secondes puis annonce la 2e vague
            yield return new WaitForSeconds(9f);
            Debug.Log("prépare toi pour la 2e vague");

            // Attends 3 secondes puis instancie Ennemy_Base 3 fois avec 2s d'intervalle
            yield return new WaitForSeconds(3f);

            var secondWave = new List<GameObject>(3);
            for (int i = 0; i < 3; i++)
            {
                secondWave.Add(SpawnAtPool(ennemyBasePrefab));
                if (i < 2) yield return new WaitForSeconds(2f);
            }

            // Attend que les 3 ennemis soient détruits (ou désactivés). On vérifie jusqu'à ce que tous soient null/absents.
            yield return StartCoroutine(WaitForAllDestroyed(secondWave));

            // Annonce la vague finale et timer de 5s
            Debug.Log("prépare toi a la vague finale!");
            yield return new WaitForSeconds(5f);

            // Instancie 2 Ennemy_Base avec 0.5s d'intervalle puis 1 Ennemy_Triangle
            SpawnAtPool(ennemyBasePrefab);
            yield return new WaitForSeconds(0.5f);
            SpawnAtPool(ennemyBasePrefab);
            yield return new WaitForSeconds(0.5f);
            SpawnAtPool(ennemyTrianglePrefab);

            // Timer de 2 secondes avant de répéter le cycle
            yield return new WaitForSeconds(2f);

            executed++;
            // Si cycles < 0 la boucle continuera indéfiniment. Sinon si on a terminé toutes les répétitions on sort.
        }

        Debug.Log("Bravo, tu as gagné");
    }

    private GameObject SpawnAtPool(GameObject prefab)
    {
        return Instantiate(prefab, ennemyPoolTransform.position, Quaternion.identity);
    }

    private IEnumerator WaitForAllDestroyed(List<GameObject> spawned)
    {
        // Attend tant qu'au moins un objet n'est pas détruit (UnityEngine.Object == null après destruction)
        bool anyAlive;
        do
        {
            anyAlive = false;
            for (int i = 0; i < spawned.Count; i++)
            {
                if (spawned[i] != null)
                {
                    anyAlive = true;
                    break;
                }
            }
            if (anyAlive) yield return null;
        } while (anyAlive);
    }
}
