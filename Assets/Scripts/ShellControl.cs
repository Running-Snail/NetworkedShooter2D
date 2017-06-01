using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody2D))]
public class ShellControl : NetworkBehaviour {
    public float mSpeed = 5.0f;
    public float mShellLiveTime = 5.0f;
    public float mDamage = 20f;
    public LayerMask mDamageLayer;
    public Vector2 mFlyDirection;
    public Transform mDamageBox;

    private Rigidbody2D mRigidbody;

	// Use this for initialization
	void Start () {
        // mRigidbody = GetComponent<Rigidbody2D>();
        // Destroy(gameObject, mShellLiveTime);
	}

    void OnTriggerEnter2D(Collider2D collision) {
        PlayerHealth ph = collision.GetComponent<PlayerHealth>();
        if (ph) {
            ph.TakeDamage(mDamage);
        }
        Destroy(gameObject);
    }

    public void Init() {
        mRigidbody = GetComponent<Rigidbody2D>();
        if (mRigidbody) {
            mRigidbody.velocity = mFlyDirection.normalized * mSpeed;
        }
        Destroy(gameObject, mShellLiveTime);
    }
}
