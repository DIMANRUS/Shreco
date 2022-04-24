var builder = WebApplication.CreateBuilder(args);
#region Builder Services

builder.Services.AddControllers().AddJsonOptions(options => {
    //options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;

});
#if DEBUG
builder.Services.AddDbContext<AppContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ShrecoLocalDatabase"), b => b.MigrationsAssembly("Shreco.API")));
#else
builder.Services.AddDbContext<AppContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ShrecoDatabase"), b => b.MigrationsAssembly("Shreco.API")));
#endif
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])),
        RequireExpirationTime = false
    };
}).AddJwtBearer("SessionJWT", options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SessionKey"])),
        RequireExpirationTime = true
    };
}).AddJwtBearer("QrJWT", options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:QrKey"])),
    };
});
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ICodeService, CodeService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IQrService, QrService>();
builder.Services.AddTransient<IHistoryService, HistoryService>();

#endregion
var app = builder.Build();
#region App

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseHsts();
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Map("/", () => 6);
app.Run();

#endregion