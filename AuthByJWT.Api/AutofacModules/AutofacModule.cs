using AuthByJWT.Core.Repositories;
using AuthByJWT.Core.Services;
using AuthByJWT.Core.UnitOfWorks;
using AuthByJWT.Repository.Repositories;
using AuthByJWT.Repository.UnitOfWorks;
using AuthByJWT.Repository;
using AuthByJWT.Service.Mapping;
using AuthByJWT.Service.Services;
using Autofac;
using System.Reflection;
using Module = Autofac.Module;
using Microsoft.EntityFrameworkCore;

namespace AuthByJWT.Api.AutofacModules
{
    public class AutofacModule : Module
    {
        protected override void Load(Autofac.ContainerBuilder builder)
        {
            //sadece bir tane olan Generic olanları manuel tek tek ekledik
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();

            builder.RegisterType<AppDbContext>().As<DbContext>().InstancePerLifetimeScope();

            //Sdece bir tane olan ama generic olmayanı manuel ekledik
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            //tüm assembly leri al (katmanlar)
            var apiAssembly = Assembly.GetExecutingAssembly(); //mevcutta calısılan assembly
            var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext));
            var serviceAssembly = Assembly.GetAssembly(typeof(MapProfile));


            //AsImplementedInterfaces() => interfaceleri de al
            //InstancePerLifetimeScope() => AddScoped anlamında
            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly)
                .Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly)
                .Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
