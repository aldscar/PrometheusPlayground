FROM mcr.microsoft.com/powershell
ENV url="http://localhost:8080/weatherforecast"
ADD Run.ps1 Run.ps1
ENTRYPOINT pwsh Run.ps1 -url $url