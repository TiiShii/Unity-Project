using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;
    public int maxHP;
    public int currentHP;
    public int damage;
    public List<Spell> spells;  // Ajoutez cette ligne pour stocker les sorts de l'unité

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
            return true;
        else
            return false;
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }

    // Ajoutez une méthode pour gérer l'utilisation du sort
    public bool CastSpell(Spell spell, Unit target)
    {
        target.TakeDamage(spell.damage);
        return target.currentHP <= 0;
    }
}
