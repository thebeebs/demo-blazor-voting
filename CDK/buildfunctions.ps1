    [CmdletBinding()]
    param(
    # deployment region
        [string]$Region
    )
    
    # Build the two Lambda functions and place the zip bundles into the assets folder for the
    # CDK deployment process to pick up. Note that these commands assume you have the
    # Amazon.Lambda.Tools global tools package installed (dotnet tool install -g Amazon.Lambda.Tools).
    
    Write-Host "...Building Lambda function packages"
    $project_path = Join-Path ".." "BackEnd" "OnConnect"
    Write-Host $project_path
    $output_path = Join-Path "." "assets" "OnConnect.zip"
    Write-Host $output_path
    dotnet lambda package --project-location $project_path --output-package $output_path
    $project_path = Join-Path ".." "BackEnd" "OnDisconnect"
    $output_path = Join-Path "." "assets" "OnDisconnect.zip"  
    dotnet lambda package --project-location $project_path --output-package $output_path
    $project_path = Join-Path ".." "BackEnd" "SendMessage"
    $output_path = Join-Path "." "assets" "SendMessage.zip"  
    dotnet lambda package --project-location $project_path --output-package $output_path
    
 