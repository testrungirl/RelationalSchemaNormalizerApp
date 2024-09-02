using RelationalSchemaNormalizerLibrary.Models;

namespace RelationalSchemaNormalizerLibrary.Utilities
{
    public static class HelperClass
    {
        public static object ConvertValue(string value, string dataType)
        {
            return dataType.ToLower() switch
            {
                "string" => value,
                "boolean" => bool.TryParse(value, out var boolValue) ? boolValue : throw new ArgumentException($"Invalid boolean value: {value}"),
                "char" => char.TryParse(value, out var charValue) ? charValue : throw new ArgumentException($"Invalid char value: {value}"),
                "datetime" => DateTime.TryParse(value, out var dateTimeValue)
                        ? (object)dateTimeValue.ToString("yyyy-MM-dd HH:mm:ss")// Convert to SQL Server compatible format
                        : throw new ArgumentException($"Invalid date value: {value}"),
                "double" => double.TryParse(value, out var doubleValue) ? doubleValue : throw new ArgumentException($"Invalid double value: {value}"),
                "float" => float.TryParse(value, out var floatValue) ? floatValue : throw new ArgumentException($"Invalid float value: {value}"),
                "guid" => Guid.TryParse(value, out var guidValue) ? guidValue : throw new ArgumentException($"Invalid GUID value: {value}"),
                "int" => int.TryParse(value, out var intValue) ? intValue : throw new ArgumentException($"Invalid integer value: {value}"),
                _ => throw new ArgumentException($"Unsupported data type: {dataType}")
            };
        }
        public static List<string> GetKeyAttributes(List<AttributeDetail> attributes)
        {
            var attributesList = attributes.Where(attr => attr.KeyAttribute).ToList();
            if (attributesList.Count > 0)
            {
                return attributesList.Select(attr => attr.AttributeName).ToList();
            }
            return new List<string>();
        }
        public static List<string> GetNonKeyAttributes(List<AttributeDetail> attributes)
        {
            var attributesList = attributes.Where(attr => !attr.KeyAttribute).ToList();
            if (attributesList.Count > 0)
            {
                return attributesList.Select(attr => attr.AttributeName).ToList();
            }
            return new List<string>();
        }
        public static List<string> GenerateKeySubset(List<string> keyAttributes, int subsetIndex)
        {
            var keySubset = new List<string>();

            for (int j = 0; j < keyAttributes.Count; j++)
            {
                if ((subsetIndex & (1 << j)) != 0)
                {
                    keySubset.Add(keyAttributes[j]);
                }
            }

            return keySubset;
        }
    }
}
