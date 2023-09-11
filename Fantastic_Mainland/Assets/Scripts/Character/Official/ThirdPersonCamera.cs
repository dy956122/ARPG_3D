//
// Unityちゃん用の三人称カメラ
// 
// 2013/06/07 N.Kobyasahi
//
using UnityEngine;
using System.Collections;

namespace UnityChan
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        public float smooth = 3f;       // カメラモーションのスムーズ化用変数
        [SerializeField] UnityChanControlScriptWithRgidBody player;
        Transform standardPos, frontPos, jumpPos;

        bool bQuickSwitch = false;  //Change Camera Position Quickly

        void Start()
        {
            // 各参照の初期化
            standardPos = GameObject.Find("CamPos").transform;
            if (GameObject.Find("FrontPos")) frontPos = GameObject.Find("FrontPos").transform;
            if (GameObject.Find("JumpPos")) jumpPos = GameObject.Find("JumpPos").transform;

            transform.position = standardPos.position;
            transform.forward = standardPos.forward;
        }

        void setCameraPositionNormalView()
        {
            if (bQuickSwitch == false)
            {
                // the camera to standard position and direction
                transform.position = Vector3.Lerp(transform.position, standardPos.position, Time.fixedDeltaTime * smooth);
                transform.forward = Vector3.Lerp(transform.forward, standardPos.forward, Time.fixedDeltaTime * smooth);
            }
            else
            {
                // the camera to standard position and direction / Quick Change
                transform.position = standardPos.position;
                transform.forward = standardPos.forward;
                bQuickSwitch = false;
            }
        }

        void setCameraPositionFrontView()
        {
            // Change Front Camera
            bQuickSwitch = true;
            transform.position = frontPos.position;
            transform.forward = frontPos.forward;
        }

        void setCameraPositionJumpView()
        {
            // Change Jump Camera
            bQuickSwitch = false;
            transform.position = Vector3.Lerp(transform.position, jumpPos.position, Time.fixedDeltaTime * smooth);
            transform.forward = Vector3.Lerp(transform.forward, jumpPos.forward, Time.fixedDeltaTime * smooth);
        }

        void FixedUpdate()  // このカメラ切り替えはFixedUpdate()内でないと正常に動かない
        {
            if (Input.GetButton("Fire1")) setCameraPositionFrontView();
            else if (Input.GetButton("Fire2")) setCameraPositionJumpView();
            else setCameraPositionNormalView();
        }
    }
}