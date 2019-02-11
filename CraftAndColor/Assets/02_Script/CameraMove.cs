using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    public Vector3 player;
    public Vector3 firstPosition;
    private Vector3 offset;
    private Vector3 campos;
    public float speed = 1f;
    public float zoomSpeed = 1f;


    public Camera DesiredView { get; set; }

    private Camera playerFollowCamera;
    private float playerFollowOrthographicSize;

    void Start()
    {
        player = GameObject.Find("Player").transform.position;
        firstPosition = player;
        // プレイヤーとカメラのポジションどうしをひいて、位置の差を変数offsetに格納します
        // Pull the position between the player and camera and store the position difference in the variable offset
        offset = this.transform.position - player;
        //はじめのカメラポジションの高さを所得しておく
        // I will earn the height of the first camera position
        campos.y = GetComponent<Transform>().position.y;

        playerFollowCamera = this.GetComponent<Camera>();
        playerFollowOrthographicSize = playerFollowCamera.orthographicSize;
        DesiredView = playerFollowCamera;
    }
    public bool FollowPlayer
    {
        get
        {
            return playerFollowCamera == DesiredView;
        }
    }

    public bool FollowCustomPosition
    {
        get
        {
            return playerFollowCamera != DesiredView;
        }
    }

    void LateUpdate()
    {
        //　この（カメラの）ポジションは　プレイヤーの位置にoffsetに入れられた値を足した場所です
        // This position (camera) is the place where the player's position plus the value put in offset
        player = GameObject.Find("Player").transform.position;

        Vector3 fixYpos = new Vector3(player.x, campos.y - offset.y, player.z);

        Vector3 desiredViewOnPlayerPosition = player + offset;

        float step = speed * Time.deltaTime; // calculate distance to move
        float zoomStep = zoomSpeed * Time.deltaTime; // calculate distance to zoom


        if (FollowPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, desiredViewOnPlayerPosition, step);
            //playerFollowCamera.orthographicSize = playerFollowOrthographicSize;
            playerFollowCamera.orthographicSize = Mathf.Lerp(playerFollowCamera.orthographicSize, playerFollowOrthographicSize, zoomSpeed);
        }
        else if (FollowCustomPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, DesiredView.transform.position, step);
            // playerFollowCamera.orthographicSize = DesiredView.orthographicSize;

            playerFollowCamera.orthographicSize = Mathf.Lerp(playerFollowCamera.orthographicSize, DesiredView.orthographicSize, zoomSpeed);

        }



        //if(player.x < firstPosition.x)
        //{

        //}
    }
}
