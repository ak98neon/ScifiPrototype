using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private Animator animator;
    private bool doorIsOpen = false;
    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    public void OpenCloseDoor()
    {
        if (doorIsOpen)
        {
            audio.Play();
            animator.SetBool("openDoor", false);
        }
        else
        {
            audio.Play();
            animator.SetBool("openDoor", true);
        }
        doorIsOpen = !doorIsOpen;
    }
}
