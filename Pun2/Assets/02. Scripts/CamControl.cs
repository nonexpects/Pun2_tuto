using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    //카메라를 마우스움직이는 방향으로 회전하기
    public float speed = 150;
    //회전각도를 직접 제어하자
    float angleX, angleY;

    void Update()
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");
        angleX += h * speed * Time.deltaTime;
        angleY += v * speed * Time.deltaTime;
        angleY = Mathf.Clamp(angleY, -30, 30);
        transform.eulerAngles = new Vector3(-angleY, angleX, 0);
    }
}
