using UnityEngine;

public class CatapultaMola : MonoBehaviour
{
    [Header("Configuração")]
    public string playerTag = "Player";

    private Rigidbody rb;
    private bool ativada = false;
    private Vector3 posicaoInicial;
    private Quaternion rotacaoInicial;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        posicaoInicial = rb.position;
        rotacaoInicial = rb.rotation;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (ativada) return;
        if (!collision.gameObject.CompareTag(playerTag)) return;

        Ativar();
    }

    void Ativar()
    {
        ativada = true;

        // Libera a física: o Spring Joint puxa a plataforma pro alto na mesma hora, lançando quem estiver em cima
        rb.isKinematic = false;

        Debug.Log("Catapulta ativada!");
    }

    // Chamado pelo RespawnOnFall quando o jogador cai no limbo: volta a plataforma pro lugar e trava de novo
    public void ResetarCatapulta()
    {
        ativada = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.position = posicaoInicial;
        rb.rotation = rotacaoInicial;
        rb.isKinematic = true;
    }
}
