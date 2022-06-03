using App.Global.Commons.Helpers;
using App.Global.EventHanlderModels;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace App.Global.Commons.BackgroundJobHanlders
{
    public class EmailSendingJob : AsyncBackgroundJob<EmailSendingArgs>, ITransientDependency
    {
        private readonly EmailHelper _emailHelper;

        public EmailSendingJob(EmailHelper emailHelper)
        {
            _emailHelper = emailHelper;
        }

        public override async Task ExecuteAsync(EmailSendingArgs args)
        {
            await _emailHelper.SendMailAsync(args.EmailId);
        }
    }
}
