using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveYaxis : MonoBehaviour {

    public float length = 5.0f;     //移動する振幅
    public float speed = 2.0f;      //移動スピード　大きくすると早くなります
    private Vector3 startPos;       //ゲーム開始時にいたポジションを入れる変数
    private float radian;

    void Start()
    {
        startPos = transform.position; //ゲーム開始時のポジション
        radian = 0f;
    }

    void FixedUpdate()
    {
        //Mathfは数学関数、時間の経過をフレームごとにSin()の値を出して振幅を掛けたものをy位置に加えます
        radian = Time.time * speed;
        transform.position = new Vector3(startPos.x, startPos.y + length * Mathf.Sin(radian), startPos.z);
        //transform.position = new Vector3(startPos.x,  Mathf.PingPong(Time.time*speed,length),startPos.z);　//PingPongというメソッドも使えます
    }

}
