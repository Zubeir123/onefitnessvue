using Microsoft.EntityFrameworkCore;
using FitnessTimeGym.Model.Enquiry;
using FitnessTimeGym.Model.GeneralSetting;
using FitnessTimeGym.Model.MemberRegistration;
using FitnessTimeGym.Model.MenuCategory;
using FitnessTimeGym.Model.MenuMaster;
using FitnessTimeGym.Model.PaymentDetails;
using FitnessTimeGym.Model.Reasons;
using FitnessTimeGym.Model.Reporting;
using FitnessTimeGym.Model.UserMaster;
using FitnessTimeGym.Model.Installments;
using FitnessTimeGym.Model.MembershipType;
using FitnessTimeGym.Model.WorkOut;

namespace FitnessTimeGym.Data.EFContext
{
    public class FitnessTimeGymContext : DbContext
    {
        public FitnessTimeGymContext(DbContextOptions<FitnessTimeGymContext> options) : base(options)
        {

        }
    
        public DbSet<MenuCategoryModel> MenuCategorys { get; set; }
        public DbSet<MenuMasterModel> MenuMasters { get; set; }
        public DbSet<UserMasterModel> UserMasters { get; set; }
        public DbSet<MemberRegistrationModel> MemberRegistrationModels { get; set; }
        public DbSet<PaymentDetailsModel> PaymentDetailsModels { get; set; }
        public DbSet<GeneralSettingsModel> GeneralSettings { get; set; }
        public DbSet<ReceiptHistoryModel> ReceiptHistory { get; set; }
        public DbSet<EnquiryModel> EnquiryModels { get; set; }
        public DbSet<ReasonsModel> ReasonsModels { get; set; }
        public DbSet<WorkOutModel> WorkOuts { get; set; }
        public DbSet<InstallmentModel> Installments { get; set; }
        public DbSet<MembershipTypeModel> MembershipTypes { get; set; }
    }
}