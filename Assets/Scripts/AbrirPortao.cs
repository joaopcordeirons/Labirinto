using UnityEngine;

public class AbrirPortao : MonoBehaviour
{
    [Header("Referência")]
    public HingeJoint portao; // O portão/barreira que tem o Hinge Joint

    [Header("Configuração")]
    public float anguloAberto = 90f; // Pra qual ângulo o portão gira ao abrir
    public string playerTag = "Player";

    [Header("Feedback (opcional)")]
    public GameObject efeitoAtivacao;

    private bool ativado = false;
    private float anguloFechado;

    void Start()
    {
        // Guarda o ângulo de fechado original (pra poder resetar depois)
        anguloFechado = portao.spring.targetPosition;
    }

    void OnTriggerEnter(Collider other)
    {
        if (ativado) return;
        if (!other.CompareTag(playerTag)) return;

        Ativar();
    }

    void Ativar()
    {
        ativado = true;

        JointSpring spring = portao.spring;
        spring.targetPosition = anguloAberto;
        portao.spring = spring;

        if (efeitoAtivacao != null)
            Instantiate(efeitoAtivacao, transform.position, Quaternion.identity);

        Debug.Log("Portão abrindo!");
    }

    // Chamado pelo RespawnOnFall quando o jogador cai no limbo: fecha o portão de novo
    public void ResetarPortao()
    {
        ativado = false;

        JointSpring spring = portao.spring;
        spring.targetPosition = anguloFechado;
        portao.spring = spring;

        Debug.Log("Portão fechado novamente.");
    }
}
