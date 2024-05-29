# Push Nuget 

```Powershell
dotnet nuget push "bin/Release/Mshrm.Studio.Shared.1.0.0.nupkg"  --api-key pat --source "github"
```

```Powershell
dotnet nuget add source --username username --password pat --store-password-in-clear-text --name github "https://nuget.pkg.github.com/NAMESPACE/index.json"
```