jangan langsung di copy paste ya, tapi di sesuaikan sesuai step by step nya

1) Install paket yang dipakai:

```bash
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.Extensions.Http
dotnet add package Newtonsoft.Json
```

2) sesuaikan appsettings.json kalian
3) sesuaikan Program.cs kalian
4) tambahkan BaseResponse.cs di folder Ecommerce/Models/
5) tambahkan AuthResponse.cs di folder Ecommerce/Services/Responses/
6) tambahkan LofginViewModel.cs di folder Ecommerce/Models/
7) Buat service untuk memanggil API dengan nama ProductApiService.cs , bisa dilihat di folder Ecommerce/Services/
8) Buat/ubah Controller kalian untuk Account dengan nama AccountController.cs di folder Ecommerce/Controllers/
9) Ubah tampilan login kalian di folder Views/Account/Login.cshtml
10) Ubah Controller kalian untuk Product di folder Ecommerce/Controllers/ProductController.cs
11) Ubah tampilan Index kalian dengan yang ada disini
12) Pastikan urutan di Program.cs sudah sama
13) Silakan dicoba, jangan lupa nyalakan API nya dulu ya teman-teman


package lain lain
<Project Sdk="Microsoft.NET.Sdk.Web">
  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.Authentication.Cookies" Version="2.3.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.10" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.10">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.10">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="8.0.10" />
    <PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.14.0" />
  </ItemGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore" Version="2.3.0" />
    <PackageReference Include="Microsoft.AspNetCore.Mvc" Version="2.3.0" />
    <PackageReference Include="Microsoft.Extensions.Http" Version="9.0.9" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\Ecommerce.Core\Ecommerce.Core.csproj" />
  </ItemGroup>
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
</Project>


