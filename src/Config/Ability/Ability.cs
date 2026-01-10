namespace CS2ZombiePlague.Config.Ability;

public sealed class AbilityConfig
{
    public HealConfig Heal { get; set; } = new();
    
    public LeapConfig Leap { get; set; } = new();
}

public sealed class HealConfig : IAbilityConfig
{
    // Доступна ли способность
    public bool Enable { get; set; } = true;

    // Максимальная дистанция до цели для хила 
    public float MaxHealDistance { get; set; } = 350f;

    // Кол-во здоровья, которое будет восстановлено 
    public int HealAmount { get; set; } = 500;
    
    // Время в течение которого способность будет недоступна для повторного применения 
    public float CooldownTime { get; set; } = 30f;

    // Путь к визуальному эффекту способности
    public List<string> ParticleEffectNames { get; set; } = ["particles/kolka/part2.vpcf"];

    // Длительность визуального эффекта
    public float DurationParticleEffect { get; set; } = 2.0f;

    // Путь к звуковому эффекту способности
    public List<string> SoundEffectNames { get; set; } = [""];

    // Эффект после применения способности на таргете (экран покрывается определенным цветом)
    public bool HasScreenEffectAfterAbilityOnTarget { get; set; } = true;

    // Время когда эффект будет появляться и потухать (200 мс тратится на появление эффекта и 200 мс на растворение) 
    public uint DurationEffectAfterAbilityOnTarget { get; set; } = 200;

    // Время сколько эффект задержится на экране (Duration + holdTime + Duration = общее время эффекта на экране)
    public uint HoldTimeEffectAfterAbilityOnTarget { get; set; } = 75;

    // Кол-во красного в цвете эффекта от 0..255
    public byte RedColorEffectAfterAbilityOnTarget { get; set; } = 0;
    
    // Кол-во зеленного в цвете эффекта от 0..255
    public byte GreenColorEffectAfterAbilityOnTarget { get; set; } = 255;
    
    // Кол-во синего в цвете эффекта от 0..255
    public byte BlueColorEffectAfterAbilityOnTarget { get; set; } = 0;
    
    // Прозрачность эффекта от 0..255
    public byte AlphaEffectAfterAbilityOnTarget { get; set; } = 80;
} 

public sealed class LeapConfig : IAbilityConfig
{
    // Доступна ли способность
    public bool Enable { get; set; } = true;
    
    // Время в течение которого способность будет недоступна для повторного применения 
    public float CooldownTime { get; set; } = 10.0f;

    // Максимальная дистанция прыжка
    public float LeapDistance { get; set; } = 700f;
    
    // Высота прыжка
    public float LeapBoost { get; set; } = 400f;
} 