using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallController : MonoBehaviour
{
    public float speed = 10f;

    [SerializeField] private Rigidbody2D _rigidbody2D;

    Vector2 currenPosition;
    Vector2 currentVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        PlayBall(_rigidbody2D);

        currenPosition = transform.position;
    }

    void PlayBall(Rigidbody2D rigidbody2D)
    {
        float MaxAngle;
        float MinAngle;

        //判断小球的随机方向
        Vector2 randomDirection = Random.Range(-1f, 1f) > 0 ? Vector2.right : Vector2.left;

        //控制小球的发射角度
        // 这里规定以向右的x轴为0度，向上的y轴为90度，向左的x轴为180度，向下的y轴为270度
        if (randomDirection == Vector2.right)
        {
            MaxAngle = 30;
            MinAngle = -30;
        }
        else
        {
            MaxAngle = 150;
            MinAngle = 210;
        }

        float angle = Random.Range(MinAngle, MaxAngle);

        //根据角度计算速度分量
        float BallLinearVelocityX = speed * Mathf.Cos(angle * Mathf.Deg2Rad);
        float BallLinearVelocityY = speed * Mathf.Sin(angle * Mathf.Deg2Rad);

        //设置小球的速度
        _rigidbody2D.linearVelocity = new Vector2(BallLinearVelocityX, BallLinearVelocityY);
        currentVelocity = _rigidbody2D.linearVelocity;
    }


    // 无论是Gizmos还是Debug.DrawLine，都必须点击GameView的Gizmos按钮才能显示，且只能在Scene视图中显示的，不会在Game视图中显示
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(currenPosition,
            currentVelocity.normalized * 10000f);
    }


    void OnCollisionEnter2D(Collision2D collision)
    { 
        if (collision.gameObject.CompareTag("Bat"))
        {
            // 获取碰撞点
            Vector2 collisionPoint = collision.GetContact(0).point;

            //获取板块的中心和宽度
            float batCenter = collision.gameObject.transform.position.x;
            float batWidth = collision.collider.transform.localScale.x;

            Debug.Log($"collisionPoint: {batCenter}");
            Debug.Log($"batWidth : {batWidth}");

            //计算偏移量（归一化）
            float relativePosition = (collisionPoint.x - batCenter) / (batWidth / 2);
            Debug.Log($"relativePosition: {relativePosition}");
            
            //计算反射角
            float maxBounceAngle = 75f * Mathf.Deg2Rad; // 最大反射角度
            float angle = relativePosition * maxBounceAngle;
            Vector2 newDirection = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)).normalized;
            
            //设置小球的速度
            _rigidbody2D.linearVelocity = newDirection * speed;
        }
    }
}