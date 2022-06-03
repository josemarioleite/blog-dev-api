using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using blog_api_dev.Statics;
using Microsoft.AspNetCore.Mvc;

namespace blog_api_dev.Utils
{
    public static class TypeUtils
    {
        public static JsonResult ReturnTypeResponseHTTP(bool success = true, Exception ex = null)
        {
            if (success)
            {
                return new JsonResult(new {
                    status = true,
                    code = 200,
                    message = ResponseHTTP.HTTP_200
                });
            } else {
                var exception = ex as Win32Exception;
                return new JsonResult(new {
                    status = false,
                    code = exception.ErrorCode,
                    message = ex.Message,
                    inner = ex.InnerException.Message
                });
            }
        }

        public static T como<T>(this object obj, bool enableException = false, T defaultValue = default(T))
        {
            try
            {
                if (obj == null)
                    return defaultValue;

                if (obj.GetType() == typeof(T))
                    return (T)obj;

                if (obj is Enum)
                {
                    if (typeof(T) == typeof(int) || typeof(T) == typeof(byte) || typeof(T) == typeof(Enum) || typeof(T) == typeof(long) || typeof(T) == typeof(short))
                        return (T)obj;

                    if (defaultValue.Equals(default(T)))
                        return ConvertToEnum<T>(obj);
                    return ConvertToEnum(obj, defaultValue);
                }

                return (T)obj;
            }
            catch (Exception ex)
            {
                if (enableException)
                    throw ex;

                if (typeof(T) == obj?.GetType() || typeof(T) == obj?.GetType().GetTypeInfo().BaseType)
                    return (T)obj;

                if (defaultValue?.Equals(default(T)) ?? true)
                    return ConvertTo<T>(obj);
                return ConvertTo(obj, defaultValue);
            }
        }

        public static T ConvertTo<T>(Object vl, T nullDefault = default(T))
        {
            if (typeof(T) == typeof(double))
                return ConvertToDouble(vl, nullDefault.como<double>()).como<T>();

            if (typeof(T) == typeof(decimal))
                return ConvertToDecimal(vl, nullDefault.como<decimal>()).como<T>();

            if (typeof(T) == typeof(int))
                return ConvertToInt32(vl, nullDefault.como<int>()).como<T>();

            if (typeof(T) == typeof(Int64))
                return ConvertToInt64(vl, nullDefault.como<Int64>()).como<T>();

            if (typeof(T) == typeof(bool))
                return ConvertToBoolean(vl, nullDefault.como<bool>()).como<T>();

            if (typeof(T) == typeof(DateTime))
                return ConvertToDate(vl, nullDefault.como<DateTime>()).como<T>();

            return nullDefault;
        }

        public static T ConvertToEnum<T>(Object vl, T nullDefault = default(T))
        {
            try
            {
                var r = (T)Enum.Parse(typeof(T), vl.ToString(), true);
                return r;
            }
            catch (Exception)
            {
                return nullDefault.Equals(default(T)) ? (T)Enum.GetValues(typeof(T)).GetValue(0) : nullDefault;
            }
        }

        public static Double ConvertToDouble(Object vl, Double nullDefault = 0)
        {
            Double r = 0;
            try
            {
                r = Convert.ToDouble(vl);
            }
            catch
            {
                r = nullDefault;
            }
            return r;
        }

        public static Decimal ConvertToDecimal(Object vl, Decimal nullDefault = 0)
        {
            Decimal r = 0;
            try
            {
                r = Convert.ToDecimal(vl);
            }
            catch
            {
                r = nullDefault;
            }
            return r;
        }

        public static int ConvertToInt32(Object vl, int nullDefault = 0)
        {
            int r = 0;
            try
            {
                r = Convert.ToInt32(vl);
            }
            catch
            {
                r = nullDefault;
            }
            return r;
        }

        public static Int64 ConvertToInt64(Object vl, Int64 nullDefault = 0)
        {
            Int64 r = 0;
            try
            {
                r = Convert.ToInt64(vl);
            }
            catch
            {
                r = nullDefault;
            }
            return r;
        }

        public static DateTime ConvertToDate(Object vl, DateTime nullDefault = default(DateTime))
        {
            if (vl is DateTime)
                return (DateTime)vl;

            DateTime r;
            try
            {
                r = Convert.ToDateTime(vl);
            }
            catch
            {
                r = nullDefault;
            }
            return r;
        }

        public static Boolean ConvertToBoolean(Object vl, Boolean nullDefault = false)
        {
            Boolean r;
            try
            {
                r = Convert.ToBoolean(vl);
            }
            catch
            {
                r = nullDefault;
            }
            return r;
        }

        public static string RemoveWordsSpecials (this string wordSpecial)
        {
            wordSpecial = wordSpecial.Normalize(System.Text.NormalizationForm.FormD);
            var chars = wordSpecial.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(System.Text.NormalizationForm.FormC);
        }
    }
}