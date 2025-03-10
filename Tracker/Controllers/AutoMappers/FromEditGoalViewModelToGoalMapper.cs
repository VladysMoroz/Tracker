using AutoMapper;
using Tracker.Entitites;
using Tracker.Entitites.ViewModels;

namespace Tracker.Controllers.AutoMappers
{
    public class FromEditGoalViewModelToGoalMapper : Profile
    {
        public FromEditGoalViewModelToGoalMapper()
        {
            CreateMap<GoalForEditingViewModel, Goal>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DeadLine, opt => opt.MapFrom(src => src.DeadLine))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.DailyLimit, opt => opt.MapFrom(src => src.DailyLimit));
        }
    }
}
