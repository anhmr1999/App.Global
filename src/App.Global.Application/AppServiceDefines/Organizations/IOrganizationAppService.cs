using App.Global.DataTranferObjects.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace App.Global.AppServiceDefines.Organizations
{
    public interface IOrganizationAppService : IApplicationService
    {
        Task<IEnumerable<OrganizationResultDto>> GetProvinceAsync();
        Task<IEnumerable<OrganizationResultDto>> GetDistricAsync(string provinceCode);
        Task<IEnumerable<OrganizationResultDto>> GetCommuneAsync(string districCode);
    }
}
