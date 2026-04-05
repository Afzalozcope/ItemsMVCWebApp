using System.Web.Mvc;
using ItemsMVCWebApp.Models;
using Unity;
using Unity.Mvc5;

namespace ItemsMVCWebApp
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            container.RegisterType<AppDbContext>();
            container.RegisterType<IItemRepository, ItemRepository>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}