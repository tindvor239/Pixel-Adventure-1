using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    private int hp;
    [SerializeField] private int maxHp;
    [SerializeField] private int damage;
    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
    }

    void Update()
    {
        SetHP();
    }

    void SetHP()
    {
        if (hp > maxHp)
        {
            hp = maxHp;
        }
        else if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public int HP
    {
        get { return hp; }
        set { hp = value; }
    }

    public int MaxHp
    {
        get { return maxHp; }
    }

    public int Damage
    {
        get { return damage; }
    }
}
