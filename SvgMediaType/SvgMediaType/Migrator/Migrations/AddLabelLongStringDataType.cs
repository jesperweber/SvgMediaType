using Umbraco.Core.Logging;
using Umbraco.Core.Migrations;
using Umbraco.Core.Models;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.Composing;

namespace SvgMediaType.Migrator.Migrations
{
    public class AddLabelLongStringDataType : MigrationBase
    {
        public AddLabelLongStringDataType(IMigrationContext context) : base(context)
        {
        }

        public override void Migrate()
        {
            Logger.Debug<AddLabelLongStringDataType>($"Running migration {nameof(AddLabelLongStringDataType)}");

            const string dataTypeName = "Label (Long string)";
            var dataTypeService = Current.Services.DataTypeService;

            var labelLongStringDataType = dataTypeService.GetDataType(dataTypeName);

            if (labelLongStringDataType != null)
            {
                Logger.Warn(GetType(), $"A data type with the name '{dataTypeName}' already exists. Skipping creation.");
            }
            else
            {
                Current.PropertyEditors.TryGet("Umbraco.Label", out IDataEditor labelPropertyEditor);

                labelLongStringDataType = new DataType(labelPropertyEditor)
                {
                    Name = dataTypeName,
                    DatabaseType = ValueStorageType.Ntext,
                    Configuration = new LabelConfiguration { ValueType = "TEXT" }
                };

                dataTypeService.Save(labelLongStringDataType);
            }
        }
    }

    public class AddLabelStringDataType : MigrationBase
    {
        public AddLabelStringDataType(IMigrationContext context) : base(context)
        {
        }

        public override void Migrate()
        {
            Logger.Debug<AddLabelStringDataType>($"Running migration {nameof(AddLabelStringDataType)}");

            const string dataTypeName = "Label (string)";
            var dataTypeService = Current.Services.DataTypeService;

            var labelLongStringDataType = dataTypeService.GetDataType(dataTypeName);

            if (labelLongStringDataType != null)
            {
                Logger.Warn(GetType(), $"A data type with the name '{dataTypeName}' already exists. Skipping creation.");
            }
            else
            {
                Current.PropertyEditors.TryGet("Umbraco.Label", out IDataEditor labelPropertyEditor);

                labelLongStringDataType = new DataType(labelPropertyEditor)
                {
                    Name = dataTypeName,
                    DatabaseType = ValueStorageType.Nvarchar,
                    Configuration = new LabelConfiguration { ValueType = "STRING" }
                };

                dataTypeService.Save(labelLongStringDataType);
            }
        }
    }
}