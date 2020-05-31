using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    private Rigidbody rb;
    //力
    public int force;
    //分数
    public Text text;
    private int score;
    //摇杆
    public EasyJoystick joystick;
    //coin对象
    public GameObject coinObj;
    //coin总数量
    private int coinAmount = 20;
    //总宽度，x
    private float lengthX = 10f;
    //总长度，z
    private float lengthZ = 10f;
    //y方向施加力的大小
    public float yForce = 1.8f;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        float f = Time.time;
        //初始化coin
        System.Random random = new System.Random(Guid.NewGuid().GetHashCode());
        for (int i = 1; i <= coinAmount; i++)
        {
            float x = (float)random.NextDouble() * lengthX;
            float z = (float)random.NextDouble() * lengthZ;
            //正负符号，用来确定coin位置是正半轴还是负半轴
            //结果有0和1两种，概率各一半
            int sign = random.Next(0, 2);
            //如果随机出的符号是0，乘-1，让coin到负半轴
            if (sign == 0)
            {
                x = x * -1;
            }
            sign = random.Next(0, 2);
            if (sign == 0)
            {
                z = z * -1;
            }
            GameObject coin = Instantiate(coinObj);
            coin.transform.position = new Vector3(x, 0.5f, z);
        }
    }
	
	// Update is called once per frame
	void Update () {
        //电脑键盘wasd
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float y = Input.GetAxis("Jump");
        rb.AddForce(new Vector3(-x, y * yForce, -z) * force);
        //手机摇杆
        float joyX = joystick.JoystickTouch.x;
        float joyZ = joystick.JoystickTouch.y;
        if(joyX * joyX + joyZ * joyZ >= 0.3f * 0.3f) {
            float r = Mathf.Atan2(joyX, joyZ);
            float forceX = force * Mathf.Cos(r);
            float forceZ = force * Mathf.Sin(r);
            rb.AddForce(new Vector3(-forceZ, 0, -forceX));
        }
	}

    private void OnMouseDown()
    {
        Debug.Log("down");
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Coin") {
            Destroy(collider.gameObject);
            score++;
            text.text = "Score: " + score;
        }
    }
}
