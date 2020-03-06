using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitController : MonoBehaviour
{
    public Animator anim;
    public AnimationClip clip;
    bool canLeave = false;

    private void Update()
    {
        //Go to next floor
        if (canLeave && Input.GetKeyDown(KeyCode.E))
        {
            //Remember that there's a memory leak here since the weapons are persistent between scenes

            //Dont destroy the players current items
            WeaponController weap = PlayerController.player.GetComponentInChildren<WeaponController>();
            if (weap.curWeapon != null)
            {
                DontDestroyOnLoad(weap.curWeapon.gameObject);
            }
            if (PlayerController.player.curEquip != null)
            {
                DontDestroyOnLoad(PlayerController.player.curEquip.gameObject);
            }

            anim.Play("FadeOut");
            Invoke("FadeOut", clip.length);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canLeave = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canLeave = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canLeave = false;
        }
    }

    void FadeOut()
    {
        PlayerController.player.transform.position = Vector3.zero + new Vector3(0, -0.5f, 0f);
        SceneManager.LoadScene(0);
    }
}
