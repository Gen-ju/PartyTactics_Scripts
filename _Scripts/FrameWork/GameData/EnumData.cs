
public enum DamagePopupType
{
    /// <summary>
    /// �Ϲ� ����
    /// </summary>
    Normal,
    /// <summary>
    /// ġ��Ÿ ����
    /// </summary>
    Critical,
    /// <summary>
    /// ȸ��
    /// </summary>
    Heal,
    /// <summary>
    /// ��ȿ ����
    /// </summary>
    Stuck,
    Max
}
public enum AttackType
{
    /// <summary>
    /// ���� ������ : ����, ġ��Ÿ, �氨 ���� ȿ���� ������
    /// </summary>
    Fixed,
    /// <summary>
    /// �ݻ� ������ : �ٽ� �ݻ�� �� ����
    /// </summary>
    Reflect,
    Max
}

public enum ImmunitableEffect
{
    /// <summary>
    /// ũ��Ƽ��
    /// </summary>
    Critical = 0,
    /// <summary>
    /// ���� : �ൿ�Ҵ�
    /// </summary>
    Stun,
    /// <summary>
    /// ���� : �޴� ���� ����
    /// </summary>
    Curse,
    /// <summary>
    /// ���� : �ൿ�Ҵ�, �ǰ� �� �߰� ������
    /// </summary>
    ElectricShock,
    /// <summary>
    /// �ӹ� : �̵��Ҵ�
    /// </summary>
    Restraint,
    None,
    Max
}

public enum StatusEffect
{
    /// <summary>
    /// ���� : �ൿ�Ҵ�
    /// </summary>
    Stun = 0,

    /// <summary>
    /// ���� : �޴� ���� ����
    /// </summary>
    Curse,

    /// <summary>
    /// ���� : �ൿ�Ҵ�, �ǰ� �� �߰� ������
    /// </summary>
    ElectricShock,

    /// <summary>
    /// �ӹ� : �̵��Ҵ�
    /// </summary>
    Restraint,

    /// <summary>
    /// �һ� : ���� ����
    /// </summary>
    Immortal,

    /// <summary>
    /// ��ȣ�� (��ġ��)
    /// </summary>
    Shield,

    /// <summary>
    /// ���� ������ ���� (�ִ� ü�� ���)
    /// </summary>
    LimitDamageAmount,


    Damage_Up,
    CoolSpeed_Up,
    AtkSpeed_Up,
    Range_Up,
    Scale_Up,
    Defense_Up,
    MaxHp_Up,
    Speed_Up,
    CriPercent_Up,
    CriDamage_Up,
    TimeScale_Up,
    Reduce_Up,
    Damage_Down,
    CoolSpeed_Down,
    AtkSpeed_Down,
    Range_Down,
    Scale_Down,
    Defense_Down,
    MaxHp_Down,
    Speed_Down,
    CriPercent_Down,
    CriDamage_Down,
    TimeScale_Down,
    Reduce_Down,
    Max,
}

public enum PassiveSkillType
{
    /// <summary>
    /// ��ü ���� ����
    /// </summary>
    UnitPassive_AllDefUp,
    /// <summary>
    /// ���� �ӵ� ����
    /// </summary>
    UnitPassive_AtkSpeedUp,
    /// <summary>
    /// ��ü ���ݷ� ����
    /// </summary>
    UnitPassive_AllDamageUp,
    /// <summary>
    /// ��ü �ִ� ü�� ����
    /// </summary>
    UnitPassive_AllMaxHPUp,
    /// <summary>
    /// �һ�
    /// </summary>
    UnitPassive_Immortal,
    /// <summary>
    /// 3ȸ �ǰݽ� ��ü����
    /// </summary>
    UnitPassive_RoyalJudgment,
    /// <summary>
    /// �ι� ��������
    /// </summary>
    UnitPassive_Charge_Romi,
    /// <summary>
    /// ���� ���� ����
    /// </summary>
    UnitPassive_SplashAndStun,

    None,
    Max
}

public enum ActiveSkillType
{
    /// <summary>
    /// ��ȣ��, ���� ������ ����
    /// </summary>
    UnitActive_RoyalGuard,
    /// <summary>
    /// ��¡ �ӵ� ���� (Ÿ�ӽ����� ����)
    /// </summary>
    UnitActive_Romi,
    None,
    Max
}

public enum BuffType
{
   Damage = 0,
   CoolSpeed,
   AtkSpeed,
   Range,
   Scale,
   Defense,
   MaxHp,
   Speed,
   CriPercent,
   CriDamage,
   TimeScale,
   Reduce,
   Max
}

public enum TraitState
{
    OnStart,
    OnAttack,
    OnCritical,
    OnDamaged,
    OnDeath,
    OnKill,
    OnEnd,
    Max
}