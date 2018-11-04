using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blackboard : MonoBehaviour
{
    public enum BlackBoardData
    {
        POSICAO_BOLA,
        POSICAO_DEFENDER,
        POSICAO_OBJETIVO,
        MINHA_ACAO,
    }

    public enum BlackBoardInfo
    {
        NUMERO_APROXIMANDO,
        NUMERO_DEFENDENDO,
    };
    
    Dictionary<GameObject, PlayerController.Acao> acao = new Dictionary<GameObject, PlayerController.Acao>();
    Vector3 posicaoBola;
    Vector3 posicaoDefender;
    Vector3 posicaoObjetivo;
    
    public class RequestResponse
    {
        public bool succes = false;
    }

    public class RequestResponse<T> : RequestResponse
    {
        public RequestResponse(T value)
        {
            this.value = value;
            this.succes = true;
        }

        public T value;
    }

    public class UpdateRequest
    {
    }

    public class UpdateRequest<T> : UpdateRequest
    {
        public UpdateRequest(T value)
        {
            this.value = value;
        }
        public T value;
    }

    public RequestResponse RequestData(PlayerController me, BlackBoardData data)
    {
        if (data == BlackBoardData.POSICAO_BOLA)
            return new RequestResponse<Vector3>(posicaoBola);
        if (data == BlackBoardData.POSICAO_DEFENDER)
            return new RequestResponse<Vector3>(posicaoDefender);
        if (data == BlackBoardData.POSICAO_OBJETIVO)
            return new RequestResponse<Vector3>(posicaoObjetivo);

        throw new System.Exception("Invalid BlackBoardData");
    }

    public RequestResponse RequestInfo(PlayerController me, BlackBoardInfo info)
    {
        if (info == BlackBoardInfo.NUMERO_APROXIMANDO)
        {
            int n = 0;
            foreach (var item in acao)
            {
                GameObject go = item.Key;

                if (go == me.gameObject)
                    continue;

                if (item.Value == PlayerController.Acao.APROXIMAR)
                    n++;
            }
            return new RequestResponse<int>(n);
        }
        if (info == BlackBoardInfo.NUMERO_DEFENDENDO)
        {
            int n = 0;
            foreach (var item in acao)
            {
                GameObject go = item.Key;

                if (go == me.gameObject)
                    continue;

                if (item.Value == PlayerController.Acao.DEFENDER)
                    n++;
            }
            return new RequestResponse<int>(n);
        }

        throw new System.Exception("Invalid BlackBoardInfo");
    }
    
    public void RequestUpdate(GameObject me, BlackBoardData data, UpdateRequest request)
    {
        if (data == BlackBoardData.POSICAO_BOLA)
            posicaoBola = ((UpdateRequest<Vector3>)request).value;        
        if (data == BlackBoardData.POSICAO_DEFENDER)
            posicaoDefender = ((UpdateRequest<Vector3>)request).value;        
        if (data == BlackBoardData.POSICAO_OBJETIVO)
            posicaoObjetivo = ((UpdateRequest<Vector3>)request).value;
        if (data == BlackBoardData.MINHA_ACAO)
            acao[me] = ((UpdateRequest<PlayerController.Acao>)request).value;        
    }   
}
