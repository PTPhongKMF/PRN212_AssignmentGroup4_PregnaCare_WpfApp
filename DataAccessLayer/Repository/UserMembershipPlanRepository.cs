using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository
{
    public class UserMembershipPlanRepository
    {
        private readonly PregnaCareAppDbContext _context;

        public UserMembershipPlanRepository()
        {
            _context = new PregnaCareAppDbContext();
        }

        public List<UserMembershipPlan> GetUserMembershipPlans(Guid userId)
        {
            return _context.UserMembershipPlans
                .Include(ump => ump.MembershipPlan)
                .Where(ump => ump.UserId == userId && ump.IsDeleted != true)
                .OrderByDescending(ump => ump.ActivatedAt)
                .ToList();
        }

        public bool ExtendMembershipPlan(Guid userMembershipPlanId, int days)
        {
            try
            {
                var plan = _context.UserMembershipPlans
                    .FirstOrDefault(ump => ump.Id == userMembershipPlanId && ump.IsDeleted != true);

                if (plan == null) return false;

                plan.ExpiryDate = plan.ExpiryDate?.AddDays(days);
                plan.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeactivateMembershipPlan(Guid userMembershipPlanId)
        {
            try
            {
                var plan = _context.UserMembershipPlans
                    .FirstOrDefault(ump => ump.Id == userMembershipPlanId && ump.IsDeleted != true);

                if (plan == null) return false;

                plan.IsActive = false;
                plan.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<MembershipPlan> GetAvailablePlans(Guid userId)
        {
            var userPlanIds = _context.UserMembershipPlans
                .Where(ump => ump.UserId == userId && ump.IsDeleted != true)
                .Select(ump => ump.MembershipPlanId)
                .ToList();

            return _context.MembershipPlans
                .Where(mp => !userPlanIds.Contains(mp.Id) && mp.IsDeleted != true)
                .ToList();
        }
    }
} 