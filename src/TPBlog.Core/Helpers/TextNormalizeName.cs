using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TPBlog.Core.Helpers
{
    public static class TextNormalizedName
    {
        public static string ToTextNormalizedString(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // Loại bỏ khoảng trắng thừa ở đầu và cuối
            input = input.Trim();

            // Loại bỏ dấu và các ký tự đặc biệt
            var regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            var normalizedString = input.Normalize(NormalizationForm.FormD);
            var result = regex.Replace(normalizedString, string.Empty);

            // Thay thế các ký tự đặc biệt (ví dụ: đ thành d, Đ thành D)
            result = result.Replace('đ', 'd').Replace('Đ', 'D');

            // Loại bỏ các ký tự không hợp lệ và thay thế bằng dấu gạch ngang
            result = Regex.Replace(result, @"[^\w\s]", "-");

            // Thay thế khoảng trắng thành dấu gạch ngang
            result = result.Replace(" ", "-");

            // Loại bỏ các dấu gạch ngang liên tiếp
            while (result.Contains("--"))
            {
                result = result.Replace("--", "-");
            }

            // Loại bỏ dấu gạch ngang ở đầu và cuối chuỗi
            result = result.Trim('-');

            // Chuyển tất cả chữ cái thành chữ thường để so sánh không phân biệt hoa/thường
            return result.ToLower();
        }
    }
}