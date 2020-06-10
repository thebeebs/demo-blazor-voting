    [CmdletBinding()]
    param(
    # deployment region
        [string]$Region
    )
    
    # Build the two Lambda functions and place the zip bundles into the assets folder for the
    # CDK deployment process to pick up. Note that these commands assume you have the
    # Amazon.Lambda.Tools global tools package installed (dotnet tool install -g Amazon.Lambda.Tools).

    Write-Host "...Building Lambda function packages"

    dotnet lambda package --project-location WorkDocs/Demos/Blazor/BackEnd/OnConnect --output-package WorkDocs/Demos/Blazor/Infrastructure/Assets/OnConnect.zip
    dotnet lambda package --project-location WorkDocs/Demos/Blazor/BackEnd/OnConnect --output-package WorkDocs/Demos/Blazor/Infrastructure/Assets/OnDisconnect.zip
    dotnet lambda package --project-location WorkDocs/Demos/Blazor/BackEnd/OnConnect --output-package WorkDocs/Demos/Blazor/Infrastructure/Assets/SendMessage.zip

 