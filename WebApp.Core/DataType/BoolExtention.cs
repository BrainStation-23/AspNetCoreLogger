//using Microsoft.AspNetCore.Html;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Web;
//using System.Xml;

//namespace WebApp.Core.DataType
//{
//    public static class BoolExtention
//    {
//        public static HtmlString ToIcon(this bool value, bool enableColor = true)
//        {
//            //return value ? "<span class='text-success'><i class='fa fa-check'></i></span>" : "<span class='text-danger'><i class='fa fa-remove'></i></span>";

//            var control = new StringBuilder();
//            using (var stringWriter = new StringWriter())
//            {
//                using (var htmlWriter = new XmlTextWriter(stringWriter))
//                {
//                    var span = new HtmlGenericControl("span");
//                    if (enableColor)
//                    {
//                        var color = value ? "text-success" : "text-danger";
//                        span.Attributes.Add("class", color);
//                    }

//                    var italic = new HtmlGenericControl("i");
//                    var icon = value ? "fa fa-check" : "fa fa-remove";
//                    italic.Attributes.Add("class", icon);

//                    span.Controls.Add(italic);

//                    span.RenderControl(htmlWriter);
//                    control.Append(stringWriter);
//                    span.Dispose();
//                }
//            }

//            return new HtmlString(control.ToString());
//        }

//        public static bool ToBool(this string value)
//        {
//            var b = false;
//            try
//            {
//                bool.TryParse(value, out b);
//            }
//            catch (Exception)
//            {
//                // ignored
//            }

//            return b;
//        }

//        public static bool ToBoolParse(this string value)
//        {
//            switch (value.Trim().ToUpper())
//            {
//                case "TRUE":
//                case "YES":
//                case "OK":
//                case "1":
//                case "-1":
//                    return true;
//                default:
//                    return false;
//            }
//        }
//    }
//}