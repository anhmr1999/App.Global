using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace App.Global.Data;

/* This is used if database provider does't define
 * IGlobalDbSchemaMigrator implementation.
 */
public class NullGlobalDbSchemaMigrator : IGlobalDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
