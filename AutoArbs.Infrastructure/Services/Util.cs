using AutoArbs.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoArbs.Infrastructure.Services
{
    public static class Util
    {
        public static bool EmailIsValid(string email)
        {
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        public static string StringHasher(string input)
        {
            return ComputeSha256Hash(input);
        }

        private static string ComputeSha256Hash(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static bool SendEmail(string method, string otp, string email)
        {
            bool output = false;
            var subject = "OTP Requested";
            if (method == "1")
                subject = "Welcome to AutoArbs";
            else if (method == "2")
                subject = "Withdrawal Request";
            else if(method == "3")
                subject = "Password Update Requested";
            else { }

            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("mail@autoarbs.com");
                message.To.Add(new MailAddress(email));
                message.Subject = subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = "Your OTP code is "+otp;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("adeyemidada620@gmail.com", "lmuvyvvimtptpnmu");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                output = true;
            }
            catch (Exception)
            {
                output = false;
            }
            return output;
        }

        public static string GenerateOtp()
        {
            Random r = new Random();
            int randNum = r.Next(1000000);
            string sixDigitNumber = randNum.ToString("D6");
            if (sixDigitNumber.Length < 6)
                GenerateOtp();
            return sixDigitNumber;
        }

        public static decimal UpdateBalance(string method, User user, decimal amount)
        {
            var balance = user.Balance;
            if(method == "deposit")
            {
                balance = balance + amount;
            }
            else  if(method == "withdraw")
            {
                balance = balance - amount;
            }
            else
            {}
            return balance;
        }
    }
}
