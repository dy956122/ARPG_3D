using UnityEngine;
using System.Collections;
using System;

namespace UnityChan
{
    [RequireComponent(typeof(Animator), typeof(CapsuleCollider), typeof(Rigidbody))]
    public class UnityChanControlScriptWithRgidBody : MonoBehaviour
    {
        public bool useCurves = true;
        public float useCurvesHeight = 0.5f;
        public float animSpeed = 1.5f;
        public float lookSmoother = 3.0f;
        public float forwardSpeed = 7.0f;
        public float backwardSpeed = 2.0f;
        public float rotateSpeed = 2.0f;
        public float jumpPower = 3.0f;
        // public event Action NormalType, Rest;
        float orgColHight;
        CapsuleCollider col;
        Rigidbody rb;
        Vector3 velocity, orgVectColCenter;
        Animator anim;
        AnimatorStateInfo currentBaseState;
        GameObject cameraObject;

        static int idleState = Animator.StringToHash("Base Layer.Idle");
        static int locoState = Animator.StringToHash("Base Layer.Locomotion");
        static int jumpState = Animator.StringToHash("Base Layer.Jump");
        static int restState = Animator.StringToHash("Base Layer.Rest");

        void Move()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            anim.SetFloat("Speed", v);
            anim.SetFloat("Direction", h);
            anim.speed = animSpeed;
            currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
            rb.useGravity = true;

            velocity = new Vector3(0, 0, v);
            velocity = transform.TransformDirection(velocity);
            if (v > 0.1) velocity *= forwardSpeed;
            else if (v < -0.1) velocity *= backwardSpeed;

            transform.localPosition += velocity * Time.fixedDeltaTime;
            transform.Rotate(0, h * rotateSpeed, 0);
        }

        void Start()
        {
            anim = GetComponent<Animator>();
            col = GetComponent<CapsuleCollider>();
            rb = GetComponent<Rigidbody>();
            cameraObject = GameObject.FindWithTag("MainCamera");
            orgColHight = col.height;
            orgVectColCenter = col.center;
        }

        void FixedUpdate()
        {
            Move();

            if (Input.GetButtonDown("Jump"))
            {
                if (currentBaseState.fullPathHash == locoState && !anim.IsInTransition(0))
                {
                    rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
                    anim.SetBool("Jump", true);
                }
            }

            if (currentBaseState.fullPathHash == locoState)
            {
                if (useCurves) resetCollider();
            }
            else if (currentBaseState.fullPathHash == jumpState)
            {
                // cameraObject.SendMessage("setCameraPositionJumpView");
				print(0);
                // Rest();
				print(1);
                if (!anim.IsInTransition(0))
                {

                    if (useCurves)
                    {
                        // 以下JUMP00アニメーションについているカーブJumpHeightとGravityControl
                        // JumpHeight:JUMP00でのジャンプの高さ（0〜1）
                        // GravityControl:1⇒ジャンプ中（重力無効）、0⇒重力有効
                        float jumpHeight = anim.GetFloat("JumpHeight");
                        float gravityControl = anim.GetFloat("GravityControl");
                        if (gravityControl > 0) rb.useGravity = false;

                        Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
                        RaycastHit hitInfo = new RaycastHit();
                        // 高さが useCurvesHeight 以上ある時のみ、コライダーの高さと中心をJUMP00アニメーションについているカーブで調整する
                        if (Physics.Raycast(ray, out hitInfo))
                        {
                            if (hitInfo.distance > useCurvesHeight)
                            {
                                col.height = orgColHight - jumpHeight;
                                float adjCenterY = orgVectColCenter.y + jumpHeight;
                                col.center = new Vector3(0, adjCenterY, 0);
                            }
                            else
                            {
                                resetCollider();
                            }
                        }
                    }
                    anim.SetBool("Jump", false);
                }
            }

            else if (currentBaseState.fullPathHash == idleState)
            {
                if (useCurves) resetCollider();

                if (Input.GetButtonDown("Jump")) anim.SetBool("Rest", true);
            }
            else if (currentBaseState.fullPathHash == restState)
            {
                // cameraObject.SendMessage("setCameraPositionFrontView");		// カメラを正面に切り替える
                if (!anim.IsInTransition(0)) anim.SetBool("Rest", false);
            }
        }

        void OnGUI()
        {
            GUI.Box(new Rect(Screen.width - 260, 10, 250, 150), "Interaction");
            GUI.Label(new Rect(Screen.width - 245, 30, 250, 30), "Up/Down Arrow : Go Forwald/Go Back");
            GUI.Label(new Rect(Screen.width - 245, 50, 250, 30), "Left/Right Arrow : Turn Left/Turn Right");
            GUI.Label(new Rect(Screen.width - 245, 70, 250, 30), "Hit Space key while Running : Jump");
            GUI.Label(new Rect(Screen.width - 245, 90, 250, 30), "Hit Spase key while Stopping : Rest");
            GUI.Label(new Rect(Screen.width - 245, 110, 250, 30), "Left Control : Front Camera");
            GUI.Label(new Rect(Screen.width - 245, 130, 250, 30), "Alt : LookAt Camera");
        }

        void resetCollider()
        {
            col.height = orgColHight;
            col.center = orgVectColCenter;
        }
    }
}