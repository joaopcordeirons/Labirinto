using UnityEngine;

public class AlavancaPonte : MonoBehaviour
{
    [Header("Referências")]
    public Rigidbody[] pontes;   // Todas as tábuas que vão travar (Rigidbody normal, não kinematic)
    public Rigidbody[] anchors;  // Opcional: deixe o elemento vazio (None) pra travar a tábua direto no chão/mundo

    [Header("Configuração")]
    public string playerTag = "Player";

    [Header("Feedback (opcional)")]
    public GameObject efeitoAtivacao;

    private bool ativada = false;
    private Vector3[] posicoesIniciais;
    private Quaternion[] rotacoesIniciais;

    void Start()
    {
        // Guarda a posição/rotação original de cada tábua, pra poder voltar depois
        posicoesIniciais = new Vector3[pontes.Length];
        rotacoesIniciais = new Quaternion[pontes.Length];

        for (int i = 0; i < pontes.Length; i++)
        {
            if (pontes[i] == null) continue;
            posicoesIniciais[i] = pontes[i].position;
            rotacoesIniciais[i] = pontes[i].rotation;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (ativada) return;
        if (!other.CompareTag(playerTag)) return;

        Ativar();
    }

    void Ativar()
    {
        ativada = true;

        for (int i = 0; i < pontes.Length; i++)
        {
            Rigidbody ponte = pontes[i];
            if (ponte == null) continue;

            // Se não tiver anchor nessa posição, o joint trava a tábua direto no mundo (chão)
            Rigidbody anchor = (anchors != null && i < anchors.Length) ? anchors[i] : null;

            // Liga a gravidade agora (antes ela tava desligada, por isso não caía) e trava com o Joint na mesma hora
            ponte.useGravity = true;
            ponte.linearVelocity = Vector3.zero;
            ponte.angularVelocity = Vector3.zero;

            FixedJoint joint = ponte.gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = anchor; // null = trava no mundo, sem precisar de outro objeto
            joint.breakForce = Mathf.Infinity;
            joint.breakTorque = Mathf.Infinity;
        }

        if (efeitoAtivacao != null)
            Instantiate(efeitoAtivacao, transform.position, Quaternion.identity);

        Debug.Log("Todas as tábuas foram travadas pela alavanca!");
    }

    // Chamado pelo RespawnOnFall quando o jogador cai no limbo: desfaz tudo e volta ao estado inicial
    public void ResetarPonte()
    {
        ativada = false;

        for (int i = 0; i < pontes.Length; i++)
        {
            Rigidbody ponte = pontes[i];
            if (ponte == null) continue;

            // Remove o Joint, se já tiver sido criado
            FixedJoint joint = ponte.GetComponent<FixedJoint>();
            if (joint != null) Destroy(joint);

            ponte.useGravity = false;
            ponte.linearVelocity = Vector3.zero;
            ponte.angularVelocity = Vector3.zero;
            ponte.position = posicoesIniciais[i];
            ponte.rotation = rotacoesIniciais[i];
        }

        Debug.Log("Tábuas resetadas ao estado inicial.");
    }
}
