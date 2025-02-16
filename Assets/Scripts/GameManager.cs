using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Rigidbody2D LeftBat;

    [SerializeField] private Rigidbody2D RightBat;
    private Vector2 _MoveDirection;
    public float Speed = 2f; // speed 速率 是一个标量，表示速度的大小；velocity 速度 是一个矢量，表示速度的方向和大小
    public float MaxSpeed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 这里容易出现的错误，find无法根据name找到对应的gameObject，出现该错误的主要原因是GameObject对象命名时，多了空格

        LeftBat = GameObject.Find("LeftBat").GetComponent<Rigidbody2D>();
        RightBat = GameObject.Find("RightBat").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputToMove();
    }

    private void FixedUpdate()
    {
        HandleToMove(LeftBat);
        HandleToMove(RightBat);
    }


    private void HandleInputToMove()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            _MoveDirection = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            _MoveDirection = Vector2.down;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.UpArrow) ||
                 Input.GetKeyUp(KeyCode.W))
        {
            _MoveDirection = Vector2.zero;
        }
    }

    private void HandleToMove(Rigidbody2D rigidbody2D)
    {
        // 这里的代码没有达到预期效果的主要原因是，_MoveDirection为0时，rigidbody2D.linearVelocity -=0，所以速度不会减小
        // if (_MoveDirection == Vector2.zero)
        // {
        //     rigidbody2D.linearVelocity -= _MoveDirection * (Speed * (float)Time.fixedTimeAsDouble * 5);
        //
        //     // 限制最小速度
        //     if (rigidbody2D.linearVelocity.magnitude < 0.1f)
        //     {
        //         rigidbody2D.linearVelocity = Vector2.zero;
        //     }
        //     
        //     Debug.Log(rigidbody2D.linearVelocity);
        // }
        if (_MoveDirection == Vector2.zero)
        {
            rigidbody2D.linearVelocity =
                Vector2.Lerp(rigidbody2D.linearVelocity, Vector2.zero, Speed * Time.fixedDeltaTime);

            // 限制最小速度
            if (rigidbody2D.linearVelocity.magnitude < 0.1f)
            {
                rigidbody2D.linearVelocity = Vector2.zero;
            }
        }
        else
        {
            rigidbody2D.linearVelocity += _MoveDirection * (Speed * (float)Time.fixedTimeAsDouble);

            //限制最大速度
            rigidbody2D.linearVelocity = Vector2.ClampMagnitude(rigidbody2D.linearVelocity, MaxSpeed);
        }
    }
}