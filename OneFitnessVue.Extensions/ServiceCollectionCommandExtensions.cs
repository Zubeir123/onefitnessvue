using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using FitnessTimeGym.Data.Enquiry.Command;
using FitnessTimeGym.Data.MemberRegistration.Command;
using FitnessTimeGym.Data.PaymentDetails.Command;
using FitnessTimeGym.Data.Reporting.Command;

namespace FitnessTimeGym.Extensions
{
    public static class ServiceCollectionCommandExtensions
    {
        public static IServiceCollection AddServicesCommand(this IServiceCollection services,
            IConfiguration configuration)
        {
            
            services.AddScoped<IMemberRegistrationCommand, MemberRegistrationCommand>();
          
            services.AddScoped<IPaymentDetailsCommand, PaymentDetailsCommand>();
            services.AddScoped<IReportingCommand, ReportingCommand>();
            services.AddScoped<IEnquiryCommand, EnquiryCommand>();
            return services;
        }
    }
}
