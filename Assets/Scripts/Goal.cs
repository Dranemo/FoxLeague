using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public enum PlayerGoal { Player_1,Player_2 }
    [SerializeField] private PlayerGoal playerGoal;

    public void SetGoal(PlayerGoal _playerGoal)
    {
        playerGoal = _playerGoal;
    }

    private void OnTriggerEnter(Collider other)
    {

        //Debug.Log("1 uo 2 ???");
        if (other.CompareTag("Ball"))
        {
            if (playerGoal == PlayerGoal.Player_2)
            {
                StartCoroutine(ScoreManager.GetInstance().Goal(1, other.gameObject));
            }
            else if (playerGoal == PlayerGoal.Player_1)
            {
                StartCoroutine(ScoreManager.GetInstance().Goal(2, other.gameObject));
            }
        }

    }

    private void Awake()
    {
        // Trouver le GameObject "Cube" à partir de l'objet actuel (ce script est attaché à un GameObject)
        Transform cubeTransform = gameObject.transform.Find("GOAL/Cube");

        // Vérifier si le GameObject "Cube" a été trouvé
        if (cubeTransform != null)
        {
            // Accéder au composant MeshRenderer du GameObject "Cube"
            MeshRenderer cubeRenderer = cubeTransform.GetComponent<MeshRenderer>();

            // Vérifier si le composant MeshRenderer a été trouvé
            if (cubeRenderer != null)
            {
                // Accéder à la taille du Bounds du MeshRenderer
                Vector3 size = cubeRenderer.bounds.size;

                // Afficher la taille dans la console
                Debug.Log("Taille du Cube : " + size);
            }
            else
            {
                Debug.LogError("Le composant MeshRenderer n'a pas été trouvé sur le Cube.");
            }
        }
        else
        {
            Debug.LogError("Le GameObject 'Cube' n'a pas été trouvé sous 'GOAL'.");
        }
    }
}
