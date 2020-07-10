using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerFire : MonoBehaviourPun
{
    int _hp = 1;
    public int hp
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
            if(_hp <= 0)
            {
                _hp = 0;
                print("죽었다");
                //내가 죽었을때 방장 소유권을 다른 클라이언트에 넘긴다
                if(photonView.IsMine)
                {
                    //1. 죽어야 하는 녀석이 마스터 클라이언트냐? (방에대한 소유권을 가지고 있다)
                    //따라서 방을 나갈때 반드시 마스터 클라이언트 권한을 이양해야 한다
                    //2. 로비씬 전환
                    //3. 방을 나가는 처리를 포톤네트웍을 통해 해준다

                    if(PhotonNetwork.IsMasterClient)
                    {
                        PhotonNetwork.SetMasterClient(PhotonNetwork.MasterClient.GetNext());
                    }
                    PhotonNetwork.LoadLevel("LobbyScene");
                    PhotonNetwork.LeaveRoom();
                }


            }
        }
    }


    //void setHp(float hp) { _hp = hp; }


    public GameObject bulletImpactFactory;  //총알파편 프리팹

    void Update()
    {
        //마우스왼쪽버튼 클릭시 레이캐스트로 총알발사
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo;
            
            //레이랑 충돌했냐?
            if (Physics.Raycast(ray, out hitInfo))
            {
                print("충돌오브젝트 : " + hitInfo.collider.name);

                //충돌이펙트 보여주기(모든 사용자에게 보여야 한다)
                photonView.RPC("ShowEffect", RpcTarget.All, hitInfo.point, hitInfo.normal);

                //에너미가 총에 맞았을때
                PhotonView enemy = hitInfo.transform.GetComponent<PhotonView>();
                if(enemy)
                {
                    enemy.RPC("Damage", RpcTarget.All, 1);
                }
            }
        }
    }

    [PunRPC]
    void ShowEffect(Vector3 pos, Vector3 normal)
    {
        //충돌지점에 총알이펙트 생성한다
        GameObject bulletImpact = Instantiate(bulletImpactFactory);
        //부딪힌 지점 hitInfo안에 정보들이 담겨 있다
        bulletImpact.transform.position = pos;
        //파편이 부딪힌 지점이 향하는 방향으로 튀게 해줘야 한다
        bulletImpact.transform.forward = normal;
    }

    [PunRPC]
    void Damage(int value)
    {
        hp -= value;
    }
}
