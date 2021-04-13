using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float lookSensitivity = 8f;
    public float gravityScale = -10f;
    public float groundDistance = 0.4f;
    public float jumpStr = 10f;
    public Transform groundCheck;
    private float fallVelocity;
    public LayerMask groundLayer;
    public bool onGround = true;
    private CharacterController controller;

    public int maxHealth;
    public int maxShield;
    public int health;
    public int shield;

    private float moveX;
    private float moveZ;
    private Vector3 direction;
    float mouseX;

    public Gun equippedGun;
    public Camera cam;
    public bool aimDownSight = false;
    bool moveGun = false;
    bool reloading = false;
    bool crouching = false;

    public float respawnTime;

    public SpawnPointSO spawns;

    public AudioSource playerAudio;
    public Transform diePos;

    public GameObject GFX;

    Vector3 lastSpawn;
    public Image deathScreen;

    public ParticleSystem bloodEffect;
    public GameObject hitMarker;

    public int kills;
    public int deaths;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.visible = false;
        health = maxHealth;

        lastSpawn = spawns.spawnPoints[Random.Range(0, spawns.spawnPoints.Length - 1)];
        transform.position = lastSpawn;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Shield")
        {
            shield = maxShield;
            Destroy(other.gameObject);
        }
    }

    private void Update()
    {
        if (equippedGun.currentAmmo == 0 && !reloading) StartCoroutine(ReloadWeapon());
        onGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

        //if on ground stay on the ground
        if(onGround && fallVelocity < 0)
        {
            fallVelocity = -2f;
        }
        //otherwise apply gravity
        fallVelocity += gravityScale * Time.deltaTime;
        controller.Move(new Vector3(0f, fallVelocity, 0f) * Time.deltaTime);

        //rotate player based on camera rotation
        transform.Rotate(Vector3.up, mouseX * Time.deltaTime);
        //move the player
        if(direction.magnitude >= 0.1f)
        {
            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            controller.Move(move * speed * Time.deltaTime);
        }

        //Aim down sight or return to hip position depending on player's state
        if (aimDownSight && equippedGun.gameObject.transform.localPosition != equippedGun.gunObject.adsLocation && moveGun) 
        {
            equippedGun.animator.enabled = false;
            equippedGun.gameObject.transform.localPosition = Vector3.Lerp(equippedGun.gameObject.transform.localPosition, equippedGun.gunObject.adsLocation, Time.deltaTime * equippedGun.gunObject.aimSpeed);
            if (equippedGun.gameObject.transform.localPosition == equippedGun.gunObject.adsLocation)
            {
                moveGun = false;
            }
        }
        else if(aimDownSight && equippedGun.gameObject.transform.localPosition == equippedGun.gunObject.adsLocation)
        {
            equippedGun.transform.localPosition = equippedGun.gunObject.adsLocation;
        }
        else
        {
            equippedGun.animator.enabled = true;
            equippedGun.gameObject.transform.localPosition = Vector3.Lerp(equippedGun.gameObject.transform.localPosition, equippedGun.gunObject.holdLocation, Time.deltaTime * equippedGun.gunObject.aimSpeed);
        }
    }

    public void OnMove(InputValue value)
    {
        var moveDir = value.Get<Vector2>();
        moveX = moveDir.x;
        moveZ = moveDir.y;
        direction = new Vector3(moveX, 0f, moveZ);
    }

    public void OnLook(InputValue value)
    {
        var lookDir = value.Get<Vector2>();
        mouseX = lookDir.x * lookSensitivity;
    }

    void OnAim()
    {
        if (equippedGun != null)
        {
            Debug.Log("AIM");
            aimDownSight = !aimDownSight;
            moveGun = true;
        }
    }

    void OnFire()
    {
        if(equippedGun != null && Time.time > equippedGun.nextFire)
        {
            equippedGun.nextFire = Time.time + equippedGun.gunObject.fireRate;
            if (equippedGun.currentAmmo > 0 && !reloading)
            {
                equippedGun.currentAmmo--;
                equippedGun.muzzleFlash.Play();
                StartCoroutine(ShotEffect());

                //setup raycast
                Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));

                RaycastHit hit;


                //check if ray hit anything
                if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, equippedGun.gunObject.weaponRange))
                {
                    if (hit.collider.gameObject.tag == "Shootable")
                    {
                        //make bullet hole
                        SpawnBulletHole(hit);
                    }
                    else if(hit.collider.gameObject.tag == "Player")
                    {
                        hit.collider.GetComponent<PlayerMovement>().TakeDamage((int)equippedGun.gunObject.baseDamage, this);
                        StartCoroutine(HitMarker());
                    }
                }
            } else
            {
                playerAudio.clip = equippedGun.gunObject.emptySound;
                playerAudio.Play();
                if(!reloading)
                    StartCoroutine(ReloadWeapon());
            }
        }
    }

    void SpawnBulletHole(RaycastHit hit)
    {
        var decal = Instantiate(equippedGun.gunObject.bulletHole);
        decal.transform.position = hit.point + hit.normal * 0.001f;
        decal.transform.forward = hit.normal * -1f;
    }
    private IEnumerator ReloadWeapon()
    {
        equippedGun.animator.enabled = true;
        reloading = true;
        equippedGun.animator.SetBool("Reloading", true);
        equippedGun.animator.speed = equippedGun.gunObject.reloadClip.length / equippedGun.gunObject.reloadTime;
        yield return new WaitForSeconds(equippedGun.gunObject.reloadTime);
        equippedGun.animator.SetBool("Reloading", false);
        equippedGun.currentAmmo = equippedGun.gunObject.maxAmmo;
        reloading = false;
    }
    private IEnumerator ShotEffect()
    {
        playerAudio.clip = equippedGun.gunObject.gunShot;
        playerAudio.Play();
        yield return new WaitForSeconds(1.0f);

    }

    private IEnumerator HitMarker()
    {
        hitMarker.SetActive(true);
        yield return new WaitForSeconds(.25f);
        hitMarker.SetActive(false);
    }

    void OnJump()
    {
        Debug.Log("Jumping");
        fallVelocity += jumpStr;
    }

    void OnReload()
    {
        if(equippedGun != null)
        {
            if (equippedGun.currentAmmo != equippedGun.gunObject.maxAmmo) StartCoroutine(ReloadWeapon());
        }
    }

    void OnCrouch()
    {
        crouching = !crouching;
        CrouchPlayer();
    }

    void CrouchPlayer()
    {
        if (crouching)
            controller.height *= .5f;
        else
            controller.height *= 2;
    }

    void TakeDamage(int damage, PlayerMovement damager)
    {
        if (shield > 0)
        {
            shield -= damage;
        }
        if(shield == 0) health -= damage;
        if (shield < 0)
        {
            health -= Mathf.Abs(shield);
            shield = 0;
        }
        if (health <= 0)
        {
            StartCoroutine(Die());
            damager.kills++;
        }
        bloodEffect.Play();
    }

    private IEnumerator Die()
    {
        deaths++;
        deathScreen.gameObject.SetActive(true);
        shield = 0;
        GFX.SetActive(false);
        equippedGun.gameObject.SetActive(false);
        controller.enabled = false;
        yield return new WaitForSeconds(respawnTime);
        Vector3 temp = Vector3.zero;
        do
        {
            temp = spawns.spawnPoints[Random.Range(0, spawns.spawnPoints.Length - 1)];
        } while (temp == lastSpawn);
        transform.position = temp;
        lastSpawn = temp;
        controller.enabled = true;
        GFX.gameObject.SetActive(true);
        equippedGun.gameObject.SetActive(true);
        health = maxHealth;
        deathScreen.gameObject.SetActive(false);
    }

}
