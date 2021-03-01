using Umbraco.Core.Logging;
using Umbraco.Core.Migrations;
using Umbraco.Core.Models;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.Composing;

namespace SvgMediaType.Migrator.Migrations
{
    public class AddLabelBigIntDataType : MigrationBase
    {
        public AddLabelBigIntDataType(IMigrationContext context) : base(context)
        {
        }

        public override void Migrate()
        {
            Logger.Debug<AddLabelBigIntDataType>($"Running migration {nameof(AddLabelBigIntDataType)}");

            const string dataTypeName = "Label (bigint)";
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
                    Configuration = new LabelConfiguration { ValueType = "BIGINT" }
                };

                dataTypeService.Save(labelLongStringDataType);
            }
        }
    }
}