using App.Global.Commons.Helpers;
using App.Global.Entitis.Emails;
using App.Global.Entitis.ExcelServices;
using App.Global.EventHanlderModels;
using App.Global.ExcelModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace App.Global.Commons.BackgroundJobHanlders
{
    public class ExcelImportJob : AsyncBackgroundJob<ExcelImportArgs>, ITransientDependency
    {
        public override Task ExecuteAsync(ExcelImportArgs args)
        {
            throw new NotImplementedException();
        }
    }

    public class ExcelExportJob : AsyncBackgroundJob<ExcelExportArgs>, ITransientDependency
    {
        private readonly IRepository<Service_SendMail> _emailRepository;
        private readonly IRepository<ExcelService> _excelRepository;
        private readonly ExcelHelper _excelHelper;
        private readonly EmailHelper _emailHelper;
        public ExcelExportJob(
            IRepository<Service_SendMail> emailRepository,
            IRepository<ExcelService> excelRepository,
            ExcelHelper excelHelper,
            EmailHelper emailHelper,
            FileHelper fileHelper)
        {
            _emailRepository = emailRepository;
            _excelRepository = excelRepository;
            _excelHelper = excelHelper;
            _emailHelper = emailHelper;
        }

        [UnitOfWork]
        public override async Task ExecuteAsync(ExcelExportArgs args)
        {
            var excel = await _excelRepository.FirstOrDefaultAsync(x => x.Id == args.ExcelId);
            if (excel == null)
                return;
            excel.Status = ExcelStatusEnum.Processing;
            await _excelRepository.UpdateAsync(excel);
            var lst = (await _emailRepository.GetQueryableAsync())
                .WhereIf(!args.Filter.IsNullOrEmpty(), x => x.ReceiverEmail.Contains(args.Filter.ToLower()))
                .WhereIf(args.Status.HasValue, x => x.Status == args.Status)
                .Select(x => new EmailExport()
                {
                    ReceiverEmail = x.ReceiverEmail,
                    Content = _emailHelper.GetEmailContent(x).Result,
                    NumberOfTimeSend = x.NumberOfTimeSend,
                    Title = x.Title,
                    Status = ((EmailStatusEnum)x.Status).ToString()
                }).ToList();
            _excelHelper.Export(excel.FileName, args.Headers, lst);
        }
    }
}
