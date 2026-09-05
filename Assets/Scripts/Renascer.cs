using UnityEngine;

public class RespawnOnFall : MonoBehaviour
{
    public Transform respawnPoint;
    public float limboY = -10f;

    [Header("Reset de mecânicas ao cair (opcional)")]
    public AlavancaPonte[] alavancasPraResetar; // Arraste aqui os objetos "Botao_Magico" que devem voltar ao normal

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (transform.position.y < limboY)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.position = respawnPoint.position;
        rb.rotation = respawnPoint.rotation;
        rb.WakeUp();

        if (alavancasPraResetar != null)
        {
            foreach (var alavanca in alavancasPraResetar)
            {
                if (alavanca != null) alavanca.ResetarPonte();
            }
        }
    }
}
