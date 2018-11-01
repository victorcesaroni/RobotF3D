using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public GameObject ball;
    public GameObject defend;
    public GameObject goal;

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
    
    float distanceToBall; // distancia ate a bola
    float distanceToGoal; // distancia ate o gol objetivo
    float distanceToDefend; // distancia ate o gol para defender

    Vector3 direcaoAteBola; // vetor direcao entre jogador e bola
    Vector3 direcaoAteObjetivo; // vetor direcao entre jogador e gol objetivo
    Vector3 direcaoAteDefender; // vetor direcao entre jogador e gol para defender

    float goalBallDot; // < 0 siginifica que o jogador esta entre a bola e o objetivo (aka que a bola esta atras do jogador)
    bool attackSide; // esta no lado do ataque
    bool defendSide; // esta no lado da defesa

    void Start () {
        rigidbody = GetComponent<Rigidbody>();
	}

    public Acao ultimaAcao;
    public bool ultimoPular;
    public Velocidade ultimaVelocidade;
    public bool pertoDoObjetivo;
    public bool ladoDoAtaque;
    public bool pertoDaBola;
    public bool emDirecaoAoObjetivo;

    void ExecutaAcao(Acao acao, bool pular, Velocidade velocidade)
    {
        ultimaAcao = acao;
        ultimoPular = pular;
        ultimaVelocidade = velocidade;

        float div = 5;

        float force = 1000.0f / div;

        if (velocidade == Velocidade.NENHUMA)
            force = 0;
        else if (velocidade == Velocidade.RAPIDA)
            force = 6000.0f / div;

        Vector3 vecForce = direcaoAteBola * force;

        vecForce.y = 0;

        if (pular && transform.position.y < 1.0f)
        {
             vecForce.y = 5000.0f;
        }

        if (acao == Acao.APROXIMAR)
        {
            rigidbody.AddForce(vecForce);
        }
        else if (acao == Acao.DEFENDER)
        {
            rigidbody.AddForce(vecForce);
        }
    }

    void FixedUpdate()
    {
        distanceToBall = (ball.transform.position - transform.position).magnitude;
        distanceToGoal = (goal.transform.position - transform.position).magnitude;
        distanceToDefend = (defend.transform.position - transform.position).magnitude;
        
        direcaoAteBola = (ball.transform.position - transform.position).normalized; 
        direcaoAteObjetivo = (goal.transform.position - transform.position).normalized; 
        direcaoAteDefender = (defend.transform.position - transform.position).normalized; 

        goalBallDot = Vector3.Dot(direcaoAteObjetivo, ball.transform.position - transform.position); 

        attackSide = distanceToGoal < distanceToDefend; 
        defendSide = distanceToGoal >= distanceToDefend; 

        pertoDoObjetivo = distanceToGoal < 25;
        ladoDoAtaque = distanceToGoal < distanceToDefend;
        pertoDaBola = distanceToBall < 6;
        emDirecaoAoObjetivo = Vector3.Dot(direcaoAteObjetivo, ball.transform.position - transform.position) > 0.5f;

        if (aiType == AIType.RANDOM)
        {
            if (Random.Range(1, 5) == 1)
            {
                float force = Random.Range(1000.0f, 5000.0f);

                //transform.LookAt(ball.transform);
                //rigidbody.AddRelativeForce(0, 0, 10000.0f);

                direcaoAteBola.y = 0;
                rigidbody.AddForce(direcaoAteBola * force);
            }
        }
        else if (aiType == AIType.FUZZY)
        {
            //if (Random.Range(1, 5) == 1)
            {
                if (!pertoDoObjetivo && !ladoDoAtaque && !pertoDaBola && !emDirecaoAoObjetivo)
                {
                    ExecutaAcao(Acao.DEFENDER, false, Velocidade.NORMAL);
                }
                else if (!pertoDoObjetivo && !ladoDoAtaque && !pertoDaBola && emDirecaoAoObjetivo)
                {
                    ExecutaAcao(Acao.DEFENDER, false, Velocidade.NORMAL);
                }
                else if (!pertoDoObjetivo && !ladoDoAtaque && pertoDaBola && !emDirecaoAoObjetivo)
                {
                    ExecutaAcao(Acao.APROXIMAR, true, Velocidade.NORMAL);
                }
                else if (!pertoDoObjetivo && !ladoDoAtaque && pertoDaBola && emDirecaoAoObjetivo)
                {
                    ExecutaAcao(Acao.APROXIMAR, false, Velocidade.RAPIDA);
                }
                else if (!pertoDoObjetivo && ladoDoAtaque && !pertoDaBola && !emDirecaoAoObjetivo)
                {
                    ExecutaAcao(Acao.APROXIMAR, false, Velocidade.NORMAL);
                }
                else if (!pertoDoObjetivo && ladoDoAtaque && !pertoDaBola && emDirecaoAoObjetivo)
                {
                    ExecutaAcao(Acao.APROXIMAR, false, Velocidade.NORMAL);
                }
                else if (!pertoDoObjetivo && ladoDoAtaque && pertoDaBola && !emDirecaoAoObjetivo)
                {
                    ExecutaAcao(Acao.APROXIMAR, true, Velocidade.NORMAL);
                }
                else if (!pertoDoObjetivo && ladoDoAtaque && pertoDaBola && emDirecaoAoObjetivo)
                {
                    ExecutaAcao(Acao.APROXIMAR, false, Velocidade.NORMAL);
                }
                else if (pertoDoObjetivo && ladoDoAtaque && !pertoDaBola && !emDirecaoAoObjetivo)
                {
                    ExecutaAcao(Acao.APROXIMAR, false, Velocidade.NORMAL);
                }
                else if (pertoDoObjetivo && ladoDoAtaque && !pertoDaBola && emDirecaoAoObjetivo)
                {
                    ExecutaAcao(Acao.APROXIMAR, false, Velocidade.NORMAL);
                }
                else if (pertoDoObjetivo && ladoDoAtaque && pertoDaBola && !emDirecaoAoObjetivo)
                {
                    ExecutaAcao(Acao.APROXIMAR, true, Velocidade.NORMAL);
                }
                else if (pertoDoObjetivo && ladoDoAtaque && pertoDaBola && emDirecaoAoObjetivo)
                {
                    ExecutaAcao(Acao.APROXIMAR, false, Velocidade.RAPIDA);
                }
            }
        }
    }
}
