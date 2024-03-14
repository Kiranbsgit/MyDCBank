using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

public class EnumToIntegerConverter<TEnum> : ValueConverter<TEnum, int> where TEnum : Enum
{
    public EnumToIntegerConverter(ConverterMappingHints mappingHints = null)
        : base(
              enumValue => Convert.ToInt32(enumValue),
              intValue => (TEnum)Enum.ToObject(typeof(TEnum), intValue),
              mappingHints
          )
    {
    }
}