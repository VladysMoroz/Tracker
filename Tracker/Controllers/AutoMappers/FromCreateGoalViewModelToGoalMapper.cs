using AutoMapper;
using Tracker.Entitites;
using Tracker.Entitites.ViewModels;

namespace Tracker.Controllers.AutoMappers
{
    public class FromCreateGoalViewModelToGoalMapper : Profile
    {
        public FromCreateGoalViewModelToGoalMapper()
        {
            CreateMap<GoalForCreatingViewModel, Goal>()

                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.DeadLine, opt => opt.MapFrom(src => src.DeadLine))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.DailyLimit, opt => opt.MapFrom(src => src.DailyLimit));
        }
    }
}
