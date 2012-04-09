using Autofac;
using Autofac.Integration.Mvc;
using ImageBank.Persistence;
using ImageBank.Services.Account;
using ImageBank.Services.ImageProcessing;
using ImageBank.Services.Virtual;

namespace ImageBank.Web
{
    public class AutofacBootstrapper
    {
        public IContainer InitializeContainer()
        {
            var containerBuilder = new ContainerBuilder();
            ConfigureContainer(containerBuilder);

            return containerBuilder.Build();
        }

        private void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            #region Controllers

            containerBuilder.RegisterControllers(typeof (MvcApplication).Assembly);

            #endregion

            #region Repositories

            containerBuilder.RegisterType<ImageBankContext>()
                .AsSelf()
                .InstancePerHttpRequest();

            containerBuilder.RegisterType<ImageRepository>()
                .As<IImageRepository>();

            containerBuilder.RegisterType<SettingRepository>()
                .As<ISettingRepository>();

            containerBuilder.RegisterType<UserRepository>()
                .As<IUserRepository>();

            #endregion

            #region Services

            containerBuilder.RegisterType<ImageProcessor>()
                .As<IImageProcessor>();

            containerBuilder.RegisterType<MipMapGenerator>()
                .As<IMipMapGenerator>();

            containerBuilder.RegisterType<ImageChunkSaver>()
                .As<IImageChunkSaver>()
                .SingleInstance();

            containerBuilder.RegisterType<VirtualPathResolver>()
                .As<IVirtualPathResolver>()
                .SingleInstance();

            containerBuilder.RegisterType<AccountProvider>()
                .As<IAccountProvider>();

            #endregion
        }
    }
}