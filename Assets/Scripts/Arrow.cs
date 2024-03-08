using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    GameManager gameManager;

    void Awake()
    {
        gameManager = GameManager.GetInstance();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 relativePos = gameManager.ball.transform.position - transform.position;

        Quaternion rotation = Quaternion.LookRotation(relativePos) * Quaternion.Euler(90, 90, 0);

        transform.rotation = rotation;
    }
}
