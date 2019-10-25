using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Collections;

namespace AGPC.FixedLayout
{
  
    public class FixedLayout 
    {
        /// <summary>
        /// Returns a concatenated string for all properties with  fixed layout settings defined
        /// </summary>
        public string ToConcatString()
        {
            IEnumerable<FieldDefinition> _fields = SetAllFixedLayoutSettings(this);
            string _delimiter = SetAllDelimiterSettings();
            string _fieldValueToConcat;
            StringBuilder _sb = new StringBuilder();
            foreach (FieldDefinition _field in _fields)
            {
                _fieldValueToConcat = _field.Value;


                //Setting up the whitespaces input side
                if (_field.WhiteSpaces == FieldDefinition.Side.Left)
                    _fieldValueToConcat = _fieldValueToConcat.PadLeft(_field.Length).Substring(0, _field.Length);
                else
                    _fieldValueToConcat = _fieldValueToConcat.PadRight(_field.Length).Substring(0, _field.Length);

                _sb.Append(_fieldValueToConcat);
                _sb.Append(_delimiter);
            }
            
            string _result = _sb.ToString();

            _sb.Clear();
            _sb = null;

            return _result;
        }


        /// <summary>
        /// Load all this object properties with fixed layout settings defined
        /// </summary>
        /// <param name="contentLine">Concatenated string for load all this object properties with fixed layout settings defined</param>
        public void ToLoadThisObject(string contentLine)
        {
            
            
            IEnumerable<FieldDefinition> _fields = SetAllFixedLayoutSettings(this);
            string _delimiter = SetAllDelimiterSettings();
            
            int _readPositions = 0;

            foreach (FieldDefinition _field in _fields)
            {
                object _value = contentLine.Substring(_readPositions, _field.Length).Trim();
                _readPositions = _readPositions + _field.Length + _delimiter.Length;

                //Assigning value into correspondent property.
                //The object property will refresh by reference using the assignment made by SetAllFixedLayoutSettings 
                //method passing obj parameter value to the ObjectReferenced property of FieldDefinition object
                _field.ObjectReferenced.GetType().GetProperty(_field.Name).SetValue(_field.ObjectReferenced, Convert.ChangeType(_value, _field.ObjectReferenced.GetType().GetProperty(_field.Name).PropertyType));

            }
        }

        private IEnumerable<FieldDefinition> SetAllFixedLayoutSettings(object obj)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                  .Where(x => x.CustomAttributes.Count() > 0).ToArray();


            if (properties.Length < 0)
                return null;
           
            List<FieldDefinition> _fields = new List<FieldDefinition>();

            for (int i = 0; i < properties.Length; i++)
            {
                object[] customAttributesArr = properties[i].GetCustomAttributes(true);

                foreach (Attribute customAttribute in customAttributesArr)
                {
                    if (customAttribute is FieldDefinition)
                    {
                        //Common Properties
                        ((FieldDefinition)customAttribute).Name = properties[i].Name;
                        ((FieldDefinition)customAttribute).Value = properties[i].GetValue(obj)?.ToString() ?? string.Empty;
                        ((FieldDefinition)customAttribute).ObjectReferenced = obj;

                        _fields.Add((FieldDefinition)customAttribute);
                    }
                    else if (customAttribute is FieldObjType)
                    {
                        if (properties[i].PropertyType.GetInterfaces().Contains(typeof(IEnumerable)))
                        {
                            //Lists, Collections, IEnumerable Properties
                            foreach (var _iEnumerableItem in (IEnumerable)properties[i].GetValue(obj, null))
                                _fields.AddRange(this.SetAllFixedLayoutSettings(_iEnumerableItem));
                        }
                        else
                            //Object Properties
                            _fields.AddRange(this.SetAllFixedLayoutSettings(properties[i].GetValue(obj)));
                    }
                }
            }

            return _fields;
        }

        private string SetAllDelimiterSettings()
        {
            IEnumerable<Attribute> customAttributesArr = this.GetType().GetCustomAttributes<Attribute>(true);
            string _result = string.Empty;

            foreach (Attribute customAttribute in customAttributesArr)
                if(customAttribute is FieldDelimiter)
                    _result = _result + ((FieldDelimiter)customAttribute).Delimiter;

            return _result;
        }
    }
}





