public readonly struct EcsEntity
{
    public int Id { get; }

    public EcsEntity(int id)
    {
        Id = id;
    }

    public bool IsValid => Id >= 0;
}
