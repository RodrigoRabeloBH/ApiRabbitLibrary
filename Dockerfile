FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app
EXPOSE 80

COPY *.sln .
COPY "/Library.Api/Library.Api.csproj" "/Library.Api/"
COPY "/Library.Application/Library.Application.csproj" "/Library.Application/"
COPY "/Library.Domain/Library.Domain.csproj" "/Library.Domain/"
COPY "/Library.RabbitMQ/Library.RabbitMQ.csproj" "/Library.RabbitMQ/"

RUN dotnet restore "/Library.Api/Library.Api.csproj"


COPY . ./
WORKDIR /app/Library.Api
RUN dotnet publish -c Release -o publish 


FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/Library.Api/publish ./
ENTRYPOINT ["dotnet", "Library.Api.dll"]