using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;

namespace CrocCSharpBot
{
    /// <summary>
    /// Расширение для перечислемых типов
    /// 
    /// </summary>
   public static class EnumExtender
    {
        public static string ToDescription(this Enum enumerate)
        {
            Type type = enumerate.GetType();
            FieldInfo fildInfo = type.GetField(enumerate.ToString());
            var attributes = (DescriptionAttribute[])fildInfo.GetCustomAttributes(typeof(DescriptionAttribute),false);
            return (attributes.Length > 0) ? attributes[0].Description : enumerate.ToString();
        }
    }
}
