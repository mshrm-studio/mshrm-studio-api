$env:ASPNETCORE_ENVIRONMENT = 'Development';

# Get the solution folder
$solutionFolder = (get-item $MyInvocation.MyCommand.Path).Directory.FullName

# Get all projects to run EXCEPT the gateway
$projectsToRun = Get-ChildItem $solutionFolder -Recurse -Filter '*.Api.csproj' | Where-Object {$_.Name -notmatch 'Mshrm.Studio.Api.csproj'} | Where-Object {$_.Name -notmatch 'Mshrm.Studio.Shared.csproj'}

# Build projects
$build = Invoke-MsBuild -Path (Get-ChildItem $solutionFolder -Recurse -Filter '*.sln')[0].FullName

# For each project, create a new thread to run an instance and copy across swagger.json
foreach ($project in $projectsToRun)
{
   Write-Host "Running: " + $($project)
   cd $project.Directory
   Invoke-Expression -Command "nswag run /runtime:Net80"
}
