﻿using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerSetup : NetworkBehaviour {

	[SerializeField]
	private Behaviour[] _componentsToDisable;

    private void Start()
	{
		if (!this.isLocalPlayer)
			for (int i = 0; i < this._componentsToDisable.Length; i++)
				this._componentsToDisable[i].enabled = false;
		this.RegisterPlayer();
	}


	private void RegisterPlayer()
	{
		string id = "Player " + this.GetComponent<NetworkIdentity>().netId;
		this.transform.name = id;
	}

}
