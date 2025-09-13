namespace Code.GameEntyties.Target
{
    interface IEntityBase
    {
        int Hp { get; set; }
        void TakeDamage(DamageInfo _info);
        void Deactivate();
    }
}
