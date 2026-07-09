using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace DepartmentApp
{
    [DependsOn(typeof(DepartmentAppCoreModule), typeof(AbpAutoMapperModule))]
    public class DepartmentAppApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg =>
            {
                cfg.AddMaps(typeof(DepartmentAppApplicationModule).GetAssembly());
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(DepartmentAppApplicationModule).GetAssembly());
        }
    }
}
