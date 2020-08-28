using HtmlAgilityPack;
using Microsoft.SqlServer.Server;
using Mono.Web;
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
        public static string GetStringExceptHTML(string value, bool nullWhenError)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return null;
                }
                else
                {
                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(value);
                    return htmlDoc.DocumentNode.InnerText;
                }
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
        public static ICollection<string> GetHtmlElements(string value, string xPath, bool nullWhenError)
        {
            try
            {
                List<string> result = new List<string>();
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(value);
                return doc.DocumentNode.SelectNodes(xPath).Select(x => x.OuterHtml).ToList();
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<List<string>>(e, nullWhenError);
            }
        }
        public static string GetHtmlElementAttributeValue(string value, string attributeName, bool nullWhenError)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                var node = new HtmlNode(HtmlNodeType.Element, doc, 0);
                node.Name = "div";
                node.InnerHtml = value;
                return node.Attributes[attributeName]?.Value;
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }

        }
    }
}
