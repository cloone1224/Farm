using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class Enemy : Interactable {

    private EnemyController controller;

    public override void Interact()
    {
        base.Interact();

        // Attack
    }

    // Use this for initialization
    void Start () {
        controller = GetComponent<EnemyController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TargetPlayer(Transform player)
    {
        controller.FollowTarget(player);
    }
}
