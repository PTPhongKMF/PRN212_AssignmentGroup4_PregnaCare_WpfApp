using DataAccessLayer.Entities;
using DataAccessLayer.Repository;

namespace BusinessLogicLayer.Services
{
    public class UserMembershipPlanService
    {
        private readonly UserMembershipPlanRepository _userMembershipPlanRepository;

        public UserMembershipPlanService()
        {
            _userMembershipPlanRepository = new UserMembershipPlanRepository();
        }

        public List<UserMembershipPlan> GetUserMembershipPlans(Guid userId)
        {
            return _userMembershipPlanRepository.GetUserMembershipPlans(userId);
        }

        public bool ExtendMembershipPlan(Guid userMembershipPlanId, int days)
        {
            if (days <= 0) return false;
            return _userMembershipPlanRepository.ExtendMembershipPlan(userMembershipPlanId, days);
        }

        public bool DeactivateMembershipPlan(Guid userMembershipPlanId)
        {
            return _userMembershipPlanRepository.DeactivateMembershipPlan(userMembershipPlanId);
        }

        public List<MembershipPlan> GetAvailablePlans(Guid userId)
        {
            return _userMembershipPlanRepository.GetAvailablePlans(userId);
        }
    }
} 