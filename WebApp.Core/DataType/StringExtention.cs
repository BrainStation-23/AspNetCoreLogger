using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace WebApp.Core.DataType
{
    public static class StringExtention
    {

        #region test check
        public static string TestCheck(this object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), "Null value not allowed");

            return value.ToString();
        }


        #endregion
        public static string ToCustomString(this object value)
        {
            return value.ToString();
        }

        public static bool IsNotNullOrEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value); ;
        }

        public static bool IsNotNullOrWhiteSpace(this string value)
        {
            return !string.IsNullOrWhiteSpace(value); ;
        }

        public static List<string> ToSeperateList(this string value, char delimeter = ',')
        {
            return value.Split(delimeter).Select(e => e.Trim()).ToList();
        }

        public static string ToSentence(this string input)
        {
            return new string(input.ToCharArray().SelectMany((c, i) => i > 0 && char.IsUpper(c) ? new[] { ' ', c } : new[] { c }).ToArray());
        }

        public static string SplitCamelCase(this string input)
        {
            //Regex.Replace(input, "([a-z](?=[A-Z]|[0-9])|[A-Z](?=[A-Z][a-z]|[0-9])|[0-9](?=[^0-9]))", "$1") //([A-Z]+(?=$|[A-Z][a-z]|[0-9])|[A-Z]?[a-z]+|[0-9]+)/g
            //Regex.Replace(input, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1")
            //Regex.Replace(input, "((?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z]))", " $1").Trim();
            //Regex.Replace(input, "(\\B[A-Z])", " $1")
            return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
        }

        public static string AddSpacesToSentence(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);

            for (var i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(text[i]);
            }

            return newText.ToString();
        }

        #region string to number

        /// <summary>
        /// 1,2,3,... natural numbers
        /// 
        /// </summary>
        /// <param name="stringNumber"></param>
        /// <returns></returns>
        public static bool IsNaturalNumber(this string stringNumber)
        {
            var notNaturalPattern = new Regex("[^0-9]");
            var naturalPattern = new Regex("0*[1-9][0-9]*");

            return !notNaturalPattern.IsMatch(stringNumber) && naturalPattern.IsMatch(stringNumber);
        }

        /// <summary>
        /// check Positive Integers with zero inclusive
        /// </summary>
        /// <param name="stringNumber"></param>
        /// <returns></returns>
        public static bool IsWholeNumber(this string stringNumber)
        {
            var notWholePattern = new Regex("[^0-9]");

            return !notWholePattern.IsMatch(stringNumber);
        }

        /// <summary>
        /// check Integers both Positive & Negative
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool IsInteger(this string strNumber)
        {
            var notIntPattern = new Regex("[^0-9-]");
            var intPattern = new Regex("^-[0-9]+$|^[0-9]+$");

            return !notIntPattern.IsMatch(strNumber) && intPattern.IsMatch(strNumber);
        }

        /// <summary>
        /// check Positive Number both Integer & Real
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool IsPositiveNumber(this string strNumber)
        {
            var notPositivePattern = new Regex("[^0-9.]");
            var positivePattern = new Regex("^[.][0-9]+$|[0-9]*[.]*[0-9]+$");
            var twoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");

            return !notPositivePattern.IsMatch(strNumber) &&
                   positivePattern.IsMatch(strNumber) &&
                   !twoDotPattern.IsMatch(strNumber);
        }

        /// <summary>
        /// check valid number
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool IsNumber(this string strNumber)
        {
            var notNumberPattern = new Regex("[^0-9.-]");
            var twoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            var twoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
            var validRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
            var validIntegerPattern = "^([-]|[0-9])[0-9]*$";
            var numberPattern = new Regex("(" + validRealPattern + ")|(" + validIntegerPattern + ")");

            return !notNumberPattern.IsMatch(strNumber) &&
                   !twoDotPattern.IsMatch(strNumber) &&
                   !twoMinusPattern.IsMatch(strNumber) &&
                   numberPattern.IsMatch(strNumber);

        }

        public static bool IsNumber1(this string value)
        {
            return int.TryParse(value, out int id);
        }

        /// <summary>
        /// check Alphabets
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsAlpha(this string str)
        {
            var alphaPattern = new Regex("[^a-zA-Z]");

            return !alphaPattern.IsMatch(str);
        }

        /// <summary>
        ///  Check Alpha & Numeric
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsAlphaNumeric(this string str)
        {
            var alphaNumericPattern = new Regex("[^a-zA-Z0-9]");

            return !alphaNumericPattern.IsMatch(str);
        }
        #endregion

        public static string StandardizeUrl(string url)
        {
            url = url.ToLower();

            if (!url.StartsWith("http://"))
            {
                url = string.Concat("http://", url);
            }

            return url;
        }

        public static Guid ToGuid(this string value)
        {
            Guid result = Guid.Empty;
            Guid.TryParse(value, out result);

            return result;
        }

        //http://www.c-sharpcorner.com/uploadfile/puranindia/regular-expressions-in-C-Sharp/
        //https://stackoverflow.com/questions/272633/add-spaces-before-capital-letters

        #region FormatWith
        /// <summary>
		/// Formats a string with one literal placeholder.
		/// </summary>
		/// <param name="text">The extension text</param>
		/// <param name="arg0">Argument 0</param>
		/// <returns>The formatted string</returns>
        public static string FormatWith(this string text, object arg0)
        {
            return string.Format(text, arg0);
        }

        /// <summary>
        /// Formats a string with two literal placeholders.
        /// </summary>
        /// <param name="text">The extension text</param>
        /// <param name="arg0">Argument 0</param>
        /// <param name="arg1">Argument 1</param>
        /// <returns>The formatted string</returns>
        public static string FormatWith(this string text, object arg0, object arg1)
        {
            return string.Format(text, arg0, arg1);
        }

        /// <summary>
        /// Formats a string with tree literal placeholders.
        /// </summary>
        /// <param name="text">The extension text</param>
        /// <param name="arg0">Argument 0</param>
        /// <param name="arg1">Argument 1</param>
        /// <param name="arg2">Argument 2</param>
        /// <returns>The formatted string</returns>
        public static string FormatWith(this string text, object arg0, object arg1, object arg2)
        {
            return string.Format(text, arg0, arg1, arg2);
        }

        /// <summary>
        /// Formats a string with a list of literal placeholders.
        /// </summary>
        /// <param name="text">The extension text</param>
        /// <param name="args">The argument list</param>
        /// <returns>The formatted string</returns>
        public static string FormatWith(this string text, params object[] args)
        {
            return string.Format(text, args);
        }

        /// <summary>
        /// Formats a string with a list of literal placeholders.
        /// </summary>
        /// <param name="text">The extension text</param>
        /// <param name="provider">The format provider</param>
        /// <param name="args">The argument list</param>
        /// <returns>The formatted string</returns>
        public static string FormatWith(this string text, IFormatProvider provider, params object[] args)
        {
            return string.Format(provider, text, args);
        }
        #endregion

        #region XmlSerialize XmlDeserialize
        /// <summary>Serialises an object of type T in to an xml string</summary>
		/// <typeparam name="T">Any class type</typeparam>
		/// <param name="objectToSerialise">Object to serialise</param>
		/// <returns>A string that represents Xml, empty oterwise</returns>
		public static string XmlSerialize<T>(this T objectToSerialise) where T : class
        {
            var serialiser = new XmlSerializer(typeof(T));
            string xml;
            using (var memStream = new MemoryStream())
            {
                using (var xmlWriter = new XmlTextWriter(memStream, Encoding.UTF8))
                {
                    serialiser.Serialize(xmlWriter, objectToSerialise);
                    xml = Encoding.UTF8.GetString(memStream.GetBuffer());
                }
            }

            // ascii 60 = '<' and ascii 62 = '>'
            xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
            xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));
            return xml;
        }

        /// <summary>Deserialises an xml string in to an object of Type T</summary>
        /// <typeparam name="T">Any class type</typeparam>
        /// <param name="xml">Xml as string to deserialise from</param>
        /// <returns>A new object of type T is successful, null if failed</returns>
        public static T XmlDeserialize<T>(this string xml, string root = null) where T : class
        {
            XmlRootAttribute attribute = new XmlRootAttribute();
            if (!string.IsNullOrEmpty(root))
                attribute = new XmlRootAttribute(root);

            var serialiser = new XmlSerializer(typeof(T), attribute);
            T newObject;

            using (var stringReader = new StringReader(xml))
            {
                using (var xmlReader = new XmlTextReader(stringReader))
                {
                    try
                    {
                        newObject = serialiser.Deserialize(xmlReader) as T;
                    }
                    catch (InvalidOperationException) // String passed is not Xml, return null
                    {
                        return null;
                    }

                }
            }

            return newObject;
        }
        #endregion

        #region To X conversions

        //public static bool ToBool(this string value)
        //{
        //    if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
        //        return false;
        //    if (value.ToLower() == "true")
        //        return true;

        //    return false;
        //}

        /// <summary>
        /// Parses a string into an Enum
        /// </summary>
        /// <typeparam name="T">The type of the Enum</typeparam>
        /// <param name="value">String value to parse</param>
        /// <returns>The Enum corresponding to the stringExtensions</returns>
        public static T ToEnum<T>(this string value)
        {
            return ToEnum<T>(value, false);
        }

        /// <summary>
        /// Parses a string into an Enum
        /// </summary>
        /// <typeparam name="T">The type of the Enum</typeparam>
        /// <param name="value">String value to parse</param>
        /// <param name="ignorecase">Ignore the case of the string being parsed</param>
        /// <returns>The Enum corresponding to the stringExtensions</returns>
        public static T ToEnum<T>(this string value, bool ignorecase)
        {
            if (value == null)
                throw new ArgumentNullException("Value");

            value = value.Trim();

            if (value.Length == 0)
                throw new ArgumentNullException("Must specify valid information for parsing in the string.", "value");

            Type t = typeof(T);
            if (!t.IsEnum)
                throw new ArgumentException("Type provided must be an Enum.", "T");

            return (T)Enum.Parse(t, value, ignorecase);
        }

        /// <summary>
        /// Toes the integer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static int ToInteger(this string value, int defaultvalue)
        {
            return (int)ToDouble(value, defaultvalue);
        }

        /// <summary>
        /// Toes the integer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int ToInteger(this string value)
        {
            return ToInteger(value, 0);
        }

        ///// <summary>
        ///// Toes the U long.
        ///// </summary>
        ///// <param name="value">The value.</param>
        ///// <returns></returns>
        //public static ulong ToULong(this string value)
        //{
        //    ulong def = 0;
        //    return value.ToULong(def);
        //}
        ///// <summary>
        ///// Toes the U long.
        ///// </summary>
        ///// <param name="value">The value.</param>
        ///// <param name="defaultvalue">The defaultvalue.</param>
        ///// <returns></returns>
        //public static ulong ToULong(this string value, ulong defaultvalue)
        //{
        //    return (ulong)ToDouble(value, defaultvalue);
        //}

        /// <summary>
        /// Toes the double.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static double ToDouble(this string value, double defaultvalue)
        {
            double result;
            if (double.TryParse(value, out result))
            {
                return result;
            }
            else return defaultvalue;
        }

        /// <summary>
        /// Toes the double.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static double ToDouble(this string value)
        {
            return ToDouble(value, 0);
        }

        /// <summary>
        /// Toes the date time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string value, DateTime? defaultvalue)
        {
            DateTime result;
            if (DateTime.TryParse(value, out result))
            {
                return result;
            }
            else return defaultvalue;
        }

        /// <summary>
        /// Toes the date time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string value)
        {
            return ToDateTime(value, null);
        }

        /// <summary>
        /// Converts a string value to bool value, supports "T" and "F" conversions.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <returns>A bool based on the string value</returns>
        public static bool? ToBoolean(this string value)
        {
            if (string.Compare("T", value, true) == 0)
            {
                return true;
            }
            if (string.Compare("F", value, true) == 0)
            {
                return false;
            }
            bool result;
            if (bool.TryParse(value, out result))
            {
                return result;
            }
            else return null;
        }
        #endregion

        #region ValueOrDefault
        public static string GetValueOrEmpty(this string value)
        {
            return GetValueOrDefault(value, string.Empty);
        }

        public static string GetValueOrDefault(this string value, string defaultvalue)
        {
            if (value != null) return value;
            return defaultvalue;
        }
        #endregion

        #region ToUpperLowerNameVariant
        /// <summary>
        /// Converts string to a Name-Format where each first letter is Uppercase.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <returns></returns>
        public static string ToUpperLowerNameVariant(this string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            char[] valuearray = value.ToLower().ToCharArray();
            bool nextupper = true;
            for (int i = 0; i < (valuearray.Count() - 1); i++)
            {
                if (nextupper)
                {
                    valuearray[i] = char.Parse(valuearray[i].ToString().ToUpper());
                    nextupper = false;
                }
                else
                {
                    switch (valuearray[i])
                    {
                        case ' ':
                        case '-':
                        case '.':
                        case ':':
                        case '\n':
                            nextupper = true;
                            break;
                        default:
                            nextupper = false;
                            break;
                    }
                }
            }
            return new string(valuearray);
        }
        #endregion

        #region Encrypt Decrypt
        /// <summary>
        /// Encryptes a string using the supplied key. Encoding is done using RSA encryption.
        /// </summary>
        /// <param name="stringToEncrypt">String that must be encrypted.</param>
        /// <param name="key">Encryptionkey.</param>
        /// <returns>A string representing a byte array separated by a minus sign.</returns>
        /// <exception cref="ArgumentException">Occurs when stringToEncrypt or key is null or empty.</exception>
        public static string Encrypt(this string stringToEncrypt, string key)
        {
            if (string.IsNullOrEmpty(stringToEncrypt))
            {
                throw new ArgumentException("An empty string value cannot be encrypted.");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Cannot encrypt using an empty key. Please supply an encryption key.");
            }

            System.Security.Cryptography.CspParameters cspp = new System.Security.Cryptography.CspParameters();
            cspp.KeyContainerName = key;

            System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider(cspp);
            rsa.PersistKeyInCsp = true;

            byte[] bytes = rsa.Encrypt(System.Text.UTF8Encoding.UTF8.GetBytes(stringToEncrypt), true);

            return BitConverter.ToString(bytes);
        }

        /// <summary>
        /// Decryptes a string using the supplied key. Decoding is done using RSA encryption.
        /// </summary>
        /// <param name="key">Decryptionkey.</param>
        /// <returns>The decrypted string or null if decryption failed.</returns>
        /// <exception cref="ArgumentException">Occurs when stringToDecrypt or key is null or empty.</exception>
        public static string Decrypt(this string stringToDecrypt, string key)
        {
            string result = null;

            if (string.IsNullOrEmpty(stringToDecrypt))
            {
                throw new ArgumentException("An empty string value cannot be encrypted.");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Cannot decrypt using an empty key. Please supply a decryption key.");
            }

            try
            {
                System.Security.Cryptography.CspParameters cspp = new System.Security.Cryptography.CspParameters();
                cspp.KeyContainerName = key;

                System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider(cspp);
                rsa.PersistKeyInCsp = true;

                string[] decryptArray = stringToDecrypt.Split(new string[] { "-" }, StringSplitOptions.None);
                byte[] decryptByteArray = Array.ConvertAll<string, byte>(decryptArray, (s => Convert.ToByte(byte.Parse(s, System.Globalization.NumberStyles.HexNumber))));


                byte[] bytes = rsa.Decrypt(decryptByteArray, true);

                result = System.Text.UTF8Encoding.UTF8.GetString(bytes);

            }
            finally
            {
                // no need for further processing
            }

            return result;
        }
        #endregion

        /// <summary>
        /// Send an email using the supplied string.
        /// </summary>
        /// <param name="body">String that will be used i the body of the email.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="sender">The email address from which the message was sent.</param>
        /// <param name="recipient">The receiver of the email.</param> 
        /// <param name="server">The server from which the email will be sent.</param>  
        /// <returns>A boolean value indicating the success of the email send.</returns>
        public static bool SendEmail(this string body, string subject, string sender, string recipient, string server)
        {
            try
            {
                // To
                MailMessage mailMsg = new MailMessage();
                mailMsg.To.Add(recipient);

                // From
                MailAddress mailAddress = new MailAddress(sender);
                mailMsg.From = mailAddress;

                // Subject and Body
                mailMsg.Subject = subject;
                mailMsg.Body = body;

                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient(server);
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential();
                smtpClient.Credentials = credentials;

                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not send mail from: " + sender + " to: " + recipient + " thru smtp server: " + server + "\n\n" + ex.Message, ex);
            }

            return true;
        }

        #region Truncate
        /// <summary>
        /// Truncates the string to a specified length and replace the truncated to a ...
        /// </summary>
        /// <param name="maxLength">total length of characters to maintain before the truncate happens</param>
        /// <returns>truncated string</returns>
        public static string Truncate(this string text, int maxLength)
        {
            // replaces the truncated string to a ...
            const string suffix = "...";
            string truncatedString = text;

            if (maxLength <= 0) return truncatedString;
            int strLength = maxLength - suffix.Length;

            if (strLength <= 0) return truncatedString;

            if (text == null || text.Length <= maxLength) return truncatedString;

            truncatedString = text.Substring(0, strLength);
            truncatedString = truncatedString.TrimEnd();
            truncatedString += suffix;
            return truncatedString;
        }
        #endregion

        #region HTMLHelper
        /// <summary>
        /// Converts to a HTML-encoded string
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string HtmlEncode(this string data)
        {
            return System.Web.HttpUtility.HtmlEncode(data);
        }

        /// <summary>
        /// Converts the HTML-encoded string into a decoded string
        /// </summary>
        public static string HtmlDecode(this string data)
        {
            return System.Web.HttpUtility.HtmlDecode(data);
        }

        /// <summary>
        /// Parses a query string into a System.Collections.Specialized.NameValueCollection
        /// using System.Text.Encoding.UTF8 encoding.
        /// </summary>
        public static System.Collections.Specialized.NameValueCollection ParseQueryString(this string query)
        {
            return System.Web.HttpUtility.ParseQueryString(query);
        }

        /// <summary>
        /// Encode an Url string
        /// </summary>
        public static string UrlEncode(this string url)
        {
            return System.Web.HttpUtility.UrlEncode(url);
        }

        /// <summary>
        /// Converts a string that has been encoded for transmission in a URL into a
        /// decoded string.
        /// </summary>
        public static string UrlDecode(this string url)
        {
            return System.Web.HttpUtility.UrlDecode(url);
        }

        /// <summary>
        /// Encodes the path portion of a URL string for reliable HTTP transmission from
        /// the Web server to a client.
        /// </summary>
        public static string UrlPathEncode(this string url)
        {
            return System.Web.HttpUtility.UrlPathEncode(url);
        }
        #endregion

        #region Format
        /// <summary>
        /// Replaces the format item in a specified System.String with the text equivalent
        /// of the value of a specified System.Object instance.
        /// </summary>
        /// <param name="arg">The arg.</param>
        /// <param name="additionalArgs">The additional args.</param>
        public static string Format(this string format, object arg, params object[] additionalArgs)
        {
            if (additionalArgs == null || additionalArgs.Length == 0)
            {
                return string.Format(format, arg);
            }
            else
            {
                return string.Format(format, new object[] { arg }.Concat(additionalArgs).ToArray());
            }
        }
        #endregion

        //#region IsNullOrEmpty
        ///// <summary>
        ///// Determines whether [is not null or empty] [the specified input].
        ///// </summary>
        ///// <returns>
        ///// 	<c>true</c> if [is not null or empty] [the specified input]; otherwise, <c>false</c>.
        ///// </returns>
        //public static bool IsNotNullOrEmpty(this string input)
        //{
        //    return !String.IsNullOrEmpty(input);
        //}
        //#endregion

        /// <summary>
        /// Seperate camel case string to word 
        /// </summary>
        /// <param name="camelCaseWord"></param>
        /// <returns>return seperate word "camelCaseWord" to "camel Case Word"</returns>
        public static string Wordify(this string camelCaseWord)
        {
            // if the word is all upper, just return it
            if (!Regex.IsMatch(camelCaseWord, "[a-z]"))
                return camelCaseWord;

            return string.Join(" ", Regex.Split(camelCaseWord, @"(?<!^)(?=[A-Z])"));
        }

        /// <summary>
        /// Reverse a String
        /// </summary>
        /// <param name="input">The string to Reverse</param>
        /// <returns>The reversed String</returns>
        public static string Reverse(this string input)
        {
            char[] array = input.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }

        public static string Shorten(this string str, int length, string shortenPostfix = " ...")
        {
            if (string.IsNullOrEmpty(str) || str.Length <= length)
                return str;

            return str.Remove(length) + shortenPostfix;
        }

        #region Nullable, Empty & Whitespace

        /// <summary>
        /// This is simply a shorthand method "!string.IsNullOrEmpty(value)"
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>true is the value is not null or it's length != 0</returns>
        public static bool IsNotNullOrEmpty1(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Inverse of <see cref="IsNotNullOrEmpty"/> method
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return !value.IsNotNullOrEmpty();
        }

        /// <summary>
        /// This is simply a shorthand method "!string.IsNullOrEmpty(value)"
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>true is the value is not null or it's length != 0</returns>
        public static bool IsNotNullOrWhiteSpace1(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Inverse of <see cref="IsNotNullOrWhiteSpace"/> method
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return !value.IsNotNullOrWhiteSpace();
        }

        #endregion

        #region Lengths

        /// <summary>
        /// Checks if this string is between the min and max values (inclusive)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min">Inclusive min length</param>
        /// <param name="max">Inclusive max length</param>
        /// <returns></returns>
        public static bool IsBetweenLength(this string value, int min, int max)
        {
            if (value.IsNull() && min == 0)
            {
                return true; // if it's null it has length 0
            }
            else if (value.IsNull())
            {
                return false;
            }
            else
            {
                return value.Length >= min && value.Length <= max;
            }
        }

        /// <summary>
        /// Checks if the string is at least max characters
        /// </summary>
        /// <param name="value"></param>
        /// <param name="max">Inclusive max length</param>
        /// <returns></returns>
        public static bool IsMaxLength(this string value, int max)
        {
            if (value.IsNull())
            {
                return true; // if it's null it has length 0 and that has to be less than max
            }
            else
            {
                return value.Length <= max;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <returns></returns>
        public static bool IsMinLength(this string value, int min)
        {
            if (value.IsNull() && min == 0)
            {
                return true; // if it's null it has length 0
            }
            else if (value.IsNull())
            {
                return false;
            }
            else
            {
                return value.Length >= min;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static bool IsExactLength(this string value, int length)
        {
            return value.IsBetweenLength(length, length);
        }

        #endregion

        #region Misc
        /// <summary>
        /// Check if the current value is a valid email address. It uses the following regular expression
        /// ^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-||_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+([a-z]+|\d|-|\.{0,1}|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])?([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$
        /// null values will fail.
        /// Empty strings will fail.
        /// It performs the check from method <see cref="IsNotNullOrEmpty"/>
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns>True if the value is a valid email address</returns>
        public static bool IsEmail3(this string value)
        {
            if (value.IsNullOrEmpty())
            {
                return false; // if it's null it cannot possibly be an email
            }
            else
            {
                string exp = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-||_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+([a-z]+|\d|-|\.{0,1}|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])?([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$";

                return new Regex(exp, RegexOptions.IgnoreCase).IsMatch(value);
            }
        }

        /// <summary>
        /// Checks if the current value is a password. The password must be at least 8 characters, at least 1 uppercase character, at least 1 lowercase character, at least one number and a maximum of 30 characters.
        /// It uses the following regular expression
        /// ^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsPassword(this string value)
        {
            if (value.IsNullOrEmpty())
            {
                return false; // if it's null it cannot possibly be a password
            }
            else
            {
                string exp = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,30}$";

                return new Regex(exp, RegexOptions.IgnoreCase).IsMatch(value);
            }
        }

        /// <summary>
        /// Validates if the specified object passes the regular expression provided. If the object is null, it will fail. The method calls ToString on the object to get the string value.
        /// </summary>
        /// <param name="value">The value to be evaluated</param>
        /// <param name="exp">The regular expression</param>
        /// <returns></returns>
        public static bool IsRegex(this string value, string exp)
        {
            if (value.IsNotNullOrEmpty())
            {
                return false;
            }

            string check = value.ToString();

            return new Regex(exp, RegexOptions.IgnoreCase).IsMatch(check);
        }

        /// <summary>
        /// Determines if a string is a valid credit card number
        /// Taken from https://github.com/JeremySkinner/FluentValidation/blob/master/src/FluentValidation/Validators/CreditCardValidator.cs
        /// Uses code from: http://www.beachnet.com/~hstiles/cardtype.html
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsCreditCard(this string value)
        {
            if (value.IsNotNullOrEmpty())
            {
                value = value.Replace("-", "").Replace(" ", "");

                int checksum = 0;
                bool evenDigit = false;

                foreach (char digit in value.ToCharArray().Reverse())
                {
                    if (!char.IsDigit(digit))
                    {
                        return false;
                    }

                    int digitValue = (digit - '0') * (evenDigit ? 2 : 1);
                    evenDigit = !evenDigit;

                    while (digitValue > 0)
                    {
                        checksum += digitValue % 10;
                        digitValue /= 10;
                    }
                }

                return (checksum % 10) == 0;
            }
            else
            {
                return false; // a null or empty string cannot be a valid credit card
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static bool IsEqualTo(this string value, string compare)
        {
            if (value.IsNull() && compare.IsNull())
            {
                return true;
            }
            if (value.IsNull() || compare.IsNull())
            {
                return false;
            }
            return String.Equals(value, compare, StringComparison.Ordinal);
        }
        #endregion

        #region Object

        /// <summary>
        /// Check if the current object is not equal to null
        /// </summary>
        /// <param name="value">The object to check</param>
        /// <returns>true is the value is not null</returns>
        public static bool IsNotNull(this object value)
        {
            return (value != null);
        }

        /// <summary>
        /// Inverse of <see cref="IsNotNull"/> method
        /// </summary>
        /// <param name="value">The object to check</param>
        /// <returns>true is the value is null</returns>
        public static bool IsNull(this object value)
        {
            return !value.IsNotNull();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static bool Is(this object value, Func<bool> func)
        {
            return func();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static bool IsNot(this object value, Func<bool> func)
        {
            return !func();
        }

        #endregion

        #region Integer

        public static bool IsNotZero(this int value)
        {
            return (value != 0);
        }

        public static bool Is(this int value, int compare)
        {
            return (value == compare);
        }

        public static bool IsGreaterThan(this int value, int min)
        {
            return (value >= min);
        }

        public static bool IsLessThan(this int value, int max)
        {
            return (value <= max);
        }

        public static bool IsBetween(this int value, int min, int max)
        {
            return (value <= max && value >= min);
        }

        #endregion

        #region DateTime
        public static bool IsDate(this object value)
        {
            return value.IsDate(CultureInfo.InvariantCulture);
        }

        public static bool IsDate(this object value, CultureInfo info)
        {
            return value.IsDate(CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        public static bool IsDate(this object value, CultureInfo info, DateTimeStyles styles)
        {
            if (value.IsNotNull())
            {
                DateTime result;

                if (DateTime.TryParse(value.ToString(), info, styles, out result))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false; // if it's null it cannot be a date
            }
        }
        #endregion

        #region Date Comparisons

        public static bool IsGreaterThan(this DateTime value, DateTime compare)
        {
            return value > compare;
        }

        public static bool IsGreaterThanOrEqualTo(this DateTime value, DateTime compare)
        {
            return value >= compare;
        }

        public static bool IsLessThan(this DateTime value, DateTime compare)
        {
            return value < compare;
        }

        public static bool IsLessThanOrEqualTo(this DateTime value, DateTime compare)
        {
            return value <= compare;
        }

        public static bool IsEqualTo(this DateTime value, DateTime compare)
        {
            return value == compare;
        }

        public static bool IsBetweenInclusive(this DateTime value, DateTime from, DateTime to)
        {
            return value >= from && value <= to;
        }

        public static bool IsBetweenExclusive(this DateTime value, DateTime from, DateTime to)
        {
            return value > from && value < to;
        }

        #endregion

        #region Helpers

        public static string EmptyStringIfNull(this string value)
        {
            if (value.IsNull())
            {
                return string.Empty;
            }
            else
            {
                return value;
            }
        }

        #endregion

        #region GetName

        // Code taken from http://joelabrahamsson.com/getting-property-and-method-names-using-static-reflection-in-c/

        public static string GetName<T>(this T instance, Expression<Func<T, object>> expression)
        {
            return GetName(expression);
        }

        public static string GetName<T>(Expression<Func<T, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(
                    "The expression cannot be null.");
            }

            return GetName(expression.Body);
        }

        public static string GetName<T>(this T instance, Expression<Action<T>> expression)
        {
            return GetName(expression);
        }

        public static string GetName<T>(Expression<Action<T>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(
                    "The expression cannot be null.");
            }

            return GetName(expression.Body);
        }

        private static string GetName(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(
                    "The expression cannot be null.");
            }

            if (expression is MemberExpression)
            {
                // Reference type property or field
                var memberExpression =
                    (MemberExpression)expression;
                return memberExpression.Member.Name;
            }

            if (expression is MethodCallExpression)
            {
                // Reference type method
                var methodCallExpression =
                    (MethodCallExpression)expression;
                return methodCallExpression.Method.Name;
            }

            if (expression is UnaryExpression)
            {
                // Property, field of method returning value type
                var unaryExpression = (UnaryExpression)expression;
                return GetName(unaryExpression);
            }

            throw new ArgumentException("Invalid expression");
        }

        private static string GetName(UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operand is MethodCallExpression)
            {
                var methodExpression =
                    (MethodCallExpression)unaryExpression.Operand;
                return methodExpression.Method.Name;
            }

            return ((MemberExpression)unaryExpression.Operand)
                .Member.Name;
        }

        #endregion

        #region Type Test

        public static bool Is<T>(this object value)
        {
            if (value == null)
            {
                return false;
            }

            var converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));

            try
            {
                T result = (T)converter.ConvertFromString(value.ToString());
                return result != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsInt(this object value)
        {
            return value.Is<int>();
        }

        public static bool IsShort(this object value)
        {
            return value.Is<short>();
        }

        public static bool IsLong(this object value)
        {
            return value.Is<long>();
        }

        public static bool IsDouble(this object value)
        {
            return value.Is<Double>();
        }

        public static bool IsDecimal(this object value)
        {
            return value.Is<Decimal>();
        }

        public static bool IsBool(this object value)
        {
            return value.Is<bool>();
        }

        public static bool IsNumber(this object value)
        {
            return
                value.IsLong() ||
                value.IsDouble() ||
                value.IsDecimal() ||
                value.IsDouble();
        }

        #endregion

        #region To

        public static T To<T>(this object value)
        {
            return value.To<T>(default(T));
        }

        public static T To<T>(this object value, T fallback)
        {
            if (value == null)
            {
                return fallback;
            }

            var converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
            if (converter != null)
            {
                try
                {
                    return (T)converter.ConvertFromString(value.ToString());
                }
                catch (Exception)
                {
                    return fallback;
                }

            }
            return fallback;
        }

        public static int ToInt(this object value, int fallback = default(int))
        {
            return value.To<int>(fallback);
        }

        public static short ToShort(this object value, short fallback = default(short))
        {
            return value.To<short>(fallback);
        }

        public static long ToLong(this object value, long fallback = default(long))
        {
            return value.To<long>(fallback);
        }

        public static double ToDouble(this object value, double fallback = default(double))
        {
            return value.To<double>(fallback);
        }

        public static decimal ToDecimal(this object value, decimal fallback = default(decimal))
        {
            return value.To<decimal>(fallback);
        }

        public static bool ToBool(this object value, bool fallback = default(bool))
        {
            return value.To<bool>(fallback);
        }

        #endregion
    }
}
