using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private sbyte hp;
    [SerializeField] private sbyte maxHp;
    [SerializeField] private sbyte damage;
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

    public sbyte HP
    {
        get { return hp; }
        set { hp = value; }
    }

    public sbyte MaxHp
    {
        get { return maxHp; }
    }

    public sbyte Damage
    {
        get { return damage; }
    }
}
