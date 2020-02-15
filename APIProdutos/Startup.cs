﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using APIProdutos.Data;
using APIProdutos.Models;
using APIProdutos.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace APIProdutos
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("InMemoryDatabase"));
            services.AddScoped<ProdutoService>();
            services.AddScoped<UsuarioService>();

            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                // Valida a assinatura de um token recebido
                paramsValidation.ValidateIssuerSigningKey = true;

                // Verifica se um token recebido ainda é válido
                paramsValidation.ValidateLifetime = true;

                // Tempo de tolerância para a expiração de um token (utilizado
                // caso haja problemas de sincronismo de horário entre diferentes
                // computadores envolvidos no processo de comunicação)
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            // Ativa o uso do token como forma de autorizar o acesso
            // a recursos deste projeto
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });

            services.AddMvc(option => option.EnableEndpointRouting = false);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
             UsuarioService usrService, ProdutoService produtoService)
        {
            usrService.Incluir(
                new Usuario() { ID = "usuario01", ChaveAcesso = "94be650011cf412ca906fc335f615cdc" });

            usrService.Incluir(
                new Usuario() { ID = "usuario02", ChaveAcesso = "531fd5b19d58438da0fd9afface43b3c" });

            produtoService.Incluir(
                new Produto { CodigoBarras = "11111111111", Nome = "Produto01", Preco = 579 }
                );

            produtoService.Incluir(
              new Produto { CodigoBarras = "2222222222", Nome = "Produto02", Preco = 579 }
              );

            produtoService.Incluir(
              new Produto { CodigoBarras = "333333333333", Nome = "Produto03", Preco = 579 }
              );

            produtoService.Incluir(
              new Produto { CodigoBarras = "4444444444", Nome = "Produto04", Preco = 579 }
              );

            produtoService.Incluir(
              new Produto { CodigoBarras = "5555555555", Nome = "Produto05", Preco = 579 }
              );

            produtoService.Incluir(
              new Produto { CodigoBarras = "66666666666", Nome = "Produto06", Preco = 579 }
              );

            produtoService.Incluir(
              new Produto { CodigoBarras = "77777777777", Nome = "Produto07", Preco = 579 }
              );

            produtoService.Incluir(
              new Produto { CodigoBarras = "888888888888", Nome = "Produto08", Preco = 579 }
              );

            produtoService.Incluir(
              new Produto { CodigoBarras = "99999999999", Nome = "Produto09", Preco = 579 }
              );

            produtoService.Incluir(
              new Produto { CodigoBarras = "10101010101010101010", Nome = "Produto10", Preco = 579 }
              );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}