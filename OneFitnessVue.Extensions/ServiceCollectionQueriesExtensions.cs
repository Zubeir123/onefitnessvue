using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FitnessTimeGym.Common.Notification;
using FitnessTimeGym.Data.Enquiry.Queries;
using FitnessTimeGym.Data.GeneralSetting.Queries;
using FitnessTimeGym.Data.Installments.Queries;
using FitnessTimeGym.Data.MemberRegistration.Queries;
using FitnessTimeGym.Data.MembershipType.Queries;
using FitnessTimeGym.Data.MenuCategory.Queries;
using FitnessTimeGym.Data.MenuMaster.Queries;
using FitnessTimeGym.Data.PaymentDetails.Queries;
using FitnessTimeGym.Data.PaymentType.Queries;
using FitnessTimeGym.Data.Reporting.Queries;
using FitnessTimeGym.Data.TaxMaster.Queries;
using FitnessTimeGym.Data.UserMaster.Queries;
using FitnessTimeGym.Data.WorkOut.Queries;
using FitnessTimeGym.Services.Messages;

namespace FitnessTimeGym.Extensions
{
    public static class ServiceCollectionQueriesExtensions
    {
        public static IServiceCollection AddServicesQueries(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddTransient<IUserMasterQueries, UserMasterQueries>();
        
            services.AddTransient<IMenuMasterQueries, MenuMasterQueries>();
            services.AddTransient<IMenuCategoryQueries, MenuCategoryQueries>();
            
            services.AddTransient<IWorkOutQueries, WorkOutQueries>();
            services.AddTransient<IInstallmentQueries, InstallmentQueries>();

            services.AddTransient<IMembershipTypeQueries, MembershipTypeQueries>();
            services.AddTransient<IMemberRegistrationQueries, MemberRegistrationQueries>();
            services.AddTransient<ITaxMasterQueries, TaxMasterQueries>();
            services.AddTransient<IPaymentTypeQueries, PaymentTypeQueries>();
         
            services.AddTransient<IPaymentDetailsQueries, PaymentDetailsQueries>();
            services.AddTransient<IGeneralSettingQueries, GeneralSettingQueries>();
            services.AddScoped<IReportingQueries, ReportingQueries>();
            services.AddScoped<IEnquiryQueries, EnquiryQueries>();
            return services;
        }
    }
}