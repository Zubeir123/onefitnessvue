using Microsoft.AspNetCore.Mvc.Rendering;
using FitnessTimeGym.Data.EFContext;
using FitnessTimeGym.Model.WorkOut;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessTimeGym.Data.WorkOut.Queries
{
    public class WorkOutQueries : IWorkOutQueries
    {
        private readonly FitnessTimeGymContext _context;

        public WorkOutQueries(FitnessTimeGymContext context)
        {
            _context = context;
        }

        public List<SelectListItem> GetWorkOuts()
        {
            try
            {
                var workoutslist = _context.WorkOuts
                    .Where(x => x.Status == true)
                    .Select(x => new SelectListItem()
                    {
                        Text = x.WorkOutName,
                        Value = x.WorkOutId.ToString()
                    }).ToList();

                workoutslist.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "---Select---"
                });

                return workoutslist;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}