namespace Domain.ValueObjects;

public abstract class BaseValueObject
{
    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }
        
        
        //TODO: как сравнивать (Deep clone, Deep Compare)
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}