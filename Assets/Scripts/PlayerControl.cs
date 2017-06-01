using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour {
    public float mMoveSpeed = 5.0f;
    public Transform mShellSpawn;
    public GameObject mShellPrefab;
    public float mFireInterval = 0.2f;
    public Transform mCharacter;

    private Rigidbody2D mRigidbody;
    private float mLastFireTime;
    private bool mFaceLeft;

	// Use this for initialization
	void Start () {
        mRigidbody = GetComponent<Rigidbody2D>();
        mLastFireTime = 0;
        mFaceLeft = false;

        // set correct layer
        if (isLocalPlayer) {
            gameObject.layer = LayerMask.NameToLayer("Myself");
        } else {
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Move();
        Fire();
    }

    private void Move() {
        if (!isLocalPlayer) {
            return;
        }

        bool right = Input.GetKey(KeyCode.D);
        bool left = Input.GetKey(KeyCode.A);

        Vector2 newVelocity;
        Quaternion newRotation = mCharacter.rotation;
        if (left && !right) {
            newVelocity = new Vector2(-mMoveSpeed, mRigidbody.velocity.y);
            mFaceLeft = true;
            newRotation = Quaternion.Euler(0f, 180f, 0f);
        } else if (!left && right) {
            newVelocity = new Vector2(mMoveSpeed, mRigidbody.velocity.y);
            mFaceLeft = false;
            newRotation = Quaternion.Euler(0f, 0f, 0f);
        } else {
            newVelocity = new Vector2(0f, mRigidbody.velocity.y);
        }

        mCharacter.rotation = newRotation;
        mRigidbody.velocity = newVelocity;
    }

    [Command]
    private void CmdFire(Vector2 fireDir) {
        GameObject shell = Instantiate(mShellPrefab, mShellSpawn.position, mShellSpawn.rotation) as GameObject;
        ShellControl sc = shell.GetComponent<ShellControl>();
        if (sc) {
            sc.mFlyDirection = fireDir.normalized;
            sc.Init();
        }
        NetworkServer.Spawn(shell);
    }

    private void Fire() {
        if (!isLocalPlayer) {
            return;
        }
        if (Input.GetMouseButton(0) && Time.time - mFireInterval > mLastFireTime) {
            Vector3 mouseInViewport = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Vector3 mouseInWorld = Camera.main.ViewportToWorldPoint(mouseInViewport);
            Vector3 fireDir = mouseInWorld - transform.position;

            CmdFire(fireDir);
            mLastFireTime = Time.time;
        }
    }

    private bool IsFaceLeft() {
        return mFaceLeft;
    }
}
