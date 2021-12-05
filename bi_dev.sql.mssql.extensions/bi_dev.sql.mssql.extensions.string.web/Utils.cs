using HtmlAgilityPack;
using Microsoft.SqlServer.Server;
using Mono.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bi_dev.sql.mssql.extensions.@string.web
{
    public static class Utils
    {
        [SqlFunction]
        public static string GetQueryParamValue(string value, string paramName, bool nullWhenError)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(paramName))
                {
                    return null;
                }
                else
                {
                    string result = HttpUtility.ParseQueryString(new Uri(value).Query).Get(paramName);
                    return result;
                }
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
        public static string GetHtmlElements(string value, string xPath, bool nullWhenError)
        {
            try
            {
                List<string> result = new List<string>();
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(value);
                return JsonConvert.SerializeObject(doc.DocumentNode.SelectNodes(xPath).Select(x => x?.OuterHtml).ToList());
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
        public static string GetHtmlElement(string value, string xPath, bool nullWhenError)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(value);
                return JsonConvert.SerializeObject(doc.DocumentNode.SelectNodes(xPath).Select(x => x?.OuterHtml)?.FirstOrDefault());
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
        public static string GetHtmlElementAttributeValue(string value, string attributeName, bool nullWhenError)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                var node = doc.CreateElement("div");
                node.InnerHtml = value;
                return node.FirstChild?.Attributes[attributeName]?.Value;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
        public static string GetHtmlElementInnerText(string value, bool nullWhenError)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                var node = doc.CreateElement("div");
                node.InnerHtml = value;
                return node.FirstChild?.InnerText;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
        public static string HtmlEncode(string value, bool nullWhenError)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return null;
                }
                else
                {
                    return HttpUtility.HtmlEncode(value);
                }
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
        public static string HtmlDecode(string value, bool nullWhenError)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return null;
                }
                else
                {
                    return HttpUtility.HtmlDecode(value);
                }
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
    }
}
