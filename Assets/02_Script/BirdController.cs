using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    [Range(0,10)]
    public float gravity = 1f;
    private float normalizecGravity = 0.1f;
    [Range(0,10)]
    public float flappyPower = 1f;
    private float normalizecFlappyPower = 5f;
    public Animator animator;
    public List<AudioSource> birdSounds;

    private void Awake()
    {
        //rigidBody.gravityScale = gravity;
    }

    void Start()
    {
        
    }

    void Update()
    {
        AnimationControl();
        Gravity();
        if(Input.GetKeyDown(KeyCode.A) || Input.GetMouseButtonDown(0))
        {
            Flappy();
        }
    }

    private void Gravity()
    {
        rigidBody.velocity += Vector2.down * gravity * normalizecGravity;
    }

    private void Flappy()
    {
        rigidBody.velocity = Vector2.up * flappyPower * normalizecFlappyPower;
        //Debug.Log("Flappy");
    }

    private void AnimationControl()
    {
        animator.SetFloat("Velocity", rigidBody.velocity.y);
    }
}
