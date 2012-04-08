using Autofac;
using Autofac.Integration.Mvc;
using ImageBank.Core.ImageProcessing;
using ImageBank.Persistence;

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
            containerBuilder.RegisterControllers(typeof (MvcApplication).Assembly);

            containerBuilder.RegisterType<ImageProcessor>()
                .As<IImageProcessor>();

            containerBuilder.RegisterType<ImageResizer>()
                .As<IImageResizer>();

            containerBuilder.RegisterType<ImageBankContext>()
                .AsSelf()
                .InstancePerHttpRequest();

            containerBuilder.RegisterType<ImageRepository>()
                .As<IImageRepository>();

            containerBuilder.RegisterType<SettingRepository>()
                .As<ISettingRepository>();
        }
    }
}