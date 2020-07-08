using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
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

                //충돌지점에 총알이펙트 생성한다
                GameObject bulletImpact = Instantiate(bulletImpactFactory);
                //부딪힌 지점 hitInfo안에 정보들이 담겨 있다
                bulletImpact.transform.position = hitInfo.point;
                //파편이 부딪힌 지점이 향하는 방향으로 튀게 해줘야 한다
                bulletImpact.transform.forward = hitInfo.normal;
            }
        }
    }
}
