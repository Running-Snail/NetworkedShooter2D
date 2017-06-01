using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour {
    [SyncVar(hook = "OnHealthChange")]
    public float mCurrentHealth = 100f;
    public Slider mSlider;

	// Use this for initialization
	void Start () {
        mCurrentHealth = 100f;
        SetHealthUI(mCurrentHealth);
    }

    private void SetHealthUI(float health) {
        mSlider.value = health;
    }

    void OnHealthChange(float health) {
        SetHealthUI(health);
    }

    public void TakeDamage(float amount) {
        if (!isServer) {
            return;
        }

        mCurrentHealth -= amount;

        if (mCurrentHealth <= 0) {
            mCurrentHealth = 100f;
            RpcRespawn();
        }
    }

    [ClientRpc]
    void RpcRespawn() {
        if (isLocalPlayer) {
            transform.position = Vector3.zero;
        }
    }
}
