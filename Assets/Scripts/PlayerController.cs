using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour {

    public enum AIType
    {
        RANDOM,
        FUZZY,
        FUZZY_BLACKBOARD,
    }

    public enum Acao
    {
        DEFENDER,
        APROXIMAR,
        NADA,
    }

    public enum Velocidade
    {
        NORMAL,
        RAPIDA,
        NENHUMA,
    }
    
    public AIType aiType;

    Rigidbody rigidbody;
    Blackboard blackBoard;

    float distanceToBall; // distancia ate a bola
    float distanceToGoal; // distancia ate o gol objetivo
    float distanceToDefend; // distancia ate o gol para defender

    Vector3 direcaoAteBola; // vetor direcao entre jogador e bola
    Vector3 direcaoAteObjetivo; // vetor direcao entre jogador e gol objetivo
    Vector3 direcaoAteDefender; // vetor direcao entre jogador e gol para defender

    float goalBallDot; // < 0 siginifica que o jogador esta entre a bola e o objetivo (aka que a bola esta atras do jogador)
    bool attackSide; // esta no lado do ataque
    bool defendSide; // esta no lado da defesa
    bool bolaNoCentro; // indica se a bola esta no meio de campo
    bool poucoJogadorDefendendo; // indica se jogadoresDefendendo < jogadoresAproximando

    // blackboard info
    int jogadoresAproximando = 0;
    int jogadoresDefendendo = 0;

    public bool acaoEncerrada;
    public Acao acao = Acao.NADA;
    public bool pular;
    public Velocidade velocidade;
    public bool pertoDoObjetivo;
    public bool ladoDoAtaque;
    public bool pertoDaBola;
    public bool emDirecaoAoObjetivo;

    TextMesh debugText = null;

    void Start()
    {
        acaoEncerrada = true;
        rigidbody = GetComponent<Rigidbody>();
        blackBoard = GetComponentInParent<Blackboard>();
        debugText = GetComponentInChildren<TextMesh>();
    }

    void SaveAction(Acao acao, bool pular, Velocidade velocidade)
    {
        this.acao = acao;
        this.pular = pular;
        this.velocidade = velocidade;
    }

    void ExecuteAction()
    {
        float div = Random.Range(3, 6);

        float force = 1000.0f / div;

        if (velocidade == Velocidade.NENHUMA)
            force = 0;
        else if (velocidade == Velocidade.RAPIDA)
            force = 6000.0f / div;

        Vector3 vecForce = new Vector3(0, 0, 0);

        if (acao == Acao.APROXIMAR)
        {
            //if (distanceToBall < 2)
                acaoEncerrada = true;

            vecForce = direcaoAteBola * force;
            //vecForce.y = 0;
        }
        else if (acao == Acao.DEFENDER)
        {
            //if (distanceToDefend < 2)
                acaoEncerrada = true;

            vecForce = direcaoAteDefender * force;
            //vecForce.y = 0;
        }

        if (pular && transform.position.y < 1.0f && distanceToBall < 5)
        {
            vecForce.y = 5000.0f;
        }

        rigidbody.AddForce(vecForce);
    }

    void Reevaluate()
    {
        acaoEncerrada = false;

        if (aiType == AIType.RANDOM)
        {
            acaoEncerrada = true;

            float force = Random.Range(1000.0f, 5000.0f);

            //transform.LookAt(ball.transform);
            //rigidbody.AddRelativeForce(0, 0, 10000.0f);

            //direcaoAteBola.y = 0;
            rigidbody.AddForce(direcaoAteBola * force);
        }
        else if (aiType == AIType.FUZZY)
        {
            if (!bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && !pertoDaBola && !emDirecaoAoObjetivo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);
            }
            else if (!bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && !pertoDaBola && emDirecaoAoObjetivo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);
            }
            else if (!bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && pertoDaBola && !emDirecaoAoObjetivo)
            {
                SaveAction(Acao.APROXIMAR, true, Velocidade.NORMAL);
            }
            else if (!bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && pertoDaBola && emDirecaoAoObjetivo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.RAPIDA);
            }

            else if (bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && !pertoDaBola && !emDirecaoAoObjetivo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);
            }
            else if (bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && !pertoDaBola && emDirecaoAoObjetivo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.NORMAL);
            }
            else if (bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && pertoDaBola && !emDirecaoAoObjetivo)
            {
                SaveAction(Acao.APROXIMAR, true, Velocidade.NORMAL);
            }
            else if (bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && pertoDaBola && emDirecaoAoObjetivo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.RAPIDA);
            }

            else if (!pertoDoObjetivo && ladoDoAtaque && !pertoDaBola && !emDirecaoAoObjetivo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.NORMAL);
            }
            else if (!pertoDoObjetivo && ladoDoAtaque && !pertoDaBola && emDirecaoAoObjetivo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.NORMAL);
            }
            else if (!pertoDoObjetivo && ladoDoAtaque && pertoDaBola && !emDirecaoAoObjetivo)
            {
                SaveAction(Acao.APROXIMAR, true, Velocidade.NORMAL);
            }
            else if (!pertoDoObjetivo && ladoDoAtaque && pertoDaBola && emDirecaoAoObjetivo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.NORMAL);
            }
            else if (pertoDoObjetivo && ladoDoAtaque && !pertoDaBola && !emDirecaoAoObjetivo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.NORMAL);
            }
            else if (pertoDoObjetivo && ladoDoAtaque && !pertoDaBola && emDirecaoAoObjetivo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.NORMAL);
            }
            else if (pertoDoObjetivo && ladoDoAtaque && pertoDaBola && !emDirecaoAoObjetivo)
            {
                SaveAction(Acao.APROXIMAR, true, Velocidade.NORMAL);
            }
            else if (pertoDoObjetivo && ladoDoAtaque && pertoDaBola && emDirecaoAoObjetivo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.RAPIDA);
            }
        }
        else if (aiType == AIType.FUZZY_BLACKBOARD)
        {
            /*if (!bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && !pertoDaBola && !emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);
            }
            else if (!bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && !pertoDaBola && !emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, true, Velocidade.NORMAL);
            }
            else if (!bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && !pertoDaBola && emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);
            }
            else if (!bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && !pertoDaBola && emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.NORMAL);
            }
            else if (!bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && pertoDaBola && !emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.DEFENDER, true, Velocidade.NORMAL);
            }
            else if (!bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && pertoDaBola && !emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, true, Velocidade.NORMAL);
            }
            else if (!bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && pertoDaBola && emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);
            }
            else if (!bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && pertoDaBola && emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.RAPIDA);
            }
            else if (bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && !pertoDaBola && !emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);
            }
            else if (bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && !pertoDaBola && !emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, true, Velocidade.NORMAL);
            }
            else if (bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && !pertoDaBola && emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);
            }
            else if (bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && !pertoDaBola && emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.NORMAL);
            }
            else if (bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && pertoDaBola && !emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);
            }
            else if (bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && pertoDaBola && !emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, true, Velocidade.NORMAL);
            }
            else if (bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && pertoDaBola && emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);
            }
            else if (bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && pertoDaBola && emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.RAPIDA);
            }
            else if (!pertoDoObjetivo && ladoDoAtaque && !pertoDaBola && !emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);
            }
            else if (!pertoDoObjetivo && ladoDoAtaque && !pertoDaBola && !emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, true, Velocidade.NORMAL);
            }
            else if (!pertoDoObjetivo && ladoDoAtaque && !pertoDaBola && emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);
            }
            else if (!pertoDoObjetivo && ladoDoAtaque && !pertoDaBola && emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.NORMAL);
            }
            else if (!pertoDoObjetivo && ladoDoAtaque && pertoDaBola && !emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);
            }
            else if (!pertoDoObjetivo && ladoDoAtaque && pertoDaBola && !emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, true, Velocidade.NORMAL);
            }
            else if (!pertoDoObjetivo && ladoDoAtaque && pertoDaBola && emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);// *
            }
            else if (!pertoDoObjetivo && ladoDoAtaque && pertoDaBola && emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.NORMAL);
            }
            else if (pertoDoObjetivo && ladoDoAtaque && !pertoDaBola && !emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);
            }
            else if (pertoDoObjetivo && ladoDoAtaque && !pertoDaBola && !emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, true, Velocidade.NORMAL);
            }
            else if (pertoDoObjetivo && ladoDoAtaque && !pertoDaBola && emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);
            }
            else if (pertoDoObjetivo && ladoDoAtaque && !pertoDaBola && emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.NORMAL);
            }
            else if (pertoDoObjetivo && ladoDoAtaque && pertoDaBola && !emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);
            }
            else if (pertoDoObjetivo && ladoDoAtaque && pertoDaBola && !emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, true, Velocidade.NORMAL);
            }
            else if (pertoDoObjetivo && ladoDoAtaque && pertoDaBola && emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.RAPIDA);
            }
            else if (pertoDoObjetivo && ladoDoAtaque && pertoDaBola && emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.RAPIDA);
            }*/

            if (!bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && !pertoDaBola && !emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.NORMAL);
            }
            else if (!bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && !pertoDaBola && !emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, true, Velocidade.NORMAL);
            }
            else if (!bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && !pertoDaBola && emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.NORMAL);
            }
            else if (!bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && !pertoDaBola && emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.NORMAL);
            }
            else if (!bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && pertoDaBola && !emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, true, Velocidade.NORMAL);
            }
            else if (!bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && pertoDaBola && !emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, true, Velocidade.NORMAL);
            }
            else if (!bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && pertoDaBola && emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.NORMAL);
            }
            else if (!bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && pertoDaBola && emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.RAPIDA);
            }
            else if (bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && !pertoDaBola && !emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);
            }
            else if (bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && !pertoDaBola && !emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, true, Velocidade.NORMAL);
            }
            else if (bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && !pertoDaBola && emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);
            }
            else if (bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && !pertoDaBola && emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.NORMAL);
            }
            else if (bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && pertoDaBola && !emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);
            }
            else if (bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && pertoDaBola && !emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, true, Velocidade.NORMAL);
            }
            else if (bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && pertoDaBola && emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);
            }
            else if (bolaNoCentro && !pertoDoObjetivo && !ladoDoAtaque && pertoDaBola && emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.RAPIDA);
            }
            else if (!pertoDoObjetivo && ladoDoAtaque && !pertoDaBola && !emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);
            }
            else if (!pertoDoObjetivo && ladoDoAtaque && !pertoDaBola && !emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, true, Velocidade.NORMAL);
            }
            else if (!pertoDoObjetivo && ladoDoAtaque && !pertoDaBola && emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);
            }
            else if (!pertoDoObjetivo && ladoDoAtaque && !pertoDaBola && emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.NORMAL);
            }
            else if (!pertoDoObjetivo && ladoDoAtaque && pertoDaBola && !emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);
            }
            else if (!pertoDoObjetivo && ladoDoAtaque && pertoDaBola && !emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, true, Velocidade.NORMAL);
            }
            else if (!pertoDoObjetivo && ladoDoAtaque && pertoDaBola && emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);// *
            }
            else if (!pertoDoObjetivo && ladoDoAtaque && pertoDaBola && emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.NORMAL);
            }
            else if (pertoDoObjetivo && ladoDoAtaque && !pertoDaBola && !emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);
            }
            else if (pertoDoObjetivo && ladoDoAtaque && !pertoDaBola && !emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, true, Velocidade.NORMAL);
            }
            else if (pertoDoObjetivo && ladoDoAtaque && !pertoDaBola && emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);
            }
            else if (pertoDoObjetivo && ladoDoAtaque && !pertoDaBola && emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.NORMAL);
            }
            else if (pertoDoObjetivo && ladoDoAtaque && pertoDaBola && !emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.DEFENDER, false, Velocidade.NORMAL);
            }
            else if (pertoDoObjetivo && ladoDoAtaque && pertoDaBola && !emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, true, Velocidade.NORMAL);
            }
            else if (pertoDoObjetivo && ladoDoAtaque && pertoDaBola && emDirecaoAoObjetivo && poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.RAPIDA);
            }
            else if (pertoDoObjetivo && ladoDoAtaque && pertoDaBola && emDirecaoAoObjetivo && !poucoJogadorDefendendo)
            {
                SaveAction(Acao.APROXIMAR, false, Velocidade.RAPIDA);
            }
        }
    }

    void FixedUpdate()
    {
        var goal = blackBoard.goal;
        var defend = blackBoard.defend;
        var ball = blackBoard.ball;

        float goalsDistance = (goal.transform.position - defend.transform.position).magnitude;

        distanceToBall = (ball.transform.position - transform.position).magnitude;
        distanceToGoal = (goal.transform.position - transform.position).magnitude;
        distanceToDefend = (defend.transform.position - transform.position).magnitude;

        direcaoAteBola = (ball.transform.position - transform.position).normalized;
        direcaoAteObjetivo = (goal.transform.position - transform.position).normalized;
        direcaoAteDefender = (defend.transform.position - transform.position).normalized;

        goalBallDot = Vector3.Dot(direcaoAteObjetivo, ball.transform.position - transform.position);

        attackSide = distanceToGoal < distanceToDefend;
        defendSide = distanceToGoal >= distanceToDefend;

        bolaNoCentro = Mathf.Min((goal.transform.position - ball.transform.position).magnitude, (defend.transform.position - ball.transform.position).magnitude) > goalsDistance / 4;

        pertoDoObjetivo = distanceToGoal <= 25;
        ladoDoAtaque = distanceToGoal <= distanceToDefend;
        pertoDaBola = distanceToBall <= 6;
        emDirecaoAoObjetivo = Vector3.Dot(direcaoAteObjetivo, ball.transform.position - transform.position) > 0.5f;

        jogadoresAproximando = 0;
        jogadoresDefendendo = 0;

        foreach (var player in blackBoard.players)
        {
            if (player.gameObject == this.gameObject)
                continue;

            if (player.acao == Acao.APROXIMAR)
                jogadoresAproximando++;
            else if (player.acao == Acao.DEFENDER)
                jogadoresDefendendo++;
        }

        poucoJogadorDefendendo = jogadoresDefendendo < jogadoresAproximando;

        if (acaoEncerrada)
        {
            Reevaluate();
        }

        ExecuteAction();
    }

    void Update()
    {
        if (debugText != null)
            debugText.text = "";//acao.ToString().Substring(0, 4) + "\n" + pular.ToString().Substring(0, 4) + "\n" + velocidade.ToString().Substring(0, 4);
    }
}
