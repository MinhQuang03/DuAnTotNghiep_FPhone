using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Utilities
{
    public static class FphoneConst
    {
        public const int HoatDong = 0;
        public const int KhongHoatDong = 1;

        public const int ChuaBan = 1;
        public const int Daban = 2;
        //deliveryPaymentMethod của bảng hóa đơn

        //dùng off
        public const string ChuyenKhoan = "BANKING";
        public const string TienMat = "TIENMAT";
        public const string TienMat_ChuyenKhoan = "TIENMAT_CHUYENKHOAN";
        //dùng online
        public const string VnPay = "VnPay ";
        public const string COD = "COD";    
        public const string TT = "TT";

        //Trạn thái hóa đơn
        public const int DaGiao = 3;
        public const int DaXacNhan = 1;
        public const int DangGiao = 2;
        public const int Huy = 4;
        public const int GiaoThatBai = 5;



        public const int ChoXacNhan = 0;
        public const int HoanThanh = 0;
        //StatusPayment của bảng hóa đơn 
        public const int ChuaThanhToan = 0;
        public const int DaThanhToan = 1;
    }

    public static class BeanUtils
    {
        public static bool IsNumber(this object value)
        {
            return value is sbyte
                   || value is byte
                   || value is short
                   || value is ushort
                   || value is int
                   || value is uint
                   || value is long
                   || value is ulong
                   || value is float
                   || value is double
                   || value is decimal;
        }
        public static object GetProperty(object obj, string propName)
        {
            if (obj == null || string.IsNullOrEmpty(propName)) return null;
            var propInfo = obj.GetType().GetProperty(propName);
            if (propInfo != null)
            {
                if (!propInfo.CanRead)
                    return null;
            }
            else
            {
                if (obj is JObject)
                {
                    var obj_real = obj as JObject;
                    return obj_real.SelectToken(propName);
                }
            }

            if (propInfo == null) return null;
            var propValue = propInfo.GetValue(obj, null);
            return propValue;
        }

        public static void SetProperty(object obj, string propName, object propValue)
        {
            if (obj == null || string.IsNullOrEmpty(propName)) return;
            var propInfo = obj.GetType().GetProperty(propName);
            if (propInfo == null || !propInfo.CanWrite) return;

            if (propValue == null)
            {
                propInfo.SetValue(obj, propValue);
                return;
            }

            // convert between string and number
            var propInfoType = propInfo.PropertyType;
            if (propInfoType.IsGenericParameter || propInfoType.IsGenericType)
                propInfoType = propInfoType.GetGenericArguments()[0];
            var propValueType = propValue.GetType();
            if (propValueType.IsGenericParameter || propValueType.IsGenericType)
                propValueType = propValueType.GetGenericArguments()[0];
            if (propInfoType != propValueType)
            {
                // string -> int
                if (propValueType == typeof(string) && propInfoType == typeof(int))
                {
                    var propValue_i = 0;
                    if (int.TryParse(propValue.ToString(), out propValue_i))
                    {
                        obj.GetType().GetProperty(propName).SetValue(obj, propValue_i);
                        return;
                    }

                    obj.GetType().GetProperty(propName).SetValue(obj, 0);
                    return;
                }

                // string -> long
                if (propValueType == typeof(string) && propInfoType == typeof(long))
                {
                    long propValue_l = 0;
                    if (long.TryParse(propValue.ToString(), out propValue_l))
                    {
                        obj.GetType().GetProperty(propName).SetValue(obj, propValue_l);
                        return;
                    }

                    obj.GetType().GetProperty(propName).SetValue(obj, 0);
                    return;
                }

                // string -> double
                if (propValueType == typeof(string) && propInfoType == typeof(double))
                {
                    double propValue_d = 0;
                    if (double.TryParse(propValue.ToString(), out propValue_d))
                    {
                        obj.GetType().GetProperty(propName).SetValue(obj, propValue_d);
                        return;
                    }

                    obj.GetType().GetProperty(propName).SetValue(obj, 0);
                    return;
                }

                // int, long, double -> string
                if (IsNumber(propValueType) && propInfo.PropertyType == typeof(string))
                {
                    obj.GetType().GetProperty(propName).SetValue(obj, propValue.ToString());
                    return;
                }

                // decimal? -> int
                if (propValueType == typeof(decimal) && propInfo.PropertyType == typeof(int))
                {
                    obj.GetType().GetProperty(propName).SetValue(obj, Convert.ToInt32(propValue));
                }
            }
            else
            {
                obj.GetType().GetProperty(propName).SetValue(obj, propValue);
            }
        }

        public static void CopyAllPropertySameName(object source, object target)
        {
            // 1. không copy nếu 1 trong 2 đối tượng là null
            if (source == null || target == null) return;

            // 2. lấy danh sách thuộc tính của đối tượng copy  
            var properties = source.GetType().GetProperties();

            // 3. duyệt toàn bộ thuộc tính của đối tượng copy
            foreach (var prop in properties)
            {
                // 4. bỏ qua thuộc tính nếu không được phép đọc
                if (!prop.CanRead) continue;

                // 5. kiểm tra thuộc tính cùng tên (phải cùng kiểu ở đối tượng gán giá trị)
                var propTarget = target.GetType().GetProperty(prop.Name);

                // 6. bỏ qua nếu trường ở target không có trường của source
                if (propTarget == null) continue;

                // 7. bỏ qua nếu thuộc tính đích không được phép ghi
                if (!propTarget.CanWrite) continue;

                // 8. gán giá trị thuộc tính cho đối tượng đích
                var source_value = GetProperty(source, prop.Name);
                SetProperty(target, prop.Name, source_value);
            }
        }
    }
}
