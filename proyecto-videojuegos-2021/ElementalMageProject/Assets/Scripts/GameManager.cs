using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static float _playerHealth = 12f;
    private static float _maxPlayerHealth = 12f;
    private static bool _dashUnlocked = false;
    private static bool _shieldUnclocked = false;
    private static bool _debuffUnlocked = false;
    private static bool _lifestealUnlocked = false;

    private static bool _fire_boos_defeated = false;
    private static bool _water_boos_defeated = false;
    private static bool _air_boos_defeated = false;
    private static bool _earth_boos_defeated = false;
    private static int _bosses_defeated = 0;

    private static Directions _lastDirection = Directions.NONE;

    public static Directions LastDirection
    {
        get
        {
            return _lastDirection;
        }
        set
        {
            _lastDirection = value;
        }
    }

    public static float MaxPlayerHealth
    {
        get
        {
            return _maxPlayerHealth;
        }
        set
        {
            _maxPlayerHealth = value;
        }
    }

    public static float PlayerHealth
    {
        get
        {
            return _playerHealth;
        }
        set
        {
            _playerHealth = value;
        }
    }

    public static bool DashUnlocked
    {
        get
        {
            return _dashUnlocked;
        }
        set
        {
            _dashUnlocked = value;
        }
    }

    public static bool ShieldUnclocked
    {
        get
        {
            return _shieldUnclocked;
        }
        set
        {
            _shieldUnclocked = value;
        }
    }

    public static bool DebuffUnlocked
    {
        get
        {
            return _debuffUnlocked;
        }
        set
        {
            _debuffUnlocked = value;
        }
    }

    public static bool LifestealUnlocked
    {
        get
        {
            return _lifestealUnlocked;
        }
        set
        {
            _lifestealUnlocked = value;
        }
    }

    public static bool FireBossDefeated
    {
        get
        {
            return _fire_boos_defeated;
        }
        set
        {
            _fire_boos_defeated = value;
        }
    }

    public static bool AirBossDefeated
    {
        get
        {
            return _air_boos_defeated;
        }
        set
        {
            _air_boos_defeated = value;
        }
    }

    public static bool WaterBossDefeated
    {
        get
        {
            return _water_boos_defeated;
        }
        set
        {
            _water_boos_defeated = value;
        }
    }

    public static bool EarthBossDefeated
    {
        get
        {
            return _earth_boos_defeated;
        }
        set
        {
            _earth_boos_defeated = value;
        }
    }

    public static int BossesDefeated
    {
        get
        {
            return _bosses_defeated;
        }
        set
        {
            _bosses_defeated = value;
        }
    }
}
