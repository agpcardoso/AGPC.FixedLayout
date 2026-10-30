/*********************************************************************
 * This project was built by Alexandre G.P Cardoso and it is available
 * for use in https://github.com/agpcardoso
 *
 * Please, don't remove this comment
 ********************************************************************/

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AGPC.FixedLayout
{
    public class FixedLayout
    {
        //   Metadata is shared across instances through the thread-safe cache, avoiding repeated reflection after a type is cached.
        private static readonly ConcurrentDictionary<Type, TypeMetadata> MetadataCache =
            new ConcurrentDictionary<Type, TypeMetadata>();

        /// <summary>
        /// Returns a concatenated string for all properties with fixed layout settings defined.
        /// </summary>
        public string ToConcatString()
        {
            //   A single buffer avoids creating temporary strings for each field.
            var result = new StringBuilder();
            string delimiter = GetMetadata(GetType()).Delimiter;

            AppendValues(this, result, delimiter);
            return result.ToString();
        }

        /// <summary>
        /// Loads this object's properties with values from a concatenated fixed-layout string.
        /// </summary>
        /// <param name="contentLine">The concatenated string to map to this object.</param>
        public void ToLoadThisObject(string contentLine)
        {
            string delimiter = GetMetadata(GetType()).Delimiter;
            int position = 0;

            LoadValues(this, contentLine, delimiter, ref position);
        }

        //   Walks fields in order and reuses cached metadata for nested objects.
        private static void AppendValues(object instance, StringBuilder result, string delimiter)
        {
            foreach (PropertyMetadata property in GetMetadata(instance.GetType()).Properties)
            {
                if (property.Definition != null)
                {
                    string value = property.Property.GetValue(instance, null)?.ToString() ?? string.Empty;
                    AppendField(result, value, property.Definition.Length, property.Definition.WhiteSpaces);
                    result.Append(delimiter);
                }

                if (property.IsObject)
                {
                    object value = property.Property.GetValue(instance, null);
                    if (!IsNullCollection(property, value))
                        AppendObjectValues(value, result, delimiter);
                }
            }
        }

        private static void AppendObjectValues(object value, StringBuilder result, string delimiter)
        {
            if (value == null)
                throw new NullReferenceException("A property marked with FieldObjType must not be null.");

            if (value is IEnumerable values)
            {
                foreach (object item in values)
                {
                    AppendValues(item, result, delimiter);
                }

                return;
            }

            AppendValues(value, result, delimiter);
        }

         private static void AppendField(StringBuilder result, string value, int length, FieldDefinition.Side whiteSpaces)
        {
            int valueLength = Math.Min(value.Length, length);
            int paddingLength = length - valueLength;

            if (whiteSpaces == FieldDefinition.Side.Left)
            {
                result.Append(' ', paddingLength);
                result.Append(value, 0, valueLength);
            }
            else
            {
                result.Append(value, 0, valueLength);
                result.Append(' ', paddingLength);
            }
        }

        // Reuses cached metadata and advances by each field width and delimiter while reading the record.
        private static void LoadValues(object instance, string contentLine, string delimiter, ref int position)
        {
            foreach (PropertyMetadata property in GetMetadata(instance.GetType()).Properties)
            {
                if (property.Definition != null)
                {
                    int length = property.Definition.Length;
                    string value = contentLine.Substring(position, length).Trim();
                    position += length + delimiter.Length;

                    property.Property.SetValue(
                        instance,
                        Convert.ChangeType(value, property.Property.PropertyType),
                        null);
                }

                if (property.IsObject)
                    LoadObjectValues(property.Property.GetValue(instance, null), contentLine, delimiter, ref position);
            }
        }

        // Recursively loads objects and lists while preserving the layout field order.
        private static void LoadObjectValues(object value, string contentLine, string delimiter, ref int position)
        {
            if (value == null)
                throw new NullReferenceException("A property marked with FieldObjType must not be null.");

            if (value is IEnumerable values)
            {
                foreach (object item in values)
                {
                    LoadValues(item, contentLine, delimiter, ref position);
                }

                return;
            }

            LoadValues(value, contentLine, delimiter, ref position);
        }

        private static bool IsNullCollection(PropertyMetadata property, object value)
        {
            return value == null && typeof(IEnumerable).IsAssignableFrom(property.Property.PropertyType);
        }

        private static TypeMetadata GetMetadata(Type type)
        {
            return MetadataCache.GetOrAdd(type, CreateMetadata);
        }

        /// <summary>
        ///     Collects the properties, attributes, and delimiter needed to build metadata for a type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static TypeMetadata CreateMetadata(Type type)
        {
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var propertyMetadata = new List<PropertyMetadata>(properties.Length);

            foreach (PropertyInfo property in properties)
            {
                FieldDefinition definition = null;
                bool isObject = false;
                object[] attributes = property.GetCustomAttributes(true);

                foreach (object attribute in attributes)
                {
                    if (attribute is FieldDefinition fieldDefinition)
                        definition = fieldDefinition;
                    else if (attribute is FieldObjType)
                        isObject = true;
                }

                if (definition != null || isObject)
                    propertyMetadata.Add(new PropertyMetadata(property, definition, isObject));
            }

            object[] typeAttributes = type.GetCustomAttributes(typeof(FieldDelimiter), true);
            var delimiter = new StringBuilder();
            foreach (FieldDelimiter attribute in typeAttributes)
                delimiter.Append(attribute.Delimiter);

            return new TypeMetadata(propertyMetadata.ToArray(), delimiter.ToString());
        }

        // Stores the configuration needed to serialize and load each property.
        private sealed class PropertyMetadata
        {
            internal PropertyMetadata(PropertyInfo property, FieldDefinition definition, bool isObject)
            {
                Property = property;
                Definition = definition;
                IsObject = isObject;
            }

            internal PropertyInfo Property { get; }
            internal FieldDefinition Definition { get; }
            internal bool IsObject { get; }
        }

        // A snapshot of the layout metadata stored in the cache for reuse across conversions.
        private sealed class TypeMetadata
        {
            internal TypeMetadata(PropertyMetadata[] properties, string delimiter)
            {
                Properties = properties;
                Delimiter = delimiter;
            }

            internal PropertyMetadata[] Properties { get; }
            internal string Delimiter { get; }
        }
    }
}















































































































































































/*********************************************************************
 * This project was built by Alexandre G.P Cardoso and it is available
 * for use in https://github.com/agpcardoso
 *
 * Please, don't remove this comment
 ********************************************************************/