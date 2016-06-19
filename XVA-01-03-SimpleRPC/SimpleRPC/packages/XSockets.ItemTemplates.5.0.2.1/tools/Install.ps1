param($installPath, $toolsPath, $package, $project)

$vsVersion = ""
if($DTE.Version -eq "10.0"){$vsVersion = "2010"}
if($DTE.Version -eq "11.0"){$vsVersion = "2012"}
if($DTE.Version -eq "12.0"){$vsVersion = "2013"}
if($DTE.Version -eq "14.0"){$vsVersion = "2015"}

Try
{
    Write-Host "Installing XSockets item templates for visual studio $($vsVersion)" 

    $itemTemplatesPathXSockets = [Environment]::GetFolderPath("MyDocuments") + "\Visual Studio "+$vsVersion+"\Templates\ItemTemplates\XSockets.NET 5"

    if(!(Test-Path ($itemTemplatesPathXSockets))){New-Item ($itemTemplatesPathXSockets) -type directory}
 
    Get-ChildItem $toolspath -Filter *.zip | ForEach{        
        Copy-Item -Path $_.FullName -Destination $itemTemplatesPathXSockets        
    }
}
Catch
{
    Write-Host "Could not copy XSockets templates to disc"
}