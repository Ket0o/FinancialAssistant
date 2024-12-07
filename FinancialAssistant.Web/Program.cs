using System.Text;
using FinancialAssistant.Authentication;
using FinancialAssistant.Authentication.Implementation;
using FinancialAssistant.Authentication.Options;
using FinancialAssistant.DataAccess;
using FinancialAssistant.EmailService;
using FinancialAssistant.EmailService.Implementation;
using FinancialAssistant.EmojiService;
using FinancialAssistant.EmojiService.Implementation;
using FinancialAssistant.EmojiService.Options;
using FinancialAssistant.ExchangeRates;
using FinancialAssistant.ExchangeRates.Implementation;
using FinancialAssistant.ExchangeRates.Options;
using FinancialAssistant.Repository;
using FinancialAssistant.Repository.Implementation;
using FinancialAssistant.UserIdentity;
using FinancialAssistant.Web.Services;
using FinancialAssistant.Web.Services.Implementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<StartupBase>();
}

builder.Services.AddDbContext<DataContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DataBase")));

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddCors();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        AuthorizationSettings authorizationSettings;

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration.GetValue<string>(
                $"{nameof(authorizationSettings)}:{nameof(authorizationSettings.TokenKey)}")!));
        
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = key,
            ValidateIssuerSigningKey = false
        };
    });

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

builder.Services.Configure<FiatCurrencySettings>(
    builder.Configuration.GetSection(nameof(FiatCurrencySettings)));

builder.Services.Configure<EmojiSettings>(
    builder.Configuration.GetSection(nameof(EmojiSettings)));

builder.Services.Configure<AuthorizationSettings>(
	builder.Configuration.GetSection(nameof(AuthorizationSettings)));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient<EmojiService>();
builder.Services.AddHttpClient<FiatCurrencyService>();
builder.Services.AddHttpClient<CryptoCurrencyService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPasswordResetCodeRepository, PasswordResetCodeRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUserIdentityService, UserIdentityService>();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IPasswordService, PasswordService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IPasswordResetCodeService, PasswordResetCodeService>();
builder.Services.AddTransient<ITransactionService, TransactionService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();

builder.Services.AddSingleton<IEmojiService, EmojiService>();
builder.Services.AddSingleton<IFiatCurrencyService, FiatCurrencyService>();
builder.Services.AddSingleton<ICryptoCurrencyService, CryptoCurrencyService>();
builder.Services.AddSingleton<ICurrencyRateManager, CurrencyRateManager>();
builder.Services.AddSingleton<IGreetingsService, GreetingsService>();

var app = builder.Build();

app.UseCors(corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());


app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});

app.Run();
