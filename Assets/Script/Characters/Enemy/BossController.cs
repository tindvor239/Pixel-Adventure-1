using UnityEngine;
using UnityEngine.UI;

public class BossController : EnemyController
{
    // Start is called before the first frame update
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed;
    [SerializeField] Slider healthBar;
    [SerializeField] float shootTime = 2f;

    SceneController sceneController;
    public override void Start()
    {
        base.Start();
        sceneController = SceneController.instance;
        healthBar.maxValue = Stats.MaxHp;

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        switch (sceneController.gameState)
        {
            case SceneController.GameState.Play:
                SetHealth();
                Shoot();
                break;
        }
    }
    void SetHealth()
    {
        healthBar.value = Stats.HP;
    }

    void Shoot()
    {
        shootTime -= Time.deltaTime;
        if (shootTime <= 0.0f)
        {
            // create gameobject.
            if(Player != null)
            {
                GameObject newBullet;
                newBullet = Instantiate(bullet);
                newBullet.transform.position = gameObject.transform.position;

                // if player NOT null.
                // set rotation for bullet.
                Vector2 direction = Player.transform.position - newBullet.transform.position;
                float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                newBullet.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);

                Rigidbody2D rigidbody = newBullet.GetComponent<Rigidbody2D>();
                rigidbody.velocity = newBullet.transform.right * bulletSpeed;
                shootTime = 2f;
            }
        }
    }
}
