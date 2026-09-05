using UnityEngine;

public class AlavancaPorta : MonoBehaviour
{
    [SerializeField] private Transform porta;
    [SerializeField] private Vector3 deslocamentoAberta = new Vector3(0f, 3f, 0f);
    [SerializeField] private float velocidade = 3f;
    [SerializeField] private float anguloAtivacao = 25f;

    private Vector3 posicaoFechada;
    private bool ativada;

    private void Start()
    {
        if (porta != null)
            posicaoFechada = porta.position;
    }

    private void Update()
    {
        if (porta == null)
            return;

        Vector3 destino = ativada
            ? posicaoFechada + deslocamentoAberta
            : posicaoFechada;

        porta.position = Vector3.Lerp(
            porta.position,
            destino,
            velocidade * Time.deltaTime
        );

        float angulo = Mathf.Abs(Mathf.DeltaAngle(0f, transform.localEulerAngles.x));

        if (angulo > 180f)
            angulo = 360f - angulo;

        if (angulo >= anguloAtivacao)
            ativada = true;
    }
}