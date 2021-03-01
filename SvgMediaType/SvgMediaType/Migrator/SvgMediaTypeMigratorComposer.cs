using SvgMediaType.Migrator.Migrations;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Migrations;
using Umbraco.Core.Migrations.Upgrade;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;

namespace SvgMediaType.Migrator
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class SvgMediaTypeMigratorComposer : ComponentComposer<SvgMediaTypeMigratorComponent>, IUserComposer
    {
    }

    public class SvgMediaTypeMigratorComponent : IComponent
    {
        private IScopeProvider _scopeProvider;
        private IMigrationBuilder _migrationBuilder;
        private IKeyValueService _keyValueService;
        private ILogger _logger;

        public SvgMediaTypeMigratorComponent(IScopeProvider scopeProvider, IMigrationBuilder migrationBuilder, IKeyValueService keyValueService, ILogger logger)
        {
            _scopeProvider = scopeProvider;
            _migrationBuilder = migrationBuilder;
            _keyValueService = keyValueService;
            _logger = logger;
        }

        public void Initialize()
        {
            // Create a migration plan for a specific project/feature
            // We can then track that latest migration state/step for this project/feature
            var migrationPlan = new MigrationPlan("SvgMediaTypeMigrationPlan");

            // This is the steps we need to take
            // Each step in the migration adds a unique value
            migrationPlan.From(string.Empty)
                .To<AddLabelLongStringDataType>(nameof(AddLabelLongStringDataType))
                .To<AddLabelStringDataType>(nameof(AddLabelStringDataType))
                .To<AddLabelBigIntDataType>(nameof(AddLabelBigIntDataType))
                .To<AddUploadDataType>(nameof(AddUploadDataType))
                .To<AddSvgMediaType>(nameof(AddSvgMediaType));

            // Go and upgrade our site (Will check if it needs to do the work or not)
            // Based on the current/latest step
            var upgrader = new Upgrader(migrationPlan);
            upgrader.Execute(_scopeProvider, _migrationBuilder, _keyValueService, _logger);
        }

        public void Terminate()
        {
        }
    }
}