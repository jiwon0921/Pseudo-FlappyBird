using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    [Range(0,10)]
    public float gravity = 1f;
    private float normalizedGravity = 0.1f;
    [Range(0,10)]
    public float flappyPower = 1f;
    private float normalizedFlappyPower = 5f;
    public Animator animator;
    public List<AudioSource> birdSounds;

    private void Awake()
    {

    }

    void Start()
    {
        
    }

    void Update()
    {
        AnimationControl();

        if (GameManager.Instance.isReady) return;

        if (transform.position.y > -2.65f)
        {
            Rotate();
            Gravity();
        }

        if (!GameManager.Instance.isGameover)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetMouseButtonDown(0))
            {
                Flappy();
            }

            if(transform.position.y <= -2.65f)
            {
                GameManager.Instance.GameOver();
                birdSounds[2].Play();
            }
        }




        ClampPosition();
    }

    private void Gravity()
    {
        rigidBody.velocity += Vector2.down * gravity * normalizedGravity;
    }

    private void Flappy()
    {
        rigidBody.velocity = Vector2.up * flappyPower * normalizedFlappyPower;

        //sound
        birdSounds[0].Play();

        //Debug.Log("Flappy");
    }



    private void AnimationControl()
    {
        animator.SetFloat("Velocity", rigidBody.velocity.y);
    }

    private void ClampPosition()
    {
        Vector2 curPos = transform.position;
        curPos.y = Mathf.Clamp(curPos.y,-2.65f ,4.75f);
        transform.position = curPos;
    }

    private void Rotate()
    {
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Min(30,Mathf.Max(-90,rigidBody.velocity.y*10)));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance.isGameover) return;

        if (collision.tag == "Score")
        {
            GameManager.Instance.score += 1;
            UIManager.Instance.uiInGame.UpdateScore();
            birdSounds[1].Play();
        }

        if(collision.tag == "Column")
        {
            GameManager.Instance.GameOver();
            birdSounds[3].Play();
        }
    }


}
