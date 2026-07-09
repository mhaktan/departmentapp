using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using DepartmentApp.EntityFrameworkCore;

namespace DepartmentApp.Web.Host
{
    [DependsOn(typeof(DepartmentAppApplicationModule), typeof(DepartmentAppEntityFrameworkCoreModule), typeof(AbpAspNetCoreModule))]
    public class DepartmentAppWebHostModule : AbpModule
    {
        public override void PreInitialize()
        {
            // Expose all AppServices as dynamic API controllers
            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(DepartmentAppApplicationModule).GetAssembly(),
                    moduleName: "app",
                    useConventionalHttpVerbs: true
                );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(DepartmentAppWebHostModule).GetAssembly());
        }
    }
}
