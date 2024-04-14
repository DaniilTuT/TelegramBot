using System.Text.Json;

namespace Domain.ValueObjects;

public abstract class BaseValueObject
{
    
    public override bool Equals(object? obj)
    {
        if (obj == null || obj is not BaseValueObject valueObject)
            return false;
        
        var thisSerialize = JsonSerializer.Serialize(this);
        var valueSerialize = JsonSerializer.Serialize(valueObject);
        if (string.Compare(thisSerialize, valueSerialize) !=0)
            return false;
        
        return true;
    }

    public override int GetHashCode()
    {
        return JsonSerializer.Serialize(this).GetHashCode();
    }
}