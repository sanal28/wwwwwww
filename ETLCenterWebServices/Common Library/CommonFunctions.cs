using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ETLCenterWebServices.Common_Library
{
    public class CommonFunctions
    {
        public bool SendEmail(string ToAddress, string CcAddress, string BccAddress, string Subject, string MessageBody)
        {
            MailAddress email;
            string[] AddressArray;
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("jibin@nuvento.com");
                if (ToAddress != string.Empty)
                {
                    AddressArray = ToAddress.Split(',');
                    foreach (string item in AddressArray)
                    {
                        email = new MailAddress(item);
                        if (!mail.To.Contains(email))
                            mail.To.Add(item);
                    }
                }
                if (CcAddress != string.Empty)
                {
                    AddressArray = CcAddress.Split(',');
                    foreach (string item in AddressArray)
                    {
                        email = new MailAddress(item);
                        if (!mail.CC.Contains(email))
                            mail.CC.Add(item);
                    }
                }
                if (BccAddress != string.Empty)
                {
                    AddressArray = BccAddress.Split(',');
                    foreach (string item in AddressArray)
                    {
                        email = new MailAddress(item);
                        if (!mail.Bcc.Contains(email))
                            mail.Bcc.Add(item);
                    }
                }
                mail.Subject = Subject;
                mail.Body = MessageBody + "<br/><br/>Thanks,<br/>ETL Support";
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("jibin@nuvento.com", "Nuvmail368");
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                AddressArray = null;
                email = null;
            }
        }


        public void LogError(HttpContext Context, string ErrorMessage, string MethodName, string PageName)
        {
            try
            {
                string FilePath = Context.Server.MapPath("~/Logs/ErrorLog.txt");
                if (!File.Exists(FilePath))
                {
                    using (FileStream fs = new FileStream(FilePath, FileMode.Create))
                    {
                        //File Creation goes here
                    }
                }
                using (StreamWriter writer = new StreamWriter(FilePath, true))
                {
                    writer.WriteLine("Error Message : " + ErrorMessage);
                    writer.WriteLine("Method Name : " + MethodName);
                    writer.WriteLine("Page Name : " + PageName);
                    writer.WriteLine("Date Time : " + DateTime.Now.ToString());
                    writer.WriteLine("---------------------------------------------------------------------------------");
                }
            }
            catch(Exception)
            {

            }
        }
    }
    public class Crypto
    {

        public static string Encryption(string clearText)
        {
            MemoryStream ms = null;
            System.Security.Cryptography.CryptoStream cs = null;
            string EncryptionKey = "ETLCenter";
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            try
            {
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    encryptor.Key = pdb.GetBytes(32);

                    encryptor.IV = pdb.GetBytes(16);

                    using (ms = new MemoryStream())

                    {

                        using (cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))

                        {

                            cs.Write(clearBytes, 0, clearBytes.Length);

                            cs.Close();

                        }

                        clearText = Convert.ToBase64String(ms.ToArray());

                    }

                }

                return clearText;

            }

            catch (Exception e)
            {
              //  ExceptionLog.Log(e, "no ip : login model error encryption \n ");
            }
            finally
            {
                pdb.Dispose();
                cs.Dispose();
                ms.Dispose();
            }
            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            MemoryStream ms = null;
            CryptoStream cs = null;
            string EncryptionKey = "ETLCenter";
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

            try

            {



                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                using (Aes encryptor = Aes.Create())

                {


                    encryptor.Key = pdb.GetBytes(32);

                    encryptor.IV = pdb.GetBytes(16);

                    using (ms = new MemoryStream())

                    {

                        using (cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))

                        {

                            cs.Write(cipherBytes, 0, cipherBytes.Length);

                            cs.Close();

                        }

                        cipherText = Encoding.Unicode.GetString(ms.ToArray());

                    }

                }

                return cipherText;

            }

            catch (Exception e)
            {
                //  ExceptionLog.Log(e, "no ip :login model error decryption ");
                return "0";
            }
            finally
            {
                pdb.Dispose();
                ms.Dispose();
                cs.Dispose();

            }
            return cipherText;
        }





    }
}