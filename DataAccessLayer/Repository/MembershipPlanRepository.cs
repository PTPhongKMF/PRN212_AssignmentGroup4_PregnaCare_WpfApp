using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository
{
    public class MembershipPlanRepository
    {
        private readonly PregnaCareAppDbContext _context;

        public MembershipPlanRepository()
        {
            _context = new PregnaCareAppDbContext();
        }

        public List<MembershipPlan> GetAllPlans()
        {
            return _context.MembershipPlans
                .Where(p => p.IsDeleted != true)
                .Include(p => p.MembershipPlanFeatures)
                .ThenInclude(mpf => mpf.Feature)
                .ToList();
        }

        public MembershipPlan? GetPlanById(Guid id)
        {
            return _context.MembershipPlans
                .Include(p => p.MembershipPlanFeatures)
                .ThenInclude(mpf => mpf.Feature)
                .FirstOrDefault(p => p.Id == id && p.IsDeleted != true);
        }

        public bool AddPlan(MembershipPlan plan)
        {
            try
            {
                plan.CreatedAt = DateTime.Now;
                plan.UpdatedAt = DateTime.Now;
                _context.MembershipPlans.Add(plan);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdatePlan(MembershipPlan plan)
        {
            try
            {
                var existingPlan = _context.MembershipPlans
                    .Include(p => p.MembershipPlanFeatures)
                    .FirstOrDefault(p => p.Id == plan.Id && p.IsDeleted != true);

                if (existingPlan == null) return false;

                existingPlan.PlanName = plan.PlanName;
                existingPlan.Price = plan.Price;
                existingPlan.Duration = plan.Duration;
                existingPlan.Description = plan.Description;
                existingPlan.ImageUrl = plan.ImageUrl;
                existingPlan.UpdatedAt = DateTime.Now;

                // Update features
                _context.MembershipPlanFeatures.RemoveRange(existingPlan.MembershipPlanFeatures);
                foreach (var feature in plan.MembershipPlanFeatures)
                {
                    feature.Id = Guid.NewGuid();
                    feature.CreatedAt = DateTime.Now;
                    feature.UpdatedAt = DateTime.Now;
                    existingPlan.MembershipPlanFeatures.Add(feature);
                }

                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeletePlan(Guid id)
        {
            try
            {
                var plan = _context.MembershipPlans.FirstOrDefault(p => p.Id == id && p.IsDeleted != true);
                if (plan == null) return false;

                plan.IsDeleted = true;
                plan.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
} 