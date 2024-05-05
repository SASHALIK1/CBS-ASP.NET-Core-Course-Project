using CBS_ASP.NET_Core_Course_Project.Models;

namespace CBS_ASP.NET_Core_Course_Project
{
    public static class MappingExtensions
    {
        public static LoginServiceModel ToServiceModel(this LoginBindingModel source)
        {
            return new LoginServiceModel()
            {
                Email = source.Login,
                Password = source.Password
            };
        }
    }
}
