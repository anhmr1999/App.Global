using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace App.Global.Commons.Handlers
{
    public class ValidateCommon
    {
        public static bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            var trimmedEmail = email.Trim();
            if (email.EndsWith(".")) { return false; }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        public static bool ValidatePhoneNumber(string phonenumber)
        {
            if (string.IsNullOrEmpty(phonenumber))
                return false;

            return Regex.IsMatch(phonenumber, @"(84|0[3|5|7|8|9])+([0-9]{8})");
        }

        public static string ValidatePassword(string password)
        {
            if (password.Length < 6)
            {
                return "Passwords must be at least 6 characters!";
            }
            if (!(password.Any(char.IsNumber) // chứa số
                && password.Any(char.IsLower) // chứa chữ thường
                && password.Any(char.IsUpper) // chứa chữ hoa
                && Regex.Replace(password, "[^a-zA-Z0-9]", String.Empty) != password)) // chứa ký tự đặc biệt
            {
                return "Password must contain at least 1 number, 1 lowercase letter, 1 uppercase letter and 1 special character!";
            }
            return null;
        }

        public static string Normalization(string input)
        {
            string normalString = new string(input
                .Normalize(NormalizationForm.FormD).ToCharArray()
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray());
            return normalString.Replace(" ", String.Empty);
        }

        public static string ValidateImageFile(IFormFile file)
        {
            var imagesExtensions = new List<string>()
                { ".jpg", ".png", ".svg", ".gif" };
            var extention = Path.GetExtension(file.FileName);
            if (!imagesExtensions.Any(x => x.Contains(extention)))
                return "Uploaded file is incorrect!";
            if (file.Length < GlobalConsts.imgMinSize || file.Length > GlobalConsts.imgMaxSize)
                return "Upload files must be between 5Kb and 3Mb!";
            return string.Empty;
        }

        public static string ValidateExcelFile(IFormFile file)
        {
            var extention = Path.GetExtension(file.FileName);
            if (!extention.Contains("xls"))
                return "Uploaded file is incorrect!";
            if (file.Length < GlobalConsts.imgMinSize || file.Length > GlobalConsts.imgMaxSize)
                return "Upload files must be between 5Kb and 3Mb!";
            return string.Empty;
        }
    }
}
