using System.Threading.Tasks;

namespace App.Global.Data;

public interface IGlobalDbSchemaMigrator
{
    Task MigrateAsync();
}
