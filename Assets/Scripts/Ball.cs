using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    public List<Blackboard> blackboards;

    void Start ()
    {
        foreach (var blackboard in blackboards)
        {
            blackboard.RequestUpdate(gameObject, Blackboard.BlackBoardData.POSICAO_BOLA, new Blackboard.UpdateRequest<Vector3>(transform.position));
        }
    }
	
	void FixedUpdate ()
    {
        foreach (var blackboard in blackboards)
        {
            blackboard.RequestUpdate(gameObject, Blackboard.BlackBoardData.POSICAO_BOLA, new Blackboard.UpdateRequest<Vector3>(transform.position));
        }
	}
}
