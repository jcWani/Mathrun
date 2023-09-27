using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public static bool isMagnet = false;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Magnet")
        {
            StartCoroutine(ActivateCoin());
        }

        if (other.gameObject.tag == "Buff")
        {
            StartCoroutine(scoreDouble());
        }

        if(other.gameObject.tag == "Spring")
        {
            StartCoroutine(jumpHigh());
        }
    }

    IEnumerator ActivateCoin()
    {
        GameManager.Instance.springSound.Stop();
        GameManager.Instance.buffSound.Stop();
        GameManager.Instance.magnetSound.Play();
        GameManager.Instance.springParticle.SetActive(false);
        GameManager.Instance.buffParticle.SetActive(false);
        GameManager.Instance.magnetParticle.SetActive(true);
        GameManager.Instance.deBuff();
        isMagnet = true;
        PlayerMotor.jumpForce = 5.0f;
        yield return new WaitForSeconds(10f);
        Debug.Log("END");
        GameManager.Instance.magnetParticle.SetActive(false);
        isMagnet = false;
        GameManager.Instance.magnetSound.Stop();
    }

    IEnumerator scoreDouble()
    {
        GameManager.Instance.springSound.Stop();
        GameManager.Instance.magnetSound.Stop();
        GameManager.Instance.buffSound.Play();
        GameManager.Instance.magnetParticle.SetActive(false);
        GameManager.Instance.springParticle.SetActive(false);
        GameManager.Instance.buffParticle.SetActive(true);
        GameManager.Instance.Buff();
        isMagnet = false;
        PlayerMotor.jumpForce = 5.0f;
        yield return new WaitForSeconds(10f);
        GameManager.Instance.buffParticle.SetActive(false);
        GameManager.Instance.buffSound.Stop();
        GameManager.Instance.deBuff();
        Debug.Log("END");
    }

    IEnumerator jumpHigh()
    {
        PlayerMotor.jumpForce = 8f;
        GameManager.Instance.magnetSound.Stop();
        GameManager.Instance.buffSound.Stop();
        GameManager.Instance.springSound.Play();
        GameManager.Instance.buffParticle.SetActive(false);
        GameManager.Instance.magnetParticle.SetActive(false);
        GameManager.Instance.springParticle.SetActive(true);
        GameManager.Instance.deBuff();
        isMagnet = false;
        yield return new WaitForSeconds(10f);
        GameManager.Instance.springParticle.SetActive(false);
        GameManager.Instance.springSound.Stop();
        PlayerMotor.jumpForce = 5.0f;
        Debug.Log("END");
    }
}
