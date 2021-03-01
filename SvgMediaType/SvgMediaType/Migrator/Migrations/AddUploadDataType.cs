using Umbraco.Core.Logging;
using Umbraco.Core.Migrations;
using Umbraco.Core.Models;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.Composing;

namespace SvgMediaType.Migrator.Migrations
{
    public class AddUploadDataType : MigrationBase
    {
        public AddUploadDataType(IMigrationContext context) : base(context)
        {
        }

        public override void Migrate()
        {
            Logger.Debug<AddUploadDataType>($"Running migration {nameof(AddUploadDataType)}");

            const string dataTypeName = "Upload";
            var dataTypeService = Current.Services.DataTypeService;

            var uploadDataType = dataTypeService.GetDataType(dataTypeName);

            if (uploadDataType != null)
            {
                Logger.Warn(GetType(), $"A data type with the name '{dataTypeName}' already exists. Skipping creation.");
            }
            else
            {
                Current.PropertyEditors.TryGet("Umbraco.UploadField", out IDataEditor uploadPropertyEditor);

                uploadDataType = new DataType(uploadPropertyEditor)
                {
                    Name = dataTypeName,
                    DatabaseType = ValueStorageType.Nvarchar
                };

                dataTypeService.Save(uploadDataType);
            }
        }
    }
}