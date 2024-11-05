1. Tạo Project webapi
2. Cai đặt thu vien cho entity framework(data model)
dotnet add package Pomelo.EntityFrameworkCore.MySql(Pomelo MySQL Provider)
dotnet add package Microsoft.EntityFrameworkCore.Tools
(Neu bi loi: dotnet tool install --global dotnet-ef)
3. Dong bo hoa voi Database (Create Migration)
dotnet ef migrations add InitialCreate
dotnet ef database update



DbSet 