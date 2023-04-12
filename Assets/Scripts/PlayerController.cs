using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce;
    public float gravityModifier;
    public bool gameOver = false;
    public bool doubleSpeed = false;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;

    private bool isOnGround = true;
    private bool canDoubleJump = false;
    private Animator playerAnim;
    private Rigidbody playerRb;
    private AudioSource playerAudio;
    private GameObject mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        mainCamera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isOnGround)
                {
                    playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                    playerAnim.SetTrigger("Jump_trig");
                    dirtParticle.Stop();
                    playerAudio.PlayOneShot(jumpSound, 1.0f);

                    canDoubleJump = true;
                    isOnGround = false;
                }
                else if (canDoubleJump)
                {
                    playerRb.AddForce(Vector3.up * jumpForce / 2, ForceMode.Impulse);
                    playerAudio.PlayOneShot(jumpSound, 1.0f);

                    canDoubleJump = false;
                }
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                doubleSpeed = true;
                playerAnim.SetFloat("Speed_Multiplier", 2.0f);
            }
            else
            {
                doubleSpeed = false;
                playerAnim.SetFloat("Speed_Multiplier", 1.0f);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            canDoubleJump = false;
            dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            gameOver = true;
            Debug.Log("Game Over!");
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSound, 1.0f);
            mainCamera.GetComponent<AudioSource>().Stop();
        }
    }
}
