using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Scriptable Object/Heart Data", order = int.MaxValue)]
public class HeartData : ScriptableObject
{
    public enum HeartType
    {
        redHeart
    }

    [SerializeField] Sprite sprite;
    [SerializeField] RuntimeAnimatorController controller;
    [SerializeField] HeartType heartType;
    [SerializeField] float bloodValue;

    public float GetBloodValue()
    {
        return bloodValue;
    }

    public HeartType GetHeartType()
    {
        return heartType;
    }

    public RuntimeAnimatorController GetController()
    {
        return controller;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }
}
