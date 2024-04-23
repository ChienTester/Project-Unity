using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [SerializeField] GameObject Monster1Prefab;
    [SerializeField] GameObject Monster2Prefab;
    [SerializeField] GameObject Monster3Prefab;
    [SerializeField] GameObject MonsterFlyPrefab;
    [SerializeField] GameObject MonsterBossPrefab;

    [SerializeField] GameObject Monster1_Map2Prefab;
    [SerializeField] GameObject Monster2_Map2Prefab;
    [SerializeField] GameObject Monster3_Map2Prefab;
    [SerializeField] GameObject Monster4_Map2Prefab;

    [SerializeField] GameObject whipPrefab;
    [SerializeField] GameObject bombPrefab;
    [SerializeField] GameObject gunPrefab;

    [SerializeField] GameObject blueCrystalPrefab;
    [SerializeField] GameObject greenCrystalPrefab;
    [SerializeField] GameObject redCrystalPrefab;

    [SerializeField] GameObject redHeartPrefab;

    [SerializeField] GameObject DamageText;

    static ObjectPooling instance;
    Dictionary<string, Queue<GameObject>> poolingDict = new Dictionary<string, Queue<GameObject>>();

    const int initNumEnemy = 200;
    const int initNumWeapon = 200;
    const int initNumCrystal = 200;
    const int initNumDamage = 200;
    const int initNumHeart = 200;

    void Awake()
    {
        instance = this;
        Initialize();
    }

    void Initialize()
    {
        foreach (CharacterData.CharacterType characterType in Enum.GetValues(typeof(CharacterData.CharacterType)))
        {
            if (IsPlayer(characterType)) continue;

            Queue<GameObject> newQue = new Queue<GameObject>();

            for (int j = 0; j < initNumEnemy; j++)
            {
                newQue.Enqueue(CreateObject(characterType));
            }

            poolingDict.Add(characterType.ToString(), newQue);
        }

        foreach (WeaponData.WeaponType weaponType in Enum.GetValues(typeof(WeaponData.WeaponType)))
        {
            Queue<GameObject> newQue = new Queue<GameObject>();

            for (int j = 0; j < initNumWeapon; j++)
            {
                newQue.Enqueue(CreateObject(weaponType));
            }

            poolingDict.Add(weaponType.ToString(), newQue);
        }

        foreach (CrystalData.CrystalType crystalType in Enum.GetValues(typeof(CrystalData.CrystalType)))
        {
            Queue<GameObject> newQue = new Queue<GameObject>();

            for (int j = 0; j < initNumCrystal; j++)
            {
                newQue.Enqueue(CreateObject(crystalType));
            }

            poolingDict.Add(crystalType.ToString(), newQue);
        }

        foreach (HeartData.HeartType heartType in Enum.GetValues(typeof(HeartData.HeartType)))
        {
            Queue<GameObject> newQue = new Queue<GameObject>();

            for (int j = 0; j < initNumHeart; j++)
            {
                newQue.Enqueue(CreateObject(heartType));
            }

            poolingDict.Add(heartType.ToString(), newQue);
        }

        Queue<GameObject> damageQue = new Queue<GameObject>();

        for (int j = 0; j < initNumDamage; j++)
        {
            damageQue.Enqueue(CreateObject("damage"));
        }

        poolingDict.Add("damage", damageQue);
    }

    static GameObject CreateObject<T>(T type)
    {
        GameObject newObject;
        bool isParentPlayer = false;

        switch (type)
        {
            default:
            case CharacterData.CharacterType.Monster1:
                newObject = Instantiate(instance.Monster1Prefab);
                break;

            case CharacterData.CharacterType.Monster2:
                newObject = Instantiate(instance.Monster2Prefab);
                break;

            case CharacterData.CharacterType.Monster3:
                newObject = Instantiate(instance.Monster3Prefab);
                break;
            case CharacterData.CharacterType.MonsterFly:
                newObject = Instantiate(instance.MonsterFlyPrefab);
                break;
            case CharacterData.CharacterType.MonsterBoss:
                newObject = Instantiate(instance.MonsterBossPrefab);
                break;

            case CharacterData.CharacterType.Monster1_Map2:
                newObject = Instantiate(instance.Monster1_Map2Prefab);
                break;

            case CharacterData.CharacterType.Monster2_Map2:
                newObject = Instantiate(instance.Monster2_Map2Prefab);
                break;

            case CharacterData.CharacterType.Monster3_Map2:
                newObject = Instantiate(instance.Monster3_Map2Prefab);
                break;

            case CharacterData.CharacterType.Monster4_Map2:
                newObject = Instantiate(instance.Monster4_Map2Prefab);
                break;


            case WeaponData.WeaponType.Whip:
                newObject = Instantiate(instance.whipPrefab);
                if (ItemAssets.GetInstance().GetWeaponData(WeaponData.WeaponType.Whip).GetParent().Equals(WeaponData.Parent.Player))
                    isParentPlayer = true;
                break;
            case WeaponData.WeaponType.Bomb:
                newObject = Instantiate(instance.bombPrefab);
                if (ItemAssets.GetInstance().GetWeaponData(WeaponData.WeaponType.Bomb).GetParent().Equals(WeaponData.Parent.Player))
                    isParentPlayer = true;
                break;
            case WeaponData.WeaponType.Gun:
                newObject = Instantiate(instance.gunPrefab);
                if (ItemAssets.GetInstance().GetWeaponData(WeaponData.WeaponType.Gun).GetParent().Equals(WeaponData.Parent.Player))
                    isParentPlayer = true;
                break;

            case CrystalData.CrystalType.blue:
                newObject = Instantiate(instance.blueCrystalPrefab);
                break;
            case CrystalData.CrystalType.green:
                newObject = Instantiate(instance.greenCrystalPrefab);
                break;
            case CrystalData.CrystalType.red:
                newObject = Instantiate(instance.redCrystalPrefab);
                break;

            case HeartData.HeartType.redHeart:
                newObject = Instantiate(instance.redHeartPrefab);
                break;

            case "damage":
                newObject = Instantiate(instance.DamageText);
                break;
        }

        if (isParentPlayer)
            newObject.transform.parent = GameObject.FindWithTag("Weapon").transform;
        else
            newObject.transform.parent = instance.transform;

        newObject.SetActive(false);

        return newObject;
    }

    public static GameObject GetObject<T>(T type)
    {
        if (instance.poolingDict[type.ToString()].Count > 0)
        {
            return instance.poolingDict[type.ToString()].Dequeue();
        }
        else
        {
            return CreateObject(type);
        }
    }

    public static void ReturnObject<T>(GameObject deadEnemy, T type)
    {
        instance.poolingDict[type.ToString()].Enqueue(deadEnemy);
    }

    bool IsPlayer(CharacterData.CharacterType characterType)
    {
        switch (characterType)
        {
            case CharacterData.CharacterType.WhipPlayer:
            case CharacterData.CharacterType.GunPlayer:
                return true;
            default:
                return false;
        }
    }
}
