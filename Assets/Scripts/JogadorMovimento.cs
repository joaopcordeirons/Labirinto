using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour // ← NOME DA CLASSE NÃO MUDE!
{
    // Variáveis públicas (renomeadas)
    public float velocidadeMovimento = 6f;
    public float sensibilidadeMouse = 0.08f;

    // Variáveis privadas (renomeadas e reorganizadas)
    private Rigidbody rigidbodyPlayer;
    private float rotacaoY; // antigo "yaw"
    private Vector3 direcaoEntrada; // antigo "moveInput"

    // Inicialização dos componentes
    private void Awake()
    {
        rigidbodyPlayer = GetComponent<Rigidbody>();
        rigidbodyPlayer.constraints = RigidbodyConstraints.FreezeRotation;
        rigidbodyPlayer.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void Start()
    {
        rotacaoY = transform.eulerAngles.y;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Leitura de inputs (antes era Update)
    private void Update()
    {
        LerRotacaoMouse();
        LerTeclasMovimento();
    }

    // Física (antes era FixedUpdate)
    private void FixedUpdate()
    {
        AplicarRotacaoPersonagem();
        AplicarMovimentoPersonagem();
    }

    // --- ROTAÇÃO ---
    private void LerRotacaoMouse()
    {
        if (Mouse.current == null) return;
        float deltaX = Mouse.current.delta.x.ReadValue();
        rotacaoY += deltaX * sensibilidadeMouse;
    }

    private void AplicarRotacaoPersonagem()
    {
        rigidbodyPlayer.MoveRotation(Quaternion.Euler(0f, rotacaoY, 0f));
    }

    // --- MOVIMENTO ---
    private void LerTeclasMovimento()
    {
        direcaoEntrada = Vector3.zero;
        Keyboard teclado = Keyboard.current;
        if (teclado == null) return;

        // Lógica WASD idêntica, só escrita de outro jeito
        if (teclado.wKey.isPressed) direcaoEntrada.z = 1f;
        if (teclado.sKey.isPressed) direcaoEntrada.z = -1f;
        if (teclado.aKey.isPressed) direcaoEntrada.x = -1f;
        if (teclado.dKey.isPressed) direcaoEntrada.x = 1f;
        
        direcaoEntrada.Normalize(); // Garante que a diagonal não fique mais rápida
    }

    private void AplicarMovimentoPersonagem()
    {
        // Calcula direção relativa à rotação do personagem
        Vector3 frente = transform.forward;
        Vector3 direita = transform.right;

        // Tira a inclinação vertical (para não voar ao olhar pra cima/baixo)
        frente.y = 0f;
        direita.y = 0f;
        frente.Normalize();
        direita.Normalize();

        // Soma os vetores de entrada
        Vector3 direcaoFinal = (frente * direcaoEntrada.z) + (direita * direcaoEntrada.x);
        direcaoFinal.Normalize();

        // Aplica a velocidade, mantendo a queda (gravidade) intacta
        Vector3 novaVelocidade = direcaoFinal * velocidadeMovimento;
        novaVelocidade.y = rigidbodyPlayer.linearVelocity.y; // preserva a gravidade
        rigidbodyPlayer.linearVelocity = novaVelocidade;
    }
}