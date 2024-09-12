
public enum DamagePopupType
{
    /// <summary>
    /// 일반 공격
    /// </summary>
    Normal,
    /// <summary>
    /// 치명타 공격
    /// </summary>
    Critical,
    /// <summary>
    /// 회복
    /// </summary>
    Heal,
    /// <summary>
    /// 무효 공격
    /// </summary>
    Stuck,
    Max
}
public enum AttackType
{
    /// <summary>
    /// 고정 데미지 : 방어력, 치명타, 경감 등의 효과를 무시함
    /// </summary>
    Fixed,
    /// <summary>
    /// 반사 데미지 : 다시 반사될 수 없음
    /// </summary>
    Reflect,
    Max
}

public enum ImmunitableEffect
{
    /// <summary>
    /// 크리티컬
    /// </summary>
    Critical = 0,
    /// <summary>
    /// 스턴 : 행동불능
    /// </summary>
    Stun,
    /// <summary>
    /// 저주 : 받는 피해 증가
    /// </summary>
    Curse,
    /// <summary>
    /// 감전 : 행동불능, 피격 시 추가 데미지
    /// </summary>
    ElectricShock,
    /// <summary>
    /// 속박 : 이동불능
    /// </summary>
    Restraint,
    None,
    Max
}

public enum StatusEffect
{
    /// <summary>
    /// 스턴 : 행동불능
    /// </summary>
    Stun = 0,

    /// <summary>
    /// 저주 : 받는 피해 증가
    /// </summary>
    Curse,

    /// <summary>
    /// 감전 : 행동불능, 피격 시 추가 데미지
    /// </summary>
    ElectricShock,

    /// <summary>
    /// 속박 : 이동불능
    /// </summary>
    Restraint,

    /// <summary>
    /// 불사 : 죽지 않음
    /// </summary>
    Immortal,

    /// <summary>
    /// 보호막 (수치형)
    /// </summary>
    Shield,

    /// <summary>
    /// 최종 데미지 제한 (최대 체력 비례)
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
    /// 전체 방어력 증가
    /// </summary>
    UnitPassive_AllDefUp,
    /// <summary>
    /// 공격 속도 증가
    /// </summary>
    UnitPassive_AtkSpeedUp,
    /// <summary>
    /// 전체 공격력 증가
    /// </summary>
    UnitPassive_AllDamageUp,
    /// <summary>
    /// 전체 최대 체력 증가
    /// </summary>
    UnitPassive_AllMaxHPUp,
    /// <summary>
    /// 불사
    /// </summary>
    UnitPassive_Immortal,
    /// <summary>
    /// 3회 피격시 전체공격
    /// </summary>
    UnitPassive_RoyalJudgment,
    /// <summary>
    /// 로미 차지공격
    /// </summary>
    UnitPassive_Charge_Romi,
    /// <summary>
    /// 범위 스턴 공격
    /// </summary>
    UnitPassive_SplashAndStun,

    None,
    Max
}

public enum ActiveSkillType
{
    /// <summary>
    /// 보호막, 최종 데미지 제한
    /// </summary>
    UnitActive_RoyalGuard,
    /// <summary>
    /// 차징 속도 증가 (타임스케일 증가)
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