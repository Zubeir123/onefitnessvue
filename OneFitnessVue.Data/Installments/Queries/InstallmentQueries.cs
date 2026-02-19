using Microsoft.AspNetCore.Mvc.Rendering;
using FitnessTimeGym.Data.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using FitnessTimeGym.ViewModel.Installments;

namespace FitnessTimeGym.Data.Installments.Queries
{
    public class InstallmentQueries : IInstallmentQueries
    {
      
        public List<SelectListItem> GetInstallments()
        {
            try
            {
                var installments = new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Text = "6 Month",
                        Value = "1"
                    }
                };

                installments.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "---Select---"
                });

                return installments;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public InstallmentEditViewModel GetInstallmentByInstallmentId(int? installmentId)
        {
            try
            {
                var editInstallment = new InstallmentEditViewModel()
                {
                    Status = true,
                    InstallmentId = 1,
                    InstallmentName = "6 Month",
                    InstallmentMonths = 6
                };



                return editInstallment;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}