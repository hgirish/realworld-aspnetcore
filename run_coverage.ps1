$OpenCoverExe = "$HOME\.nuget\packages\opencover\4.6.519\tools\OpenCover.Console.exe"
$ReportGenExe = "$HOME\.nuget\packages\reportgenerator\3.1.2\tools\ReportGenerator.exe"

& $OpenCoverExe -target:"C:\Program Files\dotnet\dotnet.exe" -targetargs:"test RealWorld.IntegrationTests"  -filter:"+[*]* -[xunit*]*" -oldStyle -output:coverage.xml -register:user 


& $ReportGenExe -reports:Coverage.xml -targetdir:Coverage

invoke-item .\Coverage\index.htm
