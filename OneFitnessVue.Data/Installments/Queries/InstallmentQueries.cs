using Microsoft.AspNetCore.Mvc.Rendering;
using FitnessTimeGym.Data.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using FitnessTimeGym.ViewModel.Installments;

namespace FitnessTimeGym.Data.Installments.Queries
{
    public class InstallmentQueries : IInstallmentQueries
    {
        private readonly FitnessTimeGymContext _context;

        public InstallmentQueries(FitnessTimeGymContext context)
        {
            _context = context;
        }

        public List<SelectListItem> GetInstallments()
        {
            try
            {
                var installments = _context.Installments
                    .Where(x => x.Status == true)
                    .Select(x => new SelectListItem()
                    {
                        Text = x.InstallmentName,
                        Value = x.InstallmentId.ToString()
                    }).ToList();

                installments.Insert(0, new SelectListItem()
                {
                    Text = "---Select---",
                    Value = ""
                });

                return installments;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public InstallmentEditViewModel GetInstallmentByInstallmentId(int? installmentId)
        {
            try
            {
                var installment = _context.Installments
                    .Where(x => x.InstallmentId == installmentId)
                    .Select(x => new InstallmentEditViewModel()
                    {
                        InstallmentId = x.InstallmentId,
                        InstallmentName = x.InstallmentName,
                        InstallmentMonths = x.InstallmentMonths,
                        Status = x.Status
                    }).FirstOrDefault();

                return installment;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}