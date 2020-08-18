using UnityEngine;
using UnityEngine.UI;

public class BossController : EnemyController
{
    // Start is called before the first frame update
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed;
    [SerializeField] Slider healthBar;
    float shootTime = 1f;
    float shootDelayTime;
    public override void Start()
    {
        base.Start();
        shootDelayTime = shootTime;
        healthBar.maxValue = Stats.MaxHp;

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        SetHealth();
        //Shoot();
        if(Input.GetKeyDown(KeyCode.Space))
        {
            int ml;
            ml = countUp(73);
            print(ml);
        }
    }

    int countUp(int index)
    {
        int multi = 5;
        do
        {
            multi += multi;
            print("On mul: " + multi);
        }
        while (multi - index > 0 && multi - index < 3);
        return multi;
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
            print("In");
            GameObject newBullet;
            newBullet = Instantiate(bullet);
            newBullet.transform.position = gameObject.transform.position;

            Vector2 direction = transform.position - newBullet.transform.position;
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            newBullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Rigidbody2D rigidbody = newBullet.GetComponent<Rigidbody2D>();
            rigidbody.velocity = newBullet.transform.forward * bulletSpeed * Time.deltaTime;
            shootTime = shootDelayTime;
        }
    }
}
