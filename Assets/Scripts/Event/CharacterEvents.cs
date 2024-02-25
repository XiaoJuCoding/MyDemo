using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class CharacterEvents
{
    public static UnityEvent<GameObject, int> characterDamage = new UnityEvent<GameObject, int>();
    public static UnityEvent<GameObject, int> characterHealed = new UnityEvent<GameObject, int>();
}

