namespace RiraProject.Framework.Domain.ValueObject;
public abstract record BaseId<TId>(TId Value) where TId : notnull
{

    public static implicit operator TId(BaseId<TId> id) => id.Value;

  //  public override string ToString() => Value.ToString() ?? string.Empty;
}