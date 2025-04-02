using DataAccessLayer;
using DataAccessLayer.Entities;
using DataAccessLayer.Repository;

namespace BusinessLogicLayer.Services
{
    public class MembershipPlanService
    {
        private readonly MembershipPlanRepository _membershipPlanRepository;
        private readonly UserRepository _userRepository;

        public MembershipPlanService()
        {
            _membershipPlanRepository = new MembershipPlanRepository();
            _userRepository = new UserRepository();
        }

        public List<MembershipPlan> GetAllPlans()
        {
            return _membershipPlanRepository.GetAllPlans();
        }

        public MembershipPlan? GetPlanById(Guid id)
        {
            return _membershipPlanRepository.GetPlanById(id);
        }

        public bool AddPlan(MembershipPlan plan)
        {
            if (string.IsNullOrEmpty(plan.PlanName) || plan.Price <= 0 || plan.Duration <= 0)
                return false;

            return _membershipPlanRepository.AddPlan(plan);
        }

        public bool UpdatePlan(MembershipPlan plan)
        {
            if (string.IsNullOrEmpty(plan.PlanName) || plan.Price <= 0 || plan.Duration <= 0)
                return false;

            return _membershipPlanRepository.UpdatePlan(plan);
        }

        public bool DeletePlan(Guid id)
        {
            return _membershipPlanRepository.DeletePlan(id);
        }

        public bool SubscribeToPlan(Guid userId, Guid planId)
        {
            var user = _userRepository.GetUserById(userId);
            var plan = _membershipPlanRepository.GetPlanById(planId);

            if (user == null || plan == null)
                return false;

            var userPlan = new UserMembershipPlan
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                MembershipPlanId = planId,
                ActivatedAt = DateTime.Now,
                ExpiryDate = DateTime.Now.AddDays(plan.Duration),
                IsActive = true,
                Price = plan.Price,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            try
            {
                var context = new PregnaCareAppDbContext();
                context.UserMembershipPlans.Add(userPlan);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
} 