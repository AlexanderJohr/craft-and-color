using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour {
   
    public bool jump = true;
    Rigidbody2D rb;
 

    public float jumpForce = 22f;       // ジャンプ時に加える力  Force applied at jump
    public float jumpThreshold = 1f;    // ジャンプ中か判定するための閾値 Threshold for judging whether jumping is in progress
    public float runForce = 1.5f;       // 走り始めに加える力 Power to start running
    public float runSpeed = 0.5f;       // 走っている間の速度 Speed ​​while running
    public float runThreshold = 2.2f;   // 速度切り替え判定のための閾値 Threshold for speed switching determination
    public float CCCount = 1;    //色切り替えに使用
    public bool isGround = true;        // 地面と接地しているか管理するフラグ Flag to manage whether it is grounded and ground
    public int key = 0;                 // Left / right input control
    public int keyStatus;
    public bool canHave = false;
    public bool having = false;
    public bool Toseparate = false;
    public Color NowColor;
    public Collider2D blockCol;
    public GameObject ChaildBlock;
    public Sprite standing, running, jumping, standb,runb,jumpb,separate;
    public Sprite redb, greenb, blueb, purpleb, orangeb, yellowb;
    public AudioClip[] audioClips;
    private AudioSource audioSource;

    string state;                // プレイヤーの状態管理 Player state management
    string prevState;            // 前の状態を保存 Save previous state
    float stateEffect = 1;       // 状態に応じて横移動速度を変えるための係数 A coefficient for changing the lateral movement speed according to the state



    Vector2 min;
    Vector2 max;

    void Start()
    {
        this.rb = this.gameObject.GetComponent<Rigidbody2D>();
        audioSource = gameObject.GetComponent<AudioSource>();
        //this.animator = GetComponent<Animator>();
        
    }

    void Update()
    {
        min = Camera.main.ViewportToWorldPoint(Vector2.zero);//最小 minimum
        max = Camera.main.ViewportToWorldPoint(Vector2.one);//最大 maximum

        GetInputKey();          // ① 入力を取得
        ChangeState();          // ② 状態を変更する
        ChangeAnimation();      // ③ 状態に応じてアニメーションを変更する
        Move();                 // ④ 入力に応じて移動する

        Havingblock();  //ブロックを持つためのメソッド
        ColorChange();  //ブロックの色を変えるためのメソッド
    }

    void GetInputKey()
    {
        if (having) this.gameObject.GetComponent<SpriteRenderer>().sprite = runb;
        else this.gameObject.GetComponent<SpriteRenderer>().sprite = running;
        key = 0;

        if (Input.GetAxisRaw("Horizontal") < 0) {
            key = -1;
            keyStatus = key;
        };
       
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            key = 1;
            keyStatus = key;
        };
        
    }

    void ChangeState()
    {
        // 空中にいるかどうかの判定。上下の速度(rigidbody.velocity)が一定の値を超えている場合、空中とみなす
        // Determining whether it is in the air. If the upper and lower speed (rigidbody.velocity) exceeds a certain value, it is regarded as being in the air
        
        if (Mathf.Abs(rb.velocity.y) > jumpThreshold)
        {
            isGround = false;
        }

        // 接地している場合 When it is grounded
        if (isGround)
        {
            // 走行中 Running
            if (key == 0)
            {
                state = "IDLE"; //待機状態 Standby state
                if(having) this.gameObject.GetComponent<SpriteRenderer>().sprite = standb;
                else this.gameObject.GetComponent<SpriteRenderer>().sprite = standing;
            }
            else
            {
                state = "RUN";
            }
            // 空中にいる場合 When in the air
        }
        else
        {
            // 上昇中 Rising
            if (rb.velocity.y > 0)
            {
                state = "JUMP";
                if (having) this.gameObject.GetComponent<SpriteRenderer>().sprite = jumpb;
                else if (Toseparate)this.gameObject.GetComponent<SpriteRenderer>().sprite = separate;
                else this.gameObject.GetComponent<SpriteRenderer>().sprite = jumping;
                // 下降中 Falling
            }
            else if (rb.velocity.y < 0)
            {
                state = "FALL";
                if (having) this.gameObject.GetComponent<SpriteRenderer>().sprite = jumpb;
                else if (Toseparate) this.gameObject.GetComponent<SpriteRenderer>().sprite = separate;
                else this.gameObject.GetComponent<SpriteRenderer>().sprite = jumping;
            }
        }
    }

    void ChangeAnimation()
    {
        // 状態が変わった場合のみアニメーションを変更する Change animation only when the state changes
        if (prevState != state)
        {
            switch (state)
            {
                case "JUMP":
                    
                    stateEffect = 0.5f;
                    break;
                case "FALL":
                    
                    stateEffect = 0.5f;
                    break;
                case "RUN":
                   
                    stateEffect = 1f;
                    //GetComponent<SpriteRenderer> ().flipX = true;
                    transform.localScale = new Vector3(2.4f * key , transform.localScale.y, 1); // 向きに応じてキャラクターのspriteを反転 Flip character sprite according to orientation
                    break;
                default:
                   
                    stateEffect = 1f;
                    break;
            }
            // 状態の変更を判定するために状態を保存しておく Save the state to judge the state change
            prevState = state;
        }
    }

    void Move()
    {
        // 設置している時にSpaceキー押下でジャンプ Jump by pressing Space key when installing
        if (isGround)
        {
            if (Toseparate) Toseparate = false;
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetButtonDown("X")) 
            {
                audioSource.clip = audioClips[0];
                audioSource.Play();
                if (having) this.gameObject.GetComponent<SpriteRenderer>().sprite = jumpb;
                else if (Toseparate)this.gameObject.GetComponent<SpriteRenderer>().sprite = separate;
                else this.gameObject.GetComponent<SpriteRenderer>().sprite = jumping;
                this.rb.AddForce(transform.up * this.jumpForce);
				//se01.PlayOneShot (se01.clip);
                isGround = false;
            }
        }

        // 左右の移動。一定の速度に達するまではAddforceで力を加え、それ以降はtransform.positionを直接書き換えて同一速度で移動する
        // Move left and right. Addforce applies force until it reaches a certain speed and after that it transfers transform.position directly and moves at the same speed
        float speedX = Mathf.Abs(this.rb.velocity.x);

        if (speedX < this.runThreshold)
        {
             this.rb.AddForce(transform.right * key * this.runForce * stateEffect); //未入力の場合は key の値が0になるため移動しない
             //transform.localScale = new Vector3(key, 1, 1); // 向きに応じてキャラクターのspriteを反転
        }
        else
        {      
            this.transform.position += new Vector3(runSpeed * Time.deltaTime * key * stateEffect, 0, 0);
            //transform.localScale = new Vector3(key, 1, 1); // 向きに応じてキャラクターのspriteを反転
        }

    }

    void Havingblock()
    {

        if (Input.GetButtonDown("Square"))
        {
            if (canHave == true && having == false)
            {
                Debug.Log("□ボタン押したで");
                //blockCol.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                blockCol.gameObject.GetComponent<Transform>().transform.position = new Vector3(this.transform.position.x, this.transform.position.y-0.8f, 0);//this.transform.position;
                blockCol.gameObject.GetComponent<Rigidbody2D>().simulated = false;
                blockCol.gameObject.GetComponentInParent<block>().liftBlock(this.transform);
                blockCol.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                NowColor = blockCol.gameObject.GetComponent<SpriteRenderer>().color;
                having = true;

                //持っているブロックによって見た目を変える

                if (blockCol.tag == "red")
                {
                    standb = redb;
                    CCCount = 2;
                }
                else if (blockCol.tag == "orange")
                {
                    standb = orangeb;
                    CCCount = 1;
                }
                else if (blockCol.tag == "blue")
                {
                    standb = blueb;
                    CCCount = 4;
                }
                else if (blockCol.tag == "green")
                {
                    standb = greenb;
                    CCCount = 3;
                }
                else if (blockCol.tag == "purple")
                {
                    standb = purpleb;
                    CCCount = 5;
                }
                else if (blockCol.tag == "yellow")
                {
                    standb = yellowb;
                    CCCount = 6;
                }
            }

            else if(having && isGround)
            {
                blockCol.gameObject.GetComponentInParent<block>().downBlock(this.transform);
                //blockCol.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
                blockCol.gameObject.GetComponent<Transform>().transform.position = new Vector3(this.transform.position.x, this.transform.position.y -2.0f, 0);//this.transform.position;
                blockCol.gameObject.GetComponent<Rigidbody2D>().simulated = true;
                blockCol.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                blockCol.gameObject.GetComponent<SpriteRenderer>().color = NowColor;

                //blockCol.GetComponent<Transform>().transform.parent = null;
                having = false;

                //Jump
                audioSource.clip = audioClips[0];
                audioSource.Play();
                this.gameObject.GetComponent<SpriteRenderer>().sprite = separate;
                this.rb.AddForce(transform.up * this.jumpForce);
                //se01.PlayOneShot (se01.clip);
                isGround = false;
                Toseparate = true;
            }
        }
        
    }

    //ブロックを持った状態だと、色を変えられる
    void ColorChange()
    {
        if (having && Input.GetButtonDown("Triangle"))
        {
            audioSource.clip = audioClips[1];
            audioSource.Play();

            if (CCCount == 1)
            {
                NowColor = new Color(1, 0.6352941f, 0.6352941f, 1);
                blockCol.gameObject.tag = "red";
                standb = redb;
                CCCount += 1;
            }

            else　if (CCCount == 2)
            {
                NowColor = new Color(0.7372549f, 0.937255f, 0.7019608f, 1);
                blockCol.gameObject.tag = "green";
                standb = greenb;
                CCCount += 1;
            }

            else if (CCCount == 3)
            {
                NowColor = new Color(0.7411765f, 0.8666667f, 1, 1);
                blockCol.gameObject.tag = "blue";
                standb = blueb;
                CCCount += 1;
            }

            else if (CCCount == 4)
            {
                NowColor = new Color(0.9058824f, 0.7019608f, 0.937255f, 1);
                blockCol.gameObject.tag = "purple";
                standb = purpleb;
                CCCount += 1;
            }

            else if (CCCount == 5)
            {
                NowColor = new Color(1, 1, 0.7411765f, 1);
                blockCol.gameObject.tag = "yellow";
                standb = yellowb;
                CCCount += 1;
            }
            else if (CCCount == 6)
            {
                NowColor = new Color(1, 0.7294118f, 0.4235294f, 1);
                blockCol.gameObject.tag = "orange";
                standb = orangeb;
                CCCount = 1;
            }

        }
    }

    // 着地判定 Landing determination
    void OnCollisionEnter2D(Collision2D col)
    {
        if (!isGround)isGround = true;

        if (col.collider.tag == "red" || col.collider.tag == "blue" || col.collider.tag == "green" || col.collider.tag == "yellow" 
            || col.collider.tag == "purple" || col.collider.tag == "orange")

        {
            if (having == false){
                canHave = true;
                blockCol = col.collider;

            }
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.collider.tag == "red" || col.collider.tag == "blue" || col.collider.tag == "green" || col.collider.tag == "yellow" 
            || col.collider.tag == "purple" || col.collider.tag == "orange")

        {
            if (having == false) canHave = true;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        canHave = false;
    }

    
}
