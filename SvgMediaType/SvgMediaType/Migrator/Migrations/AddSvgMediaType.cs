using System.Collections.Generic;
using System.Linq;
using SvgMediaType.Configuration;
using Umbraco.Core.Logging;
using Umbraco.Core.Migrations;
using Umbraco.Core.Models;
using Umbraco.Web.Composing;

namespace SvgMediaType.Migrator.Migrations
{
    public class AddSvgMediaType : MigrationBase
    {
        public AddSvgMediaType(IMigrationContext context) : base(context)
        {
        }

        public override void Migrate()
        {
            Logger.Debug<AddSvgMediaType>($"Running migration {nameof(AddSvgMediaType)}");

            var mediaTypeService = Current.Services.MediaTypeService;
            var dataTypeService = Current.Services.DataTypeService;

            var svgMediaType = mediaTypeService.Get(SvgMediaTypeConfig.SvgMediaTypeAlias);

            if (svgMediaType != null)
            {
                Logger.Warn(GetType(), $"A media type with the alias '{SvgMediaTypeConfig.SvgMediaTypeAlias}' already exists. Skipping creation.");
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
    }
}