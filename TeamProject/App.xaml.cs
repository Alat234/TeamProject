
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TeamProject.Base;
using TeamProject.Commends.ImageCommend;
using TeamProject.Commends.ImageDbCommand;
using TeamProject.Commends.UserCommends;
using TeamProject.DbContexts;
using TeamProject.Mappers;
using TeamProject.Repositorie;
using TeamProject.Services;
using TeamProject.Store;
using TeamProject.ViewModels;

namespace TeamProject;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private const string ConnectionString = "Data Source=appdb.db";
    private readonly ServiceProvider _serviseProvider;
  public App()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddAutoMapper(typeof(UserMapper));
            services.AddAutoMapper(typeof(ImageMapper));
            //Comand
            services.AddTransient<IModifyRainCommand,ModifyRainCommand>();
            services.AddTransient<IGetUserByIdCommand,GetUserByIdCommand>();
            services.AddTransient<IGetImagesWithSecretTextByUserIdCommand,GetImagesWithSecretTextByUserIdCommand>();
            services.AddTransient<IPixelationCommand,PixelationCommand>();
            services.AddTransient<IAddNoiseCommand,AddNoiseCommand>();
            services.AddTransient<IUpdateImageAsync,UpdateImageAsync>();
            services.AddTransient<IModifyLinesCommand,ModifyLinesCommand>();
            services.AddTransient<IAddSecretTextCommand,AddSecretTextCommand>();
            services.AddTransient<IGetSecretText,GetSecretTextCommand>();
            services.AddTransient<IDeleteImageById,DeleteImageById>();
            services.AddTransient<IGetListOfImageByUserIdCommend,GetListOfImageByUserIdCommend>();
            services.AddTransient<IGetImgeByIdCommand,GetImageByIdCommand>();
            services.AddTransient<IAddImageAsyn,AddImageAsyn>();
            services.AddTransient<IRegistrationCommend,RegistrationCommend>();
            services.AddTransient<ILoginCommend,LoginCommend>();
            
            services.AddSingleton<DbContextFactory>(provider =>
                new DbContextFactory("Data Source=C:/Users/Влад/RiderProjects/TeamProject/TeamProject/appdb.db"));
         
            
            //Servisec
            
            services.AddTransient<IImageEditorService,ImageEditorService>(); 
            services.AddSingleton<ImageStore>(); 
            services.AddTransient<IImageService,ImageService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IImageRepository, ImageRepository>();
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<NavigationStore>();
            //ViewModel
            services.AddSingleton<MainWindow>(provider =>
                new MainWindow
                {
                    DataContext = provider.GetRequiredService<MainViewModal>()
                });
            
            services.AddTransient<MainViewModal>();
            services.AddTransient<NavigationBarViewModel>();
            services.AddTransient<MyImageViewModel>();
            services.AddTransient<SecretTextlistViewModel>();
            services.AddTransient<ImageEditorViewModel>();
            services.AddTransient<LoginViewModal>();
            services.AddTransient<RegistrationViewModel>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<Func<Type, ViewModelBase>>(serviceProvider =>
                viewModelType => (ViewModelBase)serviceProvider.GetRequiredService(viewModelType));
            _serviseProvider = services.BuildServiceProvider();
            
            var authService = _serviseProvider.GetRequiredService<IAuthenticationService>();
            authService.LoadUser();
        }
   protected override void OnStartup(StartupEventArgs e)
   {
       var mainWindow = _serviseProvider.GetRequiredService<MainWindow>();
       mainWindow.Show();
       base.OnStartup(e);
   }
}