using App.Global.AppServiceDefines.Organizations;
using App.Global.DataTranferObjects.Organizations;
using App.Global.Entitis.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace App.Global.AppServices.Organizations
{
    public class OrganizationAppService : GlobalAppService, IOrganizationAppService
    {
        private readonly IRepository<Province> _provinceRepository;
        private readonly IRepository<District> _districtRepository;
        private readonly IRepository<Commune> _communeRepository;

        public OrganizationAppService(
            IRepository<Province> provinceRepository,
            IRepository<District> districtRepository,
            IRepository<Commune> communeRepository)
        {
            _provinceRepository = provinceRepository;
            _districtRepository = districtRepository;
            _communeRepository = communeRepository;
        }

        public async Task<IEnumerable<OrganizationResultDto>> GetCommuneAsync(string districCode)
        {
            return (await _communeRepository.GetListAsync(x => x.DistrictCode.ToLower() == districCode.ToLower()))
                .Select(x => new OrganizationResultDto(){ Code = x.Code, Name = x.Name }).ToList();
        }

        public async Task<IEnumerable<OrganizationResultDto>> GetDistricAsync(string provinceCode)
        {
            return (await _districtRepository.GetListAsync(x => x.ProvinceCode.ToLower() == provinceCode.ToLower()))
                .Select(x => new OrganizationResultDto(){ Code = x.Code, Name = x.Name }).ToList();
        }

        public async Task<IEnumerable<OrganizationResultDto>> GetProvinceAsync()
        {
            return (await _provinceRepository.GetListAsync())
                .Select(x => new OrganizationResultDto() { Code = x.Code, Name = x.Name }).ToList();
        }
    }
}
