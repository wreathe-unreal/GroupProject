using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WeaponBehavior : MonoBehaviour
{
    public String WeaponName = "Weapon";
    public float BulletSpread = 2f;
    public float ProjSpeed = 2000f;
    public float MaxChargeTime = 2.0f;
    public bool bChargeUpWeapon = false;
    public bool bFullyAutomatic = true;
    public float FireRate = .1f;
    public GameObject Bullet;
    public int ProjCount = 1;
    public bool bExplodes = false;
    public AudioClip WeaponAudio;

    private float FullAutoDelay = 1.0f;
    private float ChargeTime = 0.0f;
    private float FireTimer = 0.00f;
    private AudioSource AudioSource;
    private bool bUnfiredWeapon = true;
    

    // Start is called before the first frame update
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        if (AudioSource == null) // if AudioSource component does not exist, add one
        {
            AudioSource = gameObject.AddComponent<AudioSource>();
        }

    }
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), WeaponName);
    }
    // Update is called once per frame
    void Update()
    {
        if (!bChargeUpWeapon)
        {
            //fully automatic code
            if (Input.GetMouseButton(0) && bFullyAutomatic)
            {
                FullAutoDelay -= Time.deltaTime;
                if (FireTimer == 0 & FullAutoDelay <= 0)
                {
                    float RandomSpreadY = UnityEngine.Random.Range(-BulletSpread, BulletSpread);
                    float RandomSpreadZ = UnityEngine.Random.Range(-BulletSpread, BulletSpread);
                    float RandomSpreadX = UnityEngine.Random.Range(-BulletSpread, BulletSpread);
                    
                    Quaternion rotation = Quaternion.Euler(RandomSpreadX, RandomSpreadY, RandomSpreadZ);
                    Vector3 rotatedVector = rotation * transform.parent.forward;
                    

                    GameObject newBullet = Instantiate(Bullet, transform.parent.position + transform.parent.forward, Quaternion.identity);
                    newBullet.GetComponent<Rigidbody>().AddForce(rotatedVector * ProjSpeed);
                    AudioSource.PlayOneShot(WeaponAudio);
                    Destroy(newBullet, 10f);
                }

                FireTimer += Time.deltaTime;

                if (FireTimer > FireRate)
                {
                    FireTimer = 0;
                }
            }

            if (Input.GetMouseButtonUp(0) && bFullyAutomatic)
            {
                FullAutoDelay = 1.0f;
            }

            //semi-auto code
            if (Input.GetMouseButtonUp(0) && !bFullyAutomatic && (FireTimer > FireRate || bUnfiredWeapon == true))
            {
                bUnfiredWeapon = false;
                
                for (int i = 0; i < ProjCount; i++)
                {
                    
                    float RandomSpreadY = UnityEngine.Random.Range(-BulletSpread, BulletSpread);
                    float RandomSpreadZ = UnityEngine.Random.Range(-BulletSpread, BulletSpread);
                    float RandomSpreadX = UnityEngine.Random.Range(-BulletSpread, BulletSpread);
                    
                    Quaternion rotation = Quaternion.Euler(RandomSpreadX, RandomSpreadY, RandomSpreadZ);  // specify your rotation values
                    Vector3 rotatedVector = rotation * transform.parent.forward;
                    GameObject newBullet = Instantiate(Bullet, transform.parent.position + transform.parent.forward, Quaternion.identity);
                    newBullet.GetComponent<Rigidbody>().AddForce(rotatedVector * ProjSpeed);
                    AudioSource.PlayOneShot(WeaponAudio);
                    Destroy(newBullet, 10f);

                }
                FireTimer -= FireTimer + FireRate;
            }

            if (!bFullyAutomatic)
            {
                FireTimer += Time.deltaTime;
            }
   
        }
        else //charge up weapon code
        {
            if (Input.GetMouseButton(0))
            {
                // Charge up the weapon
                ChargeTime += Time.deltaTime;

                // clamp charge time
                if (ChargeTime >= MaxChargeTime)
                {
                    ChargeTime = MaxChargeTime;
                }
            }
            else if (Input.GetMouseButtonUp(0) && bChargeUpWeapon)
            {
                    GameObject newBullet = Instantiate(Bullet, transform.parent.position + transform.parent.forward, Quaternion.identity);
                    newBullet.GetComponent<Rigidbody>().AddForce(transform.parent.forward * (ProjSpeed * ChargeTime));
                    AudioSource.PlayOneShot(WeaponAudio);
                    Destroy(newBullet, 10f);

                    // reset charge time and deactivate charge indicator
                ChargeTime = 0f;
            }
            
        }
        

    }
}
