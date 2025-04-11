
using System.Reflection;

namespace Domain.Extensions;

public static class MapExtensions
{

    // Extension method to map a object to antoher object using TDestination
    public static TDestination MapTo<TDestination>(this object source)
    {
        // if source is null,  throw exception
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        // create new instance of TDestination (ex UserEntity)
        TDestination destination = (TDestination)Activator.CreateInstance(typeof(TDestination))!;

        // get all properties from source object
        var sourceProperties = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        // get all properties from the destination object.( our mapping target)
        var destinationProperties = destination.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        // loops through all properties in the destinations object
        foreach (var destinationProperty in destinationProperties)
        {
            // tries to find matching property with same name and type
            var sourceProperty = sourceProperties.FirstOrDefault(x =>
                x.Name == destinationProperty.Name &&
                x.PropertyType == destinationProperty.PropertyType);

            // if a matching property is found AND its writable , copy over the values
            if (sourceProperty != null && destinationProperty.CanWrite)
            {
                // gets the value from source
                var value = sourceProperty.GetValue(source);

                // set the value on the destination prop
                destinationProperty.SetValue(destination, value);
            }
        }

        // return the object I mapped to.
        return destination;
    }
}

