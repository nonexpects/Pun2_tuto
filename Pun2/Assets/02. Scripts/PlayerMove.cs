using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPun
{
    public float speed = 5.0f; //이동속도
    CharacterController cc; //캐릭터컨트롤러 컴포넌트

    //중력적용
    public float gravity = -20;
    float velocityY;    //낙하속도(벨로시티는 방향과 힘을 들고 있다)
    float jumpPower = 10; //점프파워

    void Start()
    {
        //캐릭터컨트롤러 컴포넌트 가져오기
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {

        //이 사용자가 나라면 움직이고 아니면 움직이지 말아라
        //포톤뷰로 확인이 가능함
        if (!photonView.IsMine) return;


        //플레이어 이동
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, v);
        //카메라가 보는 방향으로 이동해야 한다
        dir = Camera.main.transform.TransformDirection(dir);

        //if (cc.collisionFlags == CollisionFlags.Below)
        if (cc.isGrounded)
        {
            velocityY = 0;
            if (Input.GetButtonDown("Jump"))
            {
                velocityY = jumpPower;
            }
        }
        else
        {
            //땅에 닿지 않은 상태이기때문에 중력적용하기
            velocityY += gravity * Time.deltaTime;
            dir.y = velocityY;
        }


        //중력적용 이동
        cc.Move(dir * speed * Time.deltaTime);
    }
}
