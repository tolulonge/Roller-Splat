using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{

    public Rigidbody rb;
    public float speed = 15;

    private bool isTravelling;
    private Vector3 travelDirection;
    private Vector3 nextCollisionPosition;

    public int minSwipeRecognition = 500;
    private Vector2 swipePositionLastFrame;
    private Vector2 swipePositionCurrentFrame;
    private Vector2 currentSwipe;

    private Color solveColor;
    public ParticleSystem dustParticle;
    public AudioClip boomSound;
    public AudioClip swooshSound;
    private AudioSource ballAudio;

    private void Start()
    {
        solveColor = Random.ColorHSV(0.5f, 1);
        GetComponent<MeshRenderer>().material.color = solveColor;
        ballAudio = GetComponent<AudioSource>();
    }

    // Setup Swipe Functionality
    private void FixedUpdate()
    {
        dustParticle.transform.position = new Vector3(transform.position.x, -0.85f, transform.position.z);
        if (isTravelling)
        {
            rb.velocity = speed * travelDirection;
            dustParticle.Play();
            
        }
      

        Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up / 2), 0.05f);

        int i = 0;
        while(i < hitColliders.Length)
        {
            GroundPiece ground = hitColliders[i].transform.GetComponent<GroundPiece>();
            if(ground && !ground.isColored)
            {
                ground.ChangeColor(solveColor);
            }
            i++;
        }

        if(nextCollisionPosition != Vector3.zero)
        {
            if(Vector3.Distance(transform.position, nextCollisionPosition) < 1)
            {
                isTravelling = false;
                travelDirection = Vector3.zero;
                nextCollisionPosition = Vector3.zero;
                
            }
        }

        if (isTravelling)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
           
            swipePositionCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            if(swipePositionLastFrame != Vector2.zero)
            {
                currentSwipe = swipePositionCurrentFrame - swipePositionLastFrame;
                if(currentSwipe.sqrMagnitude < minSwipeRecognition)
                {
                    return;
                }
                currentSwipe.Normalize();
                ballAudio.PlayOneShot(swooshSound, 0.3f);


                // UP/Down
                if (currentSwipe.x > -0.5f && currentSwipe.x < 0.5)
                {
                    // Go up/down
                    SetDestination(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);
                    

                }

                if (currentSwipe.y > -0.5f && currentSwipe.y < 0.5)
                {
                    // Go left/right
                    SetDestination(currentSwipe.x > 0 ? Vector3.right : Vector3.left);
                    
                }
            }

            swipePositionLastFrame = swipePositionCurrentFrame;
        }

        if (Input.GetMouseButtonUp(0))
        {
            swipePositionLastFrame = Vector2.zero;
            currentSwipe = Vector2.zero;
        }
       
    }

    private void SetDestination(Vector3 direction)
    {
        travelDirection = direction;

        RaycastHit hit;

        if(Physics.Raycast(transform.position, direction, out hit, 100f))
        {
            nextCollisionPosition = hit.point;
        }

        isTravelling = true;
    }

}
