using System.Web.Http;
using ItemsMVCWebApp.Models;
using Unity;
using Unity.WebApi;

namespace ItemsMVCWebApp.App_Start
{
    public static class UnityWebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();

            container.RegisterType<AppDbContext>();
            container.RegisterType<IItemRepository, ItemRepository>();

            config.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}