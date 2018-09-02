using System;

namespace WVB.Framework.EntityFrameworkRepository.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class HistorificarAttribute : Attribute
    {
    }
}
