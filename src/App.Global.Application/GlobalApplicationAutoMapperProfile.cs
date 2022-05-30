using App.Global.DataTranferObjects.Emails;
using App.Global.Entitis.Emails;
using AutoMapper;
using Volo.Abp.SettingManagement;

namespace App.Global;

public class GlobalApplicationAutoMapperProfile : Profile
{
    public GlobalApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<EmailSettingsDto, UpdateEmailSettingsDto>();

        CreateMap<EmailConfig, EmailConfigDto>();
        CreateMap<EmailConfigDto, EmailConfig>();
        CreateMap<EmailTemplate, EmailTemplateDto>();
        CreateMap<EmailTemplateDto, EmailTemplate>();
        CreateMap<Service_SendMail, Service_SendMailDto>();
        CreateMap<Service_SendMailDto, Service_SendMail>();
    }
}
