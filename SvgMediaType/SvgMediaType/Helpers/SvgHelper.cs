using System;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using SvgMediaType.Configuration;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace SvgMediaType.Helpers
{
    public static class SvgHelper
    {
        public static HtmlString RenderSvg(this HtmlHelper helper, IPublishedContent publishedContentSvg, string title = null)
        {
            var isSvgMediaType = publishedContentSvg.ContentType.Alias == SvgMediaTypeConfig.SvgMediaTypeAlias;
            if (!isSvgMediaType)
            {
                LogError($"Node with Id '{publishedContentSvg.Id}' is not of type '{SvgMediaTypeConfig.SvgMediaTypeAlias}'.");
                return new HtmlString(string.Empty);
            }

            var svgContent = publishedContentSvg.Value<string>(SvgMediaTypeConfig.SvgMediaTypeContentPropertyAlias);
            if (string.IsNullOrWhiteSpace(svgContent))
            {
                LogError($"The property with alias '{SvgMediaTypeConfig.SvgMediaTypeContentPropertyAlias}' on node with Id '{publishedContentSvg.Id}' is empty.");
                return new HtmlString(string.Empty);
            }

            if (string.IsNullOrWhiteSpace(title))
                return new HtmlString(svgContent);

            var xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml(svgContent);

                if (xmlDoc.DocumentElement == null || xmlDoc.SelectSingleNode("/title") != null)
                    return new HtmlString(svgContent);

                var titleElement = xmlDoc.CreateElement("title", xmlDoc.DocumentElement.NamespaceURI);
                titleElement.InnerText = title;

                xmlDoc.DocumentElement.PrependChild(titleElement);

                return new HtmlString(xmlDoc.OuterXml);
            }
            catch (Exception e)
            {
                LogError(e, "Error rendering SVG file");

                return new HtmlString(string.Empty);
            }
        }

        private static void LogError(string errorMessage)
        {
            var logger = Umbraco.Web.Composing.Current.Factory.GetInstance<ILogger>();
            logger.Error(typeof(SvgHelper), errorMessage);
        }

        private static void LogError(Exception exception, string errorMessage)
        {
            var logger = Umbraco.Web.Composing.Current.Factory.GetInstance<ILogger>();
            logger.Error(typeof(SvgHelper), exception, errorMessage);
        }
    }
}