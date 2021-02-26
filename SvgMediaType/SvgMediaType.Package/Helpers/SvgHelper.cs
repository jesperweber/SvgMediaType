using System.Web;
using System.Web.Mvc;
using System.Xml;
using SvgMediaType.Package.Configuration;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace SvgMediaType.Package.Helpers
{
    public static class SvgHelper
    {
        public static HtmlString RenderSvg(this HtmlHelper helper, IPublishedContent publishedContentSvg, string title = null)
        {
            var svgContent = publishedContentSvg.Value<string>(SvgMediaTypeConfig.SvgMediaTypeContentPropertyAlias);

            if (string.IsNullOrWhiteSpace(title))
                return new HtmlString(svgContent);

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(svgContent);

            if (xmlDoc.DocumentElement == null || xmlDoc.SelectSingleNode("/title") != null)
                return new HtmlString(svgContent);

            var titleElement = xmlDoc.CreateElement("title", xmlDoc.DocumentElement.NamespaceURI);
            titleElement.InnerText = title;

            xmlDoc.DocumentElement.PrependChild(titleElement);

            return new HtmlString(xmlDoc.OuterXml);
        }
    }
}