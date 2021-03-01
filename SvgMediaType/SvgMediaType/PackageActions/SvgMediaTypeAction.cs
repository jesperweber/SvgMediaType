using System.Linq;
using System.Xml.Linq;
using SvgMediaType.Configuration;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.PackageActions;
using Umbraco.Core.Services;
using Umbraco.Web.Composing;

namespace SvgMediaType.PackageActions
{
    internal class SvgMediaTypeAction : IPackageAction
    {
        public string Alias() => "SvgMediaTypeAction";

        public bool Execute(string packageName, XElement xmlData)
        {
            var logger = Current.Logger;
            var mediaTypeService = Current.Services.MediaTypeService;

            CreateSvgMediaType(logger, mediaTypeService);

            return true;
        }

        private void CreateSvgMediaType(ILogger logger, IMediaTypeService mediaTypeService)
        {
            var svgMediaType = mediaTypeService.Get(SvgMediaTypeConfig.SvgMediaTypeAlias);

            if (svgMediaType != null)
            {
                logger.Warn(GetType(), $"A media type with the alias '{SvgMediaTypeConfig.SvgMediaTypeAlias}' already exists. Skipping creation.");
            }
            else
            {
                svgMediaType = new MediaType(-1)
                {
                    Name = "SVG",
                    Alias = SvgMediaTypeConfig.SvgMediaTypeAlias,
                    Icon = "icon-nodes"
                };

                svgMediaType.AddPropertyGroup("SVG");

                var dataTypeService = Current.Services.DataTypeService;
                var dataTypes = dataTypeService.GetAll().ToArray();

                var fileUploadDataType = dataTypes.FirstOrDefault(x => x.Name == "Upload");
                var labelStringDataType = dataTypes.FirstOrDefault(x => x.Name == "Label (string)");
                var labelBigIntDataType = dataTypes.FirstOrDefault(x => x.Name == "Label (bigint)");
                var labelLongStringDataType = dataTypes.FirstOrDefault(x => x.Name == "Label (Long string)");

                svgMediaType.AddPropertyType(new PropertyType(fileUploadDataType) { Alias = "umbracoFile", Name = "Upload SVG", Mandatory = true, SortOrder = 1 }, "SVG");
                svgMediaType.AddPropertyType(new PropertyType(labelStringDataType) { Alias = "umbracoExtension", Name = "Type", SortOrder = 2 }, "SVG");
                svgMediaType.AddPropertyType(new PropertyType(labelBigIntDataType) { Alias = "umbracoBytes", Name = "Size", SortOrder = 3 }, "SVG");
                svgMediaType.AddPropertyType(new PropertyType(labelLongStringDataType) { Alias = "content", Name = "Content", SortOrder = 4 }, "SVG");

                mediaTypeService.Save(svgMediaType);
            }
        }

        public bool Undo(string packageName, XElement xmlData)
        {
            return true;
        }
    }
}